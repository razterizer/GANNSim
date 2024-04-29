using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Xml;
using GANNSim.morphology;
using GANNSim.species;
using MathLib.utils;
using System.IO;

// #TODO:
// 1. Add fitness bias.
// 2. Show children in population fitness distribution graph as red lines.
// 3. Simulated annealing by diminishing either the mutation rate or mutation amplitude as a function of generation and/or std-dev.
// -- 4. Make sure widgets update the background worker params properly when changed during simulation.
// 5. Fix crash bug due to all individuals having the same fitness.

namespace GANNSim
{
    public partial class Form1 : Form
    {
        Population m_population;
        int m_num_iters;
        Bitmap m_back_buffer;
        bool m_reset;
        bool m_paused;
        string m_curr_designer_file;
        XmlDocument m_design_xml_doc;
        XmlDocument m_snapshot_xml_doc;
        string m_snapshot_base_path;
        DirectoryInfo m_generation_movie_dir;
        FileInfo m_movie_file;
        string m_movie_dir_base_path;
        GraphWindow m_graph;
        int m_num_leaps = 0;
        double m_curr_best_fitness = 0;
        Brush m_sky_brush;

        class BGWorkerParams
        {
            public float dt;
            public bool use_brain;
            public Phenotype.IntegrationMethod integration_method;
            public int init_pop_size;
            public int evaluation_cycles_limit;
            public int num_matings;
            public float mutation_rate;
            public int num_crossover_pts;
            public bool use_additive_noise;
            public float additive_noise_amp;
            public float output_gain;
            public int num_main_subiters;
            public int num_brain_subiters;
            public int num_phys_subiters;
            public float concussion_penalty_factor;
            public float flight_penalty_factor;
            public bool drop_on_head;
            public int num_children_per_mating;
            public bool show_best;
            public bool only_run_relevant;
            public int thread_priority;
        }
        BGWorkerParams m_bgw_params;

        public Form1()
        {
            InitializeComponent();

            m_back_buffer = null;
            m_population = null;
            m_num_iters = 0;
            m_curr_designer_file = null;
            m_design_xml_doc = null;
            m_snapshot_xml_doc = null;
            m_snapshot_base_path = null;
            Constants.m_canvas_width = panelCanvas.Width;
            Constants.m_canvas_height = panelCanvas.Height;
            m_reset = false;
            m_paused = false;
            comboBoxIntegrationMethod.SelectedIndex = 1;
            comboBoxThreadPriority.SelectedIndex = 3;
            m_bgw_params = new BGWorkerParams();
            m_sky_brush = new SolidBrush(Color.FromArgb(100, Color.LightSkyBlue));
            fill_params();

            m_movie_dir_base_path = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            m_movie_dir_base_path = Path.Combine(m_movie_dir_base_path, "GANNSim");

            next_generation_movie_dir(true);

            m_movie_file = null;
        }
        
        private void Form1_Load(object sender, EventArgs e)
        {
            m_num_iters = 0;
            textBoxIterations.Text = m_num_iters.ToString();

            m_graph = new GraphWindow();
            m_graph.Show();
        }

        private void panelCanvas_Paint(object sender, PaintEventArgs e)
        {
            draw_backbuffer();
            e.Graphics.DrawImageUnscaled(m_back_buffer, Point.Empty);
        }

