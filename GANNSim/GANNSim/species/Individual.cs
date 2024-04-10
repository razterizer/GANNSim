using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GANNSim.morphology;
using System.Drawing;
using MathLib.geometry._2d;
using MathLib.linalg._2d;

namespace GANNSim.species
{
    class Individual : IComparable<Individual>
    {
        Phenotype m_body;

        System.Xml.XmlDocument m_design_xml_doc;
        double m_fitness;
        double m_norm_fitness;
        bool m_drop_on_head;

        public Individual(System.Xml.XmlDocument design_xml_doc, bool drop_on_head)
        {
            m_body = new Phenotype(design_xml_doc, drop_on_head);
            m_drop_on_head = drop_on_head;

            this.Genome = new Genome();
            this.Genome.Init(m_body.Brain.NumWeights, m_body.Brain.NumNeurons);
            
            //m_body.Build(m_chromosome_neural_weights, m_chromosome_neural_alphas);
            m_fitness = double.NaN;
            m_norm_fitness = double.NaN;

            m_design_xml_doc = design_xml_doc;
        }

        public void Build()
        {
            m_body.Build(this.Genome);
        }

        public void Build(Genome genome)
        {
            this.Genome = genome;
            m_body.Build(this.Genome);
        }

        public void Clear()
        {
            m_body.Clear();
            //m_chromosome_neural_weights.Clear();
            //m_chromosome_neural_alphas.Clear();
        }

        public void ResetAndRandomize()
        {
            m_body.Clear();
            this.Genome.Randomize();
            m_body.Build(this.Genome);
        }

        public void Reset()
        {
            m_body.Clear();
            m_body.Build(this.Genome);
        }

        public Phenotype Body
        {
            get { return m_body; }
        }

        public void CalcFitness(float contact_penalty_factor)
        {
            if (!IsSimulated)
                m_fitness = (double)m_body.MeanPosition.X / (double)Constants.m_canvas_width - (double)contact_penalty_factor * (double)m_body.NumHeadContacts;
        }

        public void NormalizeFitness(double min_fitness, double tot_fitness)
        {
            m_norm_fitness = (m_fitness - min_fitness) / tot_fitness;
        }

        public double Fitness
        {
            get { return m_fitness; }
        }

        public bool IsSimulated
        {
            get { return !double.IsNaN(Fitness); }
        }

        public double NormFitness
        {
            get { return m_norm_fitness; }
        }

        public int CompareTo(Individual other)
        {
            return m_fitness.CompareTo(other.m_fitness);
        }

        public Genome Genome
        {
            get;
            set;
        }

        public Individual[] Mate(Individual partner, int num_crossover_pts, int num_children_per_mating)
        {
            Individual[] children;
            if (num_children_per_mating <= 0)
                num_children_per_mating = 1;
            children = new Individual[num_children_per_mating];
            Genome[] child_genomes = this.Genome.Mate(partner.Genome, num_crossover_pts, num_children_per_mating);
            for (int child_idx = 0; child_idx < children.Length; child_idx++)
            {
                children[child_idx] = new Individual(this.m_design_xml_doc, this.m_drop_on_head);
                children[child_idx].Genome = child_genomes[child_idx];
                children[child_idx].Build();
            }
            return children;
        }

        public void StepSimulation(float fix_dt, Phenotype.IntegrationMethod integration_method,
            bool use_brain,
            int num_main_subiters, int num_brain_subiters, int num_phys_subiters)
        {
            //Vec2 g = new Vec2();
            Vec2 g = new Vec2(0.0f, -9.82f);
            int c_num_tot_subiters = num_main_subiters; //5;
            int c_num_brain_subiters = num_brain_subiters; //1;
            int c_num_phys_subiters = num_phys_subiters; //2;
            float dt = fix_dt;

            for (int sub_iter = 0; sub_iter < c_num_tot_subiters; sub_iter++)
            {
                if (use_brain)
                {
                    m_body.UpdateBrain(dt, c_num_brain_subiters);
                }

                for (int phys_sub_iter = 0; phys_sub_iter < c_num_phys_subiters; phys_sub_iter++)
                {
                    // Delay
                    //for (int i = 0; i < 1000000; i++) { int b = (int)Math.Sin(i * i * i) ^ i; }

                    m_body.UpdateDynamics(dt, g, integration_method);
                }
            }
        }

        public void Draw(Graphics g, int layer_idx, int num_layers, 
            bool debug_draw_joints, bool debug_draw_angsprings,
            bool draw_dna, bool draw_name)
        {
            float t_alpha = (float)layer_idx / (float)num_layers;
            int alpha = 255;
            if (layer_idx >= 1)
                alpha = (int)Math.Round(255.0f * (1.0f - t_alpha) * 0.7f);

            if (float.IsNaN(m_body.MeanPosition.X))
                return;

            AABB aabb = m_body.BoundingBox;
            Vec2 position = new Vec2(m_body.MeanPosition.X, aabb.Max.Y + 20.0f);
            if (draw_dna)
                Genome.DrawDNA(g, position);

            position = new Vec2(m_body.MeanPosition.X, aabb.Max.Y + 40.0f);
            if (draw_name)
                Genome.DrawCheckSum(g, alpha, position);

            m_body.Draw(g, alpha, 
                debug_draw_joints, debug_draw_angsprings);
        }
    }
}