        private void draw_backbuffer()
        {
            if (m_back_buffer == null)
                m_back_buffer = new Bitmap(panelCanvas.Width, panelCanvas.Height);
            Graphics g = Graphics.FromImage(m_back_buffer);

            int ground_y = (int)Constants.m_ground_y;
            g.Clear(Color.White);
            g.FillRectangle(Brushes.LawnGreen,
                0, panelCanvas.Height - ground_y,
                panelCanvas.Width, ground_y);
            g.FillRectangle(m_sky_brush,
                0, 0, panelCanvas.Width, panelCanvas.Height - ground_y);
            g.DrawLine(Pens.ForestGreen,
                0, panelCanvas.Height - ground_y,
                panelCanvas.Width, panelCanvas.Height - ground_y);
            if (m_population != null)
            {
                try
                {
                    m_population.Draw(g,
                        checkBoxDbgDrawJoints.Checked,
                        checkBoxDbgDrawAngSprings.Checked,
                        checkBoxDrawDNA.Checked,
                        checkBoxDrawName.Checked,
                        m_bgw_params.show_best,
                        m_bgw_params.only_run_relevant);
                }
                catch (Exception /*ex*/)
                {
                    g.DrawLine(Pens.Red, PointF.Empty, new PointF(Constants.m_canvas_width, Constants.m_canvas_height));
                }
            }

            g.Dispose();
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            fill_params();
            if (!m_paused)
            {
                reset_population(false);
                m_paused = false;
            }
            m_population.OutputGain = m_bgw_params.output_gain;
            textBoxGeneration.Text = m_population.NumGenerations.ToString();

            buttonStart.Enabled = false;
            buttonPause.Enabled = true;
            buttonStop.Enabled = true;
            buttonReset.Enabled = true;
            buttonSave.Enabled = m_population.Size > 0;
            checkBoxMakeVideos.Enabled = false;
            try
            {
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Background Process Error");
            }
        }

        private void reset_population(bool reset_ui_widgets)
        {
            if (m_snapshot_xml_doc != null)
                load_snapshot_file(m_snapshot_xml_doc, m_snapshot_base_path, reset_ui_widgets);
            else
            {
                m_population = new Population(
                    m_design_xml_doc,
                    m_bgw_params.drop_on_head,
                    m_bgw_params.mutation_rate,
                    m_bgw_params.use_additive_noise,
                    m_bgw_params.additive_noise_amp,
                    m_bgw_params.init_pop_size);

                XmlNode body_node = m_design_xml_doc.FirstChild["body"];
                XmlAttribute gain_attr = body_node.Attributes["output_gain"];
                m_bgw_params.output_gain = float.Parse(gain_attr.Value, System.Globalization.NumberFormatInfo.InvariantInfo);

                if (textBoxBrainSubiters.Text.Length == 0 && m_population.NumIndividuals > 0)
                    textBoxBrainSubiters.Text = (m_population.FirstIndividual.Body.Brain.NumLayers - 1).ToString();
                if (textBoxOutputGain.Text.Length == 0)
                    textBoxOutputGain.Text = gain_attr.Value;
            }
            next_generation_movie_dir(false);
        }

        private void buttonPause_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = true;
            buttonPause.Enabled = false;
            buttonStop.Enabled = true;
            buttonReset.Enabled = true;
            buttonSave.Enabled = m_population.Size > 0;
            m_paused = true;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            textBoxIterations.Text = "0";
            textBoxLeaps.Text = "0";
            buttonStart.Enabled = true;
            buttonPause.Enabled = false;
            buttonStop.Enabled = false;
            buttonReset.Enabled = false;
            buttonSave.Enabled = m_population.Size > 0;
            checkBoxMakeVideos.Enabled = true;
            backgroundWorker1.CancelAsync();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            m_reset = true;
            textBoxIterations.Text = "0";
            textBoxLeaps.Text = "0";
            buttonSave.Enabled = m_population.Size > 0;
            backgroundWorker1.CancelAsync();
            m_graph.Reset();
            m_num_leaps = 0;
            m_curr_best_fitness = 0;
            buttonStart.Enabled = true;
        }

        void fill_params()
        {
            float f_result;
            int i_result;
            
            if (float.TryParse(textBoxTimestep.Text, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out f_result))
                m_bgw_params.dt = f_result;
            else
                m_bgw_params.dt = 0.0f;

            m_bgw_params.use_brain = checkBoxBrain.Checked;
            
            switch (comboBoxIntegrationMethod.SelectedIndex)
            {
                case 0:
                    m_bgw_params.integration_method = Phenotype.IntegrationMethod.ForwardEuler;
                    break;
                case 1:
                    m_bgw_params.integration_method = Phenotype.IntegrationMethod.SemiImplicitEuler;
                    break;
                case 2:
                    m_bgw_params.integration_method = Phenotype.IntegrationMethod.VelocityVerlet;
                    break;
            }

            if (int.TryParse(textBoxMainSubiters.Text, out i_result))
                m_bgw_params.num_main_subiters = i_result;
            else
                m_bgw_params.num_main_subiters = 1;

            if (int.TryParse(textBoxBrainSubiters.Text, out i_result))
                m_bgw_params.num_brain_subiters = i_result;
            else
                m_bgw_params.num_brain_subiters = 1;

            if (int.TryParse(textBoxPhysSubiters.Text, out i_result))
                m_bgw_params.num_phys_subiters = i_result;
            else
                m_bgw_params.num_phys_subiters = 1;

            // Evolution params

            if (int.TryParse(textBoxPopSize.Text, out i_result))
                m_bgw_params.init_pop_size = i_result;
            else
                m_bgw_params.init_pop_size = 0;

            if (int.TryParse(textBoxEvaluationCycles.Text, out i_result))
                m_bgw_params.evaluation_cycles_limit = i_result;
            else
                m_bgw_params.evaluation_cycles_limit = int.MaxValue;

            if (int.TryParse(textBoxNumMatings.Text, out i_result))
                m_bgw_params.num_matings = i_result;
            else
                m_bgw_params.num_matings = 0;

            if (float.TryParse(textBoxMutationRate.Text, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out f_result))
                m_bgw_params.mutation_rate = f_result;
            else
                m_bgw_params.mutation_rate = 0.0f;

            if (int.TryParse(textBoxCrossoverPts.Text, out i_result))
                m_bgw_params.num_crossover_pts = i_result;
            else
                m_bgw_params.num_crossover_pts = 1;

            m_bgw_params.use_additive_noise = checkBoxAdditiveNoise.Checked;

            if (float.TryParse(textBoxAdditiveNoiseAmplitude.Text, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out f_result))
                m_bgw_params.additive_noise_amp = f_result;
            else
                m_bgw_params.additive_noise_amp = 0.0f;

            if (float.TryParse(textBoxContactPenaltyFactor.Text, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out f_result))
                m_bgw_params.concussion_penalty_factor = f_result;
            else
                m_bgw_params.concussion_penalty_factor = 0.0f;

            m_bgw_params.drop_on_head = checkBoxDropOnHead.Checked;

            if (float.TryParse(textBoxFlightPenaltyFactor.Text, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out f_result))
                m_bgw_params.flight_penalty_factor = f_result;
            else
                m_bgw_params.flight_penalty_factor = 0.0f;

            if (int.TryParse(textBoxNumChildrenPerMating.Text, out i_result))
                m_bgw_params.num_children_per_mating = i_result;
            else
                m_bgw_params.num_children_per_mating = 2;

            // Population params.

            float result;
            if (float.TryParse(textBoxOutputGain.Text, NumberStyles.Any, NumberFormatInfo.InvariantInfo, out result))
                m_bgw_params.output_gain = result;
            else
                m_bgw_params.output_gain = 0.0f;

            m_bgw_params.show_best = checkBoxShowBest.Checked;
            m_bgw_params.only_run_relevant = checkBoxOnlyRunRelevant.Checked;
            m_bgw_params.thread_priority = comboBoxThreadPriority.SelectedIndex;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            e.Result = null;
            if (worker.CancellationPending)
            {
                e.Cancel = true;
            }
            else if (!buttonStart.Enabled)
            {
                // Capping subiters so that we don't accidentally simulate more steps than we set the limit to.
                int remaining_main_subiters = m_bgw_params.num_main_subiters;
                if (m_num_iters + remaining_main_subiters >= m_bgw_params.evaluation_cycles_limit)
                    remaining_main_subiters = m_bgw_params.evaluation_cycles_limit - m_num_iters;
                    
                m_population.CurrPopulationStepSimulation(
                    m_bgw_params.dt,
                    m_bgw_params.integration_method,
                    m_bgw_params.use_brain,
                    remaining_main_subiters,
                    m_bgw_params.num_brain_subiters,
                    m_bgw_params.num_phys_subiters,
                    m_bgw_params.show_best,
                    m_bgw_params.only_run_relevant,
                    m_bgw_params.thread_priority);
                m_num_iters += remaining_main_subiters;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
            {
                // Finally, handle the case where the operation 
                // succeeded.

                // Do it again!
                if (!buttonStart.Enabled)
                {
                    if (m_num_iters >= m_bgw_params.evaluation_cycles_limit)
                    {
                        m_population.CalcFitness(m_bgw_params.concussion_penalty_factor, m_bgw_params.flight_penalty_factor);
                        calc_and_show_stats();
                        if (m_population.Size == 1 || m_bgw_params.show_best)
                        {
                            //m_population.ResetAndRandomize();
                            m_population.Reset();
                        }
                        else if (m_population.Size > 1)
                        {
                            Individual best_individual = m_population.Best;
                            m_population.Breed(
                                m_bgw_params.num_matings, 
                                m_bgw_params.num_crossover_pts,
                                m_bgw_params.num_children_per_mating);
                            m_population.PerformNaturalSelection(
                                m_bgw_params.num_matings * m_bgw_params.num_children_per_mating);
                            m_population.InsertBest(best_individual, 1.0f);
                            m_population.Reset();
                        }
                        //m_population.CalcFitness(m_bgw_params.contact_penalty_factor);

                        m_num_iters = 0;

                        next_generation_movie_dir(false);
                    }

                    draw_backbuffer();
                    panelCanvas.Refresh();
                    dump_current_frame();
                    
                    textBoxIterations.Text = m_num_iters.ToString();
                    textBoxGeneration.Text = m_population.NumGenerations.ToString();
                    textBoxBestFitness.Text = m_population.Best.Fitness.ToString(NumberFormatInfo.InvariantInfo);
                    //Individual individual = m_population.Best;

                    fill_params();
                    m_population.AdditiveNoiseAmplitude = m_bgw_params.additive_noise_amp;
                    m_population.UseAdditiveNoise = m_bgw_params.use_additive_noise;
                    m_population.MutationRate = m_bgw_params.mutation_rate;
                    m_population.DropOnHead = m_bgw_params.drop_on_head;
                    m_population.OutputGain = m_bgw_params.output_gain;
                    backgroundWorker1.RunWorkerAsync();
                }
                else if (m_reset)
                {
                    reset_population(true);
                    m_population.OutputGain = m_bgw_params.output_gain;
                    m_num_iters = 0;
                    //m_population.ResetAndRandomize();
                    buttonStart.Enabled = false;
                    m_reset = false;
                    backgroundWorker1.RunWorkerAsync();
                }
            }
        }

        private void calc_and_show_stats()
        {
            var stats = m_population.CalcStats();
            if (m_curr_best_fitness < stats.best_fitness)
            {
                m_num_leaps++;
                textBoxLeaps.Text = m_num_leaps.ToString();
                m_curr_best_fitness = stats.best_fitness;
            }

            textBoxMu.Text = stats.pop_mu.ToString(NumberFormatInfo.InvariantInfo);
            textBoxSigma.Text = stats.pop_sigma.ToString(NumberFormatInfo.InvariantInfo);
            textBoxBestChildFitness.Text = stats.best_child_fitness.ToString(NumberFormatInfo.InvariantInfo);
            m_graph.AddStats(stats);
        }

        private void panelCanvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (m_population != null)
                m_population.ApplyForce(e.Location);
        }

        private void panelCanvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void panelCanvas_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void panelCanvas_MouseLeave(object sender, EventArgs e)
        {

        }

        private void textBoxOutputGain_TextChanged(object sender, EventArgs e)
        {
        }

        private void checkBoxAdditiveNoise_CheckedChanged(object sender, EventArgs e)
        {
            textBoxAdditiveNoiseAmplitude.Enabled = checkBoxAdditiveNoise.Checked;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == saveFileDialog1.ShowDialog(this))
            {
                XmlDocument doc = new XmlDocument();
                XmlNode root_node = doc.CreateElement("root");
                doc.AppendChild(root_node);
                XmlAttribute design_attr = doc.CreateAttribute("design");
                root_node.Attributes.Append(design_attr);
                design_attr.Value = System.IO.Path.GetFileName(m_curr_designer_file);

                XmlNode settings_node = doc.CreateElement("settings");
                root_node.AppendChild(settings_node);
                XmlNode gain_node = doc.CreateElement("output_gain");
                settings_node.AppendChild(gain_node);
                gain_node.InnerText = textBoxOutputGain.Text;
                XmlNode integration_node = doc.CreateElement("integration_method");
                settings_node.AppendChild(integration_node);
                integration_node.InnerText = comboBoxIntegrationMethod.SelectedItem.ToString();
                XmlAttribute integration_attr = doc.CreateAttribute("index");
                integration_node.Attributes.Append(integration_attr);
                integration_attr.Value = comboBoxIntegrationMethod.SelectedIndex.ToString();
                XmlNode timestep_node = doc.CreateElement("timestep");
                settings_node.AppendChild(timestep_node);
                timestep_node.InnerText = textBoxTimestep.Text;
                XmlNode mainsubiters_node = doc.CreateElement("main_subiters");
                settings_node.AppendChild(mainsubiters_node);
                mainsubiters_node.InnerText = textBoxMainSubiters.Text;
                XmlNode brainsubiters_node = doc.CreateElement("brain_subiters");
                settings_node.AppendChild(brainsubiters_node);
                brainsubiters_node.InnerText = textBoxBrainSubiters.Text;
                XmlNode physsubiters_node = doc.CreateElement("phys_subiters");
                settings_node.AppendChild(physsubiters_node);
                physsubiters_node.InnerText = textBoxPhysSubiters.Text;
                XmlNode popsize_node = doc.CreateElement("popsize");
                settings_node.AppendChild(popsize_node);
                popsize_node.InnerText = textBoxPopSize.Text;
                XmlNode generation_node = doc.CreateElement("generation");
                settings_node.AppendChild(generation_node);
                generation_node.InnerText = textBoxGeneration.Text;
                XmlNode bestfitness_node = doc.CreateElement("best_fitness");
                settings_node.AppendChild(bestfitness_node);
                bestfitness_node.InnerText = textBoxBestFitness.Text;
                XmlNode evaluation_node = doc.CreateElement("evaluation_cycles");
                settings_node.AppendChild(evaluation_node);
                evaluation_node.InnerText = textBoxEvaluationCycles.Text;
                XmlNode children_per_mating_node = doc.CreateElement("children_per_mating");
                settings_node.AppendChild(children_per_mating_node);
                children_per_mating_node.InnerText = textBoxNumChildrenPerMating.Text;
                XmlNode matings_node = doc.CreateElement("num_matings");
                settings_node.AppendChild(matings_node);
                matings_node.InnerText = textBoxNumMatings.Text;
                XmlNode mutation_node = doc.CreateElement("mutation_rate");
                settings_node.AppendChild(mutation_node);
                mutation_node.InnerText = textBoxMutationRate.Text;
                XmlNode crossoverpts_node = doc.CreateElement("num_crossover_pts");
                settings_node.AppendChild(crossoverpts_node);
                crossoverpts_node.InnerText = textBoxCrossoverPts.Text;
                XmlNode additivenoise_node = doc.CreateElement("additive_noise");
                settings_node.AppendChild(additivenoise_node);
                additivenoise_node.InnerText = checkBoxAdditiveNoise.Checked.ToString().ToLower();
                XmlNode noiseamplitude_node = doc.CreateElement("additive_noise_amplitude");
                settings_node.AppendChild(noiseamplitude_node);
                noiseamplitude_node.InnerText = textBoxAdditiveNoiseAmplitude.Text;
                XmlNode concussion_node = doc.CreateElement("concussion_penalty_factor");
                settings_node.AppendChild(concussion_node);
                concussion_node.InnerText = textBoxContactPenaltyFactor.Text;
                XmlNode droponhead_node = doc.CreateElement("drop_on_head");
                settings_node.AppendChild(droponhead_node);
                droponhead_node.InnerText = checkBoxDropOnHead.Checked.ToString().ToLower();
                XmlNode flight_node = doc.CreateElement("flight_penalty_factor");
                settings_node.AppendChild(flight_node);
                flight_node.InnerText = textBoxFlightPenaltyFactor.Text;
                XmlNode onlyrunrelevant_node = doc.CreateElement("only_run_relevant");
                settings_node.AppendChild(onlyrunrelevant_node);
                onlyrunrelevant_node.InnerText = checkBoxOnlyRunRelevant.Checked.ToString().ToLower();
                XmlNode threadpriority_node = doc.CreateElement("thread_priority");
                settings_node.AppendChild(threadpriority_node);
                threadpriority_node.InnerText = comboBoxThreadPriority.SelectedItem.ToString();
                XmlAttribute threadpriority_attr = doc.CreateAttribute("index");
                threadpriority_node.Attributes.Append(threadpriority_attr);
                threadpriority_attr.Value = comboBoxThreadPriority.SelectedIndex.ToString();


                XmlNode population_node = doc.CreateElement("population");
                root_node.AppendChild(population_node);
                foreach (Individual individual in m_population.Individuals)
                {
                    XmlNode individual_node = doc.CreateElement("individual");
                    population_node.AppendChild(individual_node);
                    
                    XmlNode chromosome_neural_weights_node = doc.CreateElement("chromosome_neural_weights");
                    individual_node.AppendChild(chromosome_neural_weights_node);

                    foreach (float basepair in individual.Genome.m_chromosome_neural_weights.DNA)
                    {
                        XmlNode basepair_node = doc.CreateElement("basepair");
                        chromosome_neural_weights_node.AppendChild(basepair_node);

                        XmlAttribute basepair_attr = doc.CreateAttribute("value");
                        basepair_node.Attributes.Append(basepair_attr);
                        basepair_attr.Value = Conversion.Float2Hex(basepair);
                    }

                    XmlNode chromosome_neural_alphas_node = doc.CreateElement("chromosome_neural_alphas");
                    individual_node.AppendChild(chromosome_neural_alphas_node);

                    foreach (float basepair in individual.Genome.m_chromosome_neural_alphas.DNA)
                    {
                        XmlNode basepair_node = doc.CreateElement("basepair");
                        chromosome_neural_alphas_node.AppendChild(basepair_node);

                        XmlAttribute basepair_attr = doc.CreateAttribute("value");
                        basepair_node.Attributes.Append(basepair_attr);
                        basepair_attr.Value = Conversion.Float2Hex(basepair);
                    }
                }

                doc.Save(saveFileDialog1.FileName);
            }
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog(this))
            {
                if (openFileDialog1.FileName.Last() == 's') // Simulation file.
                {
                    m_snapshot_xml_doc = new XmlDocument();
                    m_snapshot_xml_doc.Load(openFileDialog1.FileName);
                    m_snapshot_base_path = System.IO.Path.GetDirectoryName(openFileDialog1.FileName);

                    load_snapshot_file(m_snapshot_xml_doc, m_snapshot_base_path, true);
                }
                else if (openFileDialog1.FileName.Last() == 'd') // Designer file.
                {
                    m_curr_designer_file = System.IO.Path.GetFileName(openFileDialog1.FileName);
                    m_design_xml_doc = new XmlDocument();
                    m_design_xml_doc.Load(openFileDialog1.FileName);
                    m_snapshot_xml_doc = null;
                }

                buttonStart.Enabled = true;
            }
        }

        private void load_snapshot_file(XmlDocument doc, string base_path, bool reset_ui_widgets)
        {
            XmlNode root_node = doc.FirstChild;
            XmlAttribute design_attr = root_node.Attributes["design"];
            m_curr_designer_file = base_path + System.IO.Path.DirectorySeparatorChar + design_attr.Value;
            m_design_xml_doc = new XmlDocument();
            m_design_xml_doc.Load(m_curr_designer_file);

            if (reset_ui_widgets)
            {
                XmlNode settings_node = root_node["settings"];
                textBoxOutputGain.Text = settings_node["output_gain"].InnerText;
                comboBoxIntegrationMethod.SelectedIndex = int.Parse(settings_node["integration_method"].Attributes["index"].Value);
                textBoxTimestep.Text = settings_node["timestep"].InnerText;
                textBoxMainSubiters.Text = settings_node["main_subiters"].InnerText;
                textBoxBrainSubiters.Text = settings_node["brain_subiters"].InnerText;
                textBoxPhysSubiters.Text = settings_node["phys_subiters"].InnerText;
                textBoxPopSize.Text = settings_node["popsize"].InnerText;
                textBoxGeneration.Text = settings_node["generation"].InnerText;
                textBoxBestFitness.Text = settings_node["best_fitness"].InnerText;
                textBoxEvaluationCycles.Text = settings_node["evaluation_cycles"].InnerText;
                textBoxNumChildrenPerMating.Text = settings_node["children_per_mating"].InnerText;
                textBoxNumMatings.Text = settings_node["num_matings"].InnerText;
                textBoxMutationRate.Text = settings_node["mutation_rate"].InnerText;
                textBoxCrossoverPts.Text = settings_node["num_crossover_pts"].InnerText;
                checkBoxAdditiveNoise.Checked = bool.Parse(settings_node["additive_noise"].InnerText);
                textBoxAdditiveNoiseAmplitude.Text = settings_node["additive_noise_amplitude"].InnerText;
                textBoxContactPenaltyFactor.Text = settings_node["concussion_penalty_factor"].InnerText;
                checkBoxDropOnHead.Checked = bool.Parse(settings_node["drop_on_head"].InnerText);
                textBoxFlightPenaltyFactor.Text = settings_node["flight_penalty_factor"].InnerText;
                XmlNode onlyrunrelevant_node = settings_node["only_run_relevant"];
                if (onlyrunrelevant_node != null) // To make it backwards-compatible with earlier xml-model versions.
                    checkBoxOnlyRunRelevant.Checked = bool.Parse(onlyrunrelevant_node.InnerText);
                XmlNode threadpriority_node = settings_node["thread_priority"];
                if (threadpriority_node != null)
                    comboBoxThreadPriority.SelectedIndex = int.Parse(threadpriority_node.Attributes["index"].Value);
            }

            XmlNode population_node = root_node["population"];

            List<Individual> individuals = new List<Individual>();

            foreach (XmlNode individual_node in population_node)
            {
                Individual individual = new Individual(m_design_xml_doc, m_bgw_params.drop_on_head);
                Genome genome = new Genome();

                XmlNode chromosome_neural_weights_node = individual_node["chromosome_neural_weights"];
                List<float> dna_neural_weights = new List<float>();
                foreach (XmlNode basepair_node in chromosome_neural_weights_node)
                {
                    XmlAttribute basepair_attr = basepair_node.Attributes["value"];
                    float basepair = Conversion.Hex2Float(basepair_attr.Value);
                    dna_neural_weights.Add(basepair);
                }
                Chromosome chromosome_neural_weights = new Chromosome(dna_neural_weights.ToArray(), Chromosome.NoiseFunc.UniformPlusMinusOne);

                XmlNode chromosome_neural_alphas_node = individual_node["chromosome_neural_alphas"];
                List<float> dna_neural_alphas = new List<float>();
                foreach (XmlNode basepair_node in chromosome_neural_alphas_node)
                {
                    XmlAttribute basepair_attr = basepair_node.Attributes["value"];
                    float basepair = Conversion.Hex2Float(basepair_attr.Value);
                    dna_neural_alphas.Add(basepair);
                }
                Chromosome chromosome_neural_alphas = new Chromosome(dna_neural_alphas.ToArray(), Chromosome.NoiseFunc.UniformUnity);

                genome.Init(chromosome_neural_weights, chromosome_neural_alphas);

                individual.Build(genome);
                individuals.Add(individual);
            }

            int generations = 0;
            int.TryParse(textBoxGeneration.Text, out generations);

            if (m_population != null)
                m_population.Clear();
            m_population = new Population(
                m_bgw_params.mutation_rate,
                m_bgw_params.use_additive_noise,
                m_bgw_params.additive_noise_amp,
                individuals,
                generations);
        }

        private void next_generation_movie_dir(bool create_first)
        {
            if (checkBoxMakeVideos.Checked)
            {
                if (m_generation_movie_dir != null &&
                    m_generation_movie_dir.Exists && 
                    m_generation_movie_dir.GetFiles().Length > 0)
                {
                    AviFile.AviManager avi_manager = new AviFile.AviManager(m_generation_movie_dir.FullName + ".avi", false);
                    FileInfo[] frame_files = m_generation_movie_dir.GetFiles();
                    Bitmap bitmap = new Bitmap(frame_files[0].FullName);
                    int frame_rate = (int)Math.Round((double)frame_files.Length / (double)m_bgw_params.evaluation_cycles_limit / (double)m_bgw_params.dt);
                    Console.WriteLine("frame_rate = {0}", frame_rate);
                    if (frame_rate > 0)
                    {
                        AviFile.Avi.AVICOMPRESSOPTIONS compress_options = new AviFile.Avi.AVICOMPRESSOPTIONS();
                        compress_options.fccType = 0;
                        compress_options.fccHandler = (UInt32)AviFile.Avi.mmioStringToFOURCC("xvid", 0);
                        compress_options.dwKeyFrameEvery = 0;
                        compress_options.dwQuality = 0;  // 0 .. 10000
                        compress_options.dwFlags = 0;  // AVICOMRPESSF_KEYFRAMES = 4
                        compress_options.dwBytesPerSecond = 0;
                        compress_options.lpFormat = new IntPtr(0);
                        compress_options.cbFormat = 0;
                        compress_options.lpParms = new IntPtr(0);
                        compress_options.cbParms = 0;
                        compress_options.dwInterleaveEvery = 0;
                        // Xvid:
                        //fccType = 0
                        //fccHandler = 1684633208
                        //dwQuality = 0
                        /*Avi.AVICOMPRESSOPTIONS opts = new Avi.AVICOMPRESSOPTIONS();
    opts.fccType         = (UInt32)Avi.mmioStringToFOURCC("vids", 0);
    opts.fccHandler      = (UInt32)Avi.mmioStringToFOURCC("CVID", 0);
    opts.dwKeyFrameEvery = 0;
    opts.dwQuality       = 0;  // 0 .. 10000
    opts.dwFlags         = 0;  // AVICOMRPESSF_KEYFRAMES = 4
    opts.dwBytesPerSecond= 0;
    opts.lpFormat        = new IntPtr(0);
    opts.cbFormat        = 0;
    opts.lpParms         = new IntPtr(0);
    opts.cbParms         = 0;
    opts.dwInterleaveEvery = 0;*/
                        AviFile.VideoStream avi_stream = avi_manager.AddVideoStream(compress_options, frame_rate, bitmap);
                        Console.WriteLine("fccType = {0}", avi_stream.CompressOptions.fccType);
                        Console.WriteLine("fccHandler = {0}", avi_stream.CompressOptions.fccHandler);
                        Console.WriteLine("dwQuality = {0}", avi_stream.CompressOptions.dwQuality);
                        IEnumerable<FileInfo> remaining_frame_files = frame_files.Skip(1);
                        foreach (FileInfo fi in remaining_frame_files)
                        {
                            bitmap = (Bitmap)Bitmap.FromFile(fi.FullName);
                            avi_stream.AddFrame(bitmap);
                        }
                        avi_manager.Close();
                        if (m_generation_movie_dir.Exists)
                            m_generation_movie_dir.Delete(true);
                    }
                }

                m_generation_movie_dir = new DirectoryInfo(Path.Combine(
                    m_movie_dir_base_path,
                    "generation " + m_population.NumGenerations.ToString("00000")));
                m_generation_movie_dir.Create();
            }
        }

        private void dump_current_frame()
        {
            if (!buttonStart.Enabled && checkBoxMakeVideos.Checked)
            {
                string frame_path = Path.Combine(
                    m_generation_movie_dir.FullName,
                    "frame " +
                    m_num_iters.ToString("000000") +
                    ".png");
                m_movie_file = new FileInfo(frame_path);
                FileStream filestream = m_movie_file.OpenWrite();
                m_back_buffer.Save(filestream, System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
