﻿namespace GANNSim
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonStart = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.textBoxIterations = new System.Windows.Forms.TextBox();
            this.labelIterations = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.textBoxOutputGain = new System.Windows.Forms.TextBox();
            this.labelOutputGain = new System.Windows.Forms.Label();
            this.textBoxTimestep = new System.Windows.Forms.TextBox();
            this.labelTimestep = new System.Windows.Forms.Label();
            this.checkBoxDbgDrawJoints = new System.Windows.Forms.CheckBox();
            this.checkBoxBrain = new System.Windows.Forms.CheckBox();
            this.comboBoxIntegrationMethod = new System.Windows.Forms.ComboBox();
            this.labelIntegrationMethod = new System.Windows.Forms.Label();
            this.labelPopSize = new System.Windows.Forms.Label();
            this.textBoxPopSize = new System.Windows.Forms.TextBox();
            this.labelEvaluationCycles = new System.Windows.Forms.Label();
            this.textBoxEvaluationCycles = new System.Windows.Forms.TextBox();
            this.labelGeneration = new System.Windows.Forms.Label();
            this.textBoxGeneration = new System.Windows.Forms.TextBox();
            this.textBoxNumMatings = new System.Windows.Forms.TextBox();
            this.labelNumMatings = new System.Windows.Forms.Label();
            this.labelMutationRate = new System.Windows.Forms.Label();
            this.textBoxMutationRate = new System.Windows.Forms.TextBox();
            this.labelCrossoverPts = new System.Windows.Forms.Label();
            this.textBoxCrossoverPts = new System.Windows.Forms.TextBox();
            this.checkBoxAdditiveNoise = new System.Windows.Forms.CheckBox();
            this.labelAdditiveNoiseAmplitude = new System.Windows.Forms.Label();
            this.textBoxAdditiveNoiseAmplitude = new System.Windows.Forms.TextBox();
            this.textBoxPhysSubiters = new System.Windows.Forms.TextBox();
            this.labelPhysSubiters = new System.Windows.Forms.Label();
            this.textBoxMainSubiters = new System.Windows.Forms.TextBox();
            this.labelMainSubiters = new System.Windows.Forms.Label();
            this.textBoxBrainSubiters = new System.Windows.Forms.TextBox();
            this.labelBrainSubiters = new System.Windows.Forms.Label();
            this.labelContactPenaltyFactor = new System.Windows.Forms.Label();
            this.textBoxContactPenaltyFactor = new System.Windows.Forms.TextBox();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonPause = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.checkBoxDrawDNA = new System.Windows.Forms.CheckBox();
            this.checkBoxDrawName = new System.Windows.Forms.CheckBox();
            this.checkBoxDropOnHead = new System.Windows.Forms.CheckBox();
            this.checkBoxDbgDrawAngSprings = new System.Windows.Forms.CheckBox();
            this.labelBestFitness = new System.Windows.Forms.Label();
            this.textBoxBestFitness = new System.Windows.Forms.TextBox();
            this.checkBoxShowBest = new System.Windows.Forms.CheckBox();
            this.checkBoxMakeVideos = new System.Windows.Forms.CheckBox();
            this.checkBoxOnlyRunRelevant = new System.Windows.Forms.CheckBox();
            this.comboBoxThreadPriority = new System.Windows.Forms.ComboBox();
            this.labelThreadPriority = new System.Windows.Forms.Label();
            this.textBoxBestChildFitness = new System.Windows.Forms.TextBox();
            this.labelBestChildFitness = new System.Windows.Forms.Label();
            this.textBoxMu = new System.Windows.Forms.TextBox();
            this.textBoxSigma = new System.Windows.Forms.TextBox();
            this.labelMu = new System.Windows.Forms.Label();
            this.labelSigma = new System.Windows.Forms.Label();
            this.textBoxLeaps = new System.Windows.Forms.TextBox();
            this.labelLeaps = new System.Windows.Forms.Label();
            this.labelNumChildrenPerMating = new System.Windows.Forms.Label();
            this.textBoxNumChildrenPerMating = new System.Windows.Forms.TextBox();
            this.labelFlightPenaltyFactor = new System.Windows.Forms.Label();
            this.textBoxFlightPenaltyFactor = new System.Windows.Forms.TextBox();
            this.panelCanvas = new GANNSim.Canvas();
            this.SuspendLayout();
            // 
            // buttonStart
            // 
            this.buttonStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStart.Enabled = false;
            this.buttonStart.Location = new System.Drawing.Point(12, 448);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(75, 23);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // textBoxIterations
            // 
            this.textBoxIterations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxIterations.Enabled = false;
            this.textBoxIterations.Location = new System.Drawing.Point(544, 481);
            this.textBoxIterations.Name = "textBoxIterations";
            this.textBoxIterations.Size = new System.Drawing.Size(100, 20);
            this.textBoxIterations.TabIndex = 2;
            // 
            // labelIterations
            // 
            this.labelIterations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelIterations.AutoSize = true;
            this.labelIterations.Location = new System.Drawing.Point(541, 465);
            this.labelIterations.Name = "labelIterations";
            this.labelIterations.Size = new System.Drawing.Size(63, 13);
            this.labelIterations.TabIndex = 3;
            this.labelIterations.Text = "# Iterations:";
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonReset.Enabled = false;
            this.buttonReset.Location = new System.Drawing.Point(93, 477);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 5;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // textBoxOutputGain
            // 
            this.textBoxOutputGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxOutputGain.Location = new System.Drawing.Point(333, 428);
            this.textBoxOutputGain.Name = "textBoxOutputGain";
            this.textBoxOutputGain.Size = new System.Drawing.Size(100, 20);
            this.textBoxOutputGain.TabIndex = 2;
            this.textBoxOutputGain.Text = "0.5";
            this.textBoxOutputGain.TextChanged += new System.EventHandler(this.textBoxOutputGain_TextChanged);
            // 
            // labelOutputGain
            // 
            this.labelOutputGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelOutputGain.AutoSize = true;
            this.labelOutputGain.Location = new System.Drawing.Point(329, 412);
            this.labelOutputGain.Name = "labelOutputGain";
            this.labelOutputGain.Size = new System.Drawing.Size(67, 13);
            this.labelOutputGain.TabIndex = 3;
            this.labelOutputGain.Text = "Output Gain:";
            // 
            // textBoxTimestep
            // 
            this.textBoxTimestep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxTimestep.Location = new System.Drawing.Point(438, 480);
            this.textBoxTimestep.Name = "textBoxTimestep";
            this.textBoxTimestep.Size = new System.Drawing.Size(100, 20);
            this.textBoxTimestep.TabIndex = 2;
            this.textBoxTimestep.Text = "0.015";
            // 
            // labelTimestep
            // 
            this.labelTimestep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelTimestep.AutoSize = true;
            this.labelTimestep.Location = new System.Drawing.Point(435, 464);
            this.labelTimestep.Name = "labelTimestep";
            this.labelTimestep.Size = new System.Drawing.Size(53, 13);
            this.labelTimestep.TabIndex = 3;
            this.labelTimestep.Text = "Timestep:";
            // 
            // checkBoxDbgDrawJoints
            // 
            this.checkBoxDbgDrawJoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDbgDrawJoints.AutoSize = true;
            this.checkBoxDbgDrawJoints.Location = new System.Drawing.Point(234, 458);
            this.checkBoxDbgDrawJoints.Name = "checkBoxDbgDrawJoints";
            this.checkBoxDbgDrawJoints.Size = new System.Drawing.Size(76, 17);
            this.checkBoxDbgDrawJoints.TabIndex = 6;
            this.checkBoxDbgDrawJoints.Text = "Dbg Joints";
            this.checkBoxDbgDrawJoints.UseVisualStyleBackColor = true;
            // 
            // checkBoxBrain
            // 
            this.checkBoxBrain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxBrain.AutoSize = true;
            this.checkBoxBrain.Checked = true;
            this.checkBoxBrain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxBrain.Location = new System.Drawing.Point(255, 431);
            this.checkBoxBrain.Name = "checkBoxBrain";
            this.checkBoxBrain.Size = new System.Drawing.Size(72, 17);
            this.checkBoxBrain.TabIndex = 6;
            this.checkBoxBrain.Text = "Use Brain";
            this.checkBoxBrain.UseVisualStyleBackColor = true;
            // 
            // comboBoxIntegrationMethod
            // 
            this.comboBoxIntegrationMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxIntegrationMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxIntegrationMethod.FormattingEnabled = true;
            this.comboBoxIntegrationMethod.Items.AddRange(new object[] {
            "Forward Euler",
            "Semi Implicit Euler",
            "Velocity Verlet"});
            this.comboBoxIntegrationMethod.Location = new System.Drawing.Point(438, 440);
            this.comboBoxIntegrationMethod.Name = "comboBoxIntegrationMethod";
            this.comboBoxIntegrationMethod.Size = new System.Drawing.Size(121, 21);
            this.comboBoxIntegrationMethod.TabIndex = 7;
            // 
            // labelIntegrationMethod
            // 
            this.labelIntegrationMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelIntegrationMethod.AutoSize = true;
            this.labelIntegrationMethod.Location = new System.Drawing.Point(435, 424);
            this.labelIntegrationMethod.Name = "labelIntegrationMethod";
            this.labelIntegrationMethod.Size = new System.Drawing.Size(99, 13);
            this.labelIntegrationMethod.TabIndex = 3;
            this.labelIntegrationMethod.Text = "Integration Method:";
            // 
            // labelPopSize
            // 
            this.labelPopSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPopSize.AutoSize = true;
            this.labelPopSize.Location = new System.Drawing.Point(938, 9);
            this.labelPopSize.Name = "labelPopSize";
            this.labelPopSize.Size = new System.Drawing.Size(83, 13);
            this.labelPopSize.TabIndex = 8;
            this.labelPopSize.Text = "Population Size:";
            // 
            // textBoxPopSize
            // 
            this.textBoxPopSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxPopSize.Location = new System.Drawing.Point(941, 25);
            this.textBoxPopSize.Name = "textBoxPopSize";
            this.textBoxPopSize.Size = new System.Drawing.Size(100, 20);
            this.textBoxPopSize.TabIndex = 9;
            this.textBoxPopSize.Text = "40";
            // 
            // labelEvaluationCycles
            // 
            this.labelEvaluationCycles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelEvaluationCycles.AutoSize = true;
            this.labelEvaluationCycles.Location = new System.Drawing.Point(832, 9);
            this.labelEvaluationCycles.Name = "labelEvaluationCycles";
            this.labelEvaluationCycles.Size = new System.Drawing.Size(106, 13);
            this.labelEvaluationCycles.TabIndex = 8;
            this.labelEvaluationCycles.Text = "Evaluation Iterations:";
            // 
            // textBoxEvaluationCycles
            // 
            this.textBoxEvaluationCycles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxEvaluationCycles.Location = new System.Drawing.Point(835, 25);
            this.textBoxEvaluationCycles.Name = "textBoxEvaluationCycles";
            this.textBoxEvaluationCycles.Size = new System.Drawing.Size(100, 20);
            this.textBoxEvaluationCycles.TabIndex = 11;
            this.textBoxEvaluationCycles.Text = "4000";
            // 
            // labelGeneration
            // 
            this.labelGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelGeneration.AutoSize = true;
            this.labelGeneration.Location = new System.Drawing.Point(832, 296);
            this.labelGeneration.Name = "labelGeneration";
            this.labelGeneration.Size = new System.Drawing.Size(62, 13);
            this.labelGeneration.TabIndex = 8;
            this.labelGeneration.Text = "Generation:";
            // 
            // textBoxGeneration
            // 
            this.textBoxGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGeneration.Enabled = false;
            this.textBoxGeneration.Location = new System.Drawing.Point(835, 312);
            this.textBoxGeneration.Name = "textBoxGeneration";
            this.textBoxGeneration.Size = new System.Drawing.Size(100, 20);
            this.textBoxGeneration.TabIndex = 11;
            // 
            // textBoxNumMatings
            // 
            this.textBoxNumMatings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNumMatings.Location = new System.Drawing.Point(835, 64);
            this.textBoxNumMatings.Name = "textBoxNumMatings";
            this.textBoxNumMatings.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumMatings.TabIndex = 11;
            this.textBoxNumMatings.Text = "2";
            // 
            // labelNumMatings
            // 
            this.labelNumMatings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNumMatings.AutoSize = true;
            this.labelNumMatings.Location = new System.Drawing.Point(832, 48);
            this.labelNumMatings.Name = "labelNumMatings";
            this.labelNumMatings.Size = new System.Drawing.Size(57, 13);
            this.labelNumMatings.TabIndex = 8;
            this.labelNumMatings.Text = "# Matings:";
            // 
            // labelMutationRate
            // 
            this.labelMutationRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMutationRate.AutoSize = true;
            this.labelMutationRate.Location = new System.Drawing.Point(832, 87);
            this.labelMutationRate.Name = "labelMutationRate";
            this.labelMutationRate.Size = new System.Drawing.Size(77, 13);
            this.labelMutationRate.TabIndex = 8;
            this.labelMutationRate.Text = "Mutation Rate:";
            // 
            // textBoxMutationRate
            // 
            this.textBoxMutationRate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMutationRate.Location = new System.Drawing.Point(835, 103);
            this.textBoxMutationRate.Name = "textBoxMutationRate";
            this.textBoxMutationRate.Size = new System.Drawing.Size(100, 20);
            this.textBoxMutationRate.TabIndex = 11;
            this.textBoxMutationRate.Text = "1e-3";
            // 
            // labelCrossoverPts
            // 
            this.labelCrossoverPts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelCrossoverPts.AutoSize = true;
            this.labelCrossoverPts.Location = new System.Drawing.Point(938, 87);
            this.labelCrossoverPts.Name = "labelCrossoverPts";
            this.labelCrossoverPts.Size = new System.Drawing.Size(75, 13);
            this.labelCrossoverPts.TabIndex = 8;
            this.labelCrossoverPts.Text = "Crossover Pts:";
            // 
            // textBoxCrossoverPts
            // 
            this.textBoxCrossoverPts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCrossoverPts.Location = new System.Drawing.Point(941, 103);
            this.textBoxCrossoverPts.Name = "textBoxCrossoverPts";
            this.textBoxCrossoverPts.Size = new System.Drawing.Size(100, 20);
            this.textBoxCrossoverPts.TabIndex = 11;
            this.textBoxCrossoverPts.Text = "1";
            // 
            // checkBoxAdditiveNoise
            // 
            this.checkBoxAdditiveNoise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxAdditiveNoise.AutoSize = true;
            this.checkBoxAdditiveNoise.Checked = true;
            this.checkBoxAdditiveNoise.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAdditiveNoise.Location = new System.Drawing.Point(835, 144);
            this.checkBoxAdditiveNoise.Name = "checkBoxAdditiveNoise";
            this.checkBoxAdditiveNoise.Size = new System.Drawing.Size(94, 17);
            this.checkBoxAdditiveNoise.TabIndex = 12;
            this.checkBoxAdditiveNoise.Text = "Additive Noise";
            this.checkBoxAdditiveNoise.UseVisualStyleBackColor = true;
            this.checkBoxAdditiveNoise.CheckedChanged += new System.EventHandler(this.checkBoxAdditiveNoise_CheckedChanged);
            // 
            // labelAdditiveNoiseAmplitude
            // 
            this.labelAdditiveNoiseAmplitude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelAdditiveNoiseAmplitude.AutoSize = true;
            this.labelAdditiveNoiseAmplitude.Location = new System.Drawing.Point(938, 126);
            this.labelAdditiveNoiseAmplitude.Name = "labelAdditiveNoiseAmplitude";
            this.labelAdditiveNoiseAmplitude.Size = new System.Drawing.Size(56, 13);
            this.labelAdditiveNoiseAmplitude.TabIndex = 8;
            this.labelAdditiveNoiseAmplitude.Text = "Amplitude:";
            // 
            // textBoxAdditiveNoiseAmplitude
            // 
            this.textBoxAdditiveNoiseAmplitude.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxAdditiveNoiseAmplitude.Location = new System.Drawing.Point(941, 142);
            this.textBoxAdditiveNoiseAmplitude.Name = "textBoxAdditiveNoiseAmplitude";
            this.textBoxAdditiveNoiseAmplitude.Size = new System.Drawing.Size(100, 20);
            this.textBoxAdditiveNoiseAmplitude.TabIndex = 11;
            this.textBoxAdditiveNoiseAmplitude.Text = "1e-6";
            // 
            // textBoxPhysSubiters
            // 
            this.textBoxPhysSubiters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxPhysSubiters.Location = new System.Drawing.Point(738, 480);
            this.textBoxPhysSubiters.Name = "textBoxPhysSubiters";
            this.textBoxPhysSubiters.Size = new System.Drawing.Size(81, 20);
            this.textBoxPhysSubiters.TabIndex = 2;
            this.textBoxPhysSubiters.Text = "1";
            // 
            // labelPhysSubiters
            // 
            this.labelPhysSubiters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPhysSubiters.AutoSize = true;
            this.labelPhysSubiters.Location = new System.Drawing.Point(735, 464);
            this.labelPhysSubiters.Name = "labelPhysSubiters";
            this.labelPhysSubiters.Size = new System.Drawing.Size(84, 13);
            this.labelPhysSubiters.TabIndex = 3;
            this.labelPhysSubiters.Text = "# Phys Subiters:";
            // 
            // textBoxMainSubiters
            // 
            this.textBoxMainSubiters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxMainSubiters.Location = new System.Drawing.Point(700, 441);
            this.textBoxMainSubiters.Name = "textBoxMainSubiters";
            this.textBoxMainSubiters.Size = new System.Drawing.Size(69, 20);
            this.textBoxMainSubiters.TabIndex = 2;
            this.textBoxMainSubiters.Text = "1000";
            // 
            // labelMainSubiters
            // 
            this.labelMainSubiters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelMainSubiters.AutoSize = true;
            this.labelMainSubiters.Location = new System.Drawing.Point(697, 425);
            this.labelMainSubiters.Name = "labelMainSubiters";
            this.labelMainSubiters.Size = new System.Drawing.Size(123, 13);
            this.labelMainSubiters.TabIndex = 3;
            this.labelMainSubiters.Text = "# Iterations until Render:";
            // 
            // textBoxBrainSubiters
            // 
            this.textBoxBrainSubiters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxBrainSubiters.Location = new System.Drawing.Point(650, 480);
            this.textBoxBrainSubiters.Name = "textBoxBrainSubiters";
            this.textBoxBrainSubiters.Size = new System.Drawing.Size(82, 20);
            this.textBoxBrainSubiters.TabIndex = 2;
            this.textBoxBrainSubiters.Text = "3";
            // 
            // labelBrainSubiters
            // 
            this.labelBrainSubiters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelBrainSubiters.AutoSize = true;
            this.labelBrainSubiters.Location = new System.Drawing.Point(647, 464);
            this.labelBrainSubiters.Name = "labelBrainSubiters";
            this.labelBrainSubiters.Size = new System.Drawing.Size(85, 13);
            this.labelBrainSubiters.TabIndex = 3;
            this.labelBrainSubiters.Text = "# Brain Subiters:";
            // 
            // labelContactPenaltyFactor
            // 
            this.labelContactPenaltyFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelContactPenaltyFactor.AutoSize = true;
            this.labelContactPenaltyFactor.Location = new System.Drawing.Point(832, 166);
            this.labelContactPenaltyFactor.Name = "labelContactPenaltyFactor";
            this.labelContactPenaltyFactor.Size = new System.Drawing.Size(136, 13);
            this.labelContactPenaltyFactor.TabIndex = 8;
            this.labelContactPenaltyFactor.Text = "Concussion Penalty Factor:";
            // 
            // textBoxContactPenaltyFactor
            // 
            this.textBoxContactPenaltyFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxContactPenaltyFactor.Location = new System.Drawing.Point(835, 182);
            this.textBoxContactPenaltyFactor.Name = "textBoxContactPenaltyFactor";
            this.textBoxContactPenaltyFactor.Size = new System.Drawing.Size(100, 20);
            this.textBoxContactPenaltyFactor.TabIndex = 11;
            this.textBoxContactPenaltyFactor.Text = "0.1";
            // 
            // buttonStop
            // 
            this.buttonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonStop.Enabled = false;
            this.buttonStop.Location = new System.Drawing.Point(12, 477);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonPause
            // 
            this.buttonPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonPause.Enabled = false;
            this.buttonPause.Location = new System.Drawing.Point(93, 448);
            this.buttonPause.Name = "buttonPause";
            this.buttonPause.Size = new System.Drawing.Size(75, 23);
            this.buttonPause.TabIndex = 1;
            this.buttonPause.Text = "Pause";
            this.buttonPause.UseVisualStyleBackColor = true;
            this.buttonPause.Click += new System.EventHandler(this.buttonPause_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSave.Enabled = false;
            this.buttonSave.Location = new System.Drawing.Point(12, 419);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 1;
            this.buttonSave.Text = "Save...";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLoad.Location = new System.Drawing.Point(93, 419);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 1;
            this.buttonLoad.Text = "Load...";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = "GANNSim Simulation files|*.ganns";
            this.saveFileDialog1.SupportMultiDottedExtensions = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "GANNSim Simulation Files|*.ganns|GANNSim Design files|*.gannd";
            this.openFileDialog1.FilterIndex = 2;
            this.openFileDialog1.SupportMultiDottedExtensions = true;
            // 
            // checkBoxDrawDNA
            // 
            this.checkBoxDrawDNA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDrawDNA.AutoSize = true;
            this.checkBoxDrawDNA.Location = new System.Drawing.Point(174, 481);
            this.checkBoxDrawDNA.Name = "checkBoxDrawDNA";
            this.checkBoxDrawDNA.Size = new System.Drawing.Size(49, 17);
            this.checkBoxDrawDNA.TabIndex = 6;
            this.checkBoxDrawDNA.Text = "DNA";
            this.checkBoxDrawDNA.UseVisualStyleBackColor = true;
            // 
            // checkBoxDrawName
            // 
            this.checkBoxDrawName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDrawName.AutoSize = true;
            this.checkBoxDrawName.Location = new System.Drawing.Point(174, 458);
            this.checkBoxDrawName.Name = "checkBoxDrawName";
            this.checkBoxDrawName.Size = new System.Drawing.Size(54, 17);
            this.checkBoxDrawName.TabIndex = 6;
            this.checkBoxDrawName.Text = "Name";
            this.checkBoxDrawName.UseVisualStyleBackColor = true;
            // 
            // checkBoxDropOnHead
            // 
            this.checkBoxDropOnHead.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxDropOnHead.AutoSize = true;
            this.checkBoxDropOnHead.Location = new System.Drawing.Point(941, 184);
            this.checkBoxDropOnHead.Name = "checkBoxDropOnHead";
            this.checkBoxDropOnHead.Size = new System.Drawing.Size(95, 17);
            this.checkBoxDropOnHead.TabIndex = 13;
            this.checkBoxDropOnHead.Text = "Drop On Head";
            this.checkBoxDropOnHead.UseVisualStyleBackColor = true;
            // 
            // checkBoxDbgDrawAngSprings
            // 
            this.checkBoxDbgDrawAngSprings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxDbgDrawAngSprings.AutoSize = true;
            this.checkBoxDbgDrawAngSprings.Location = new System.Drawing.Point(234, 481);
            this.checkBoxDbgDrawAngSprings.Name = "checkBoxDbgDrawAngSprings";
            this.checkBoxDbgDrawAngSprings.Size = new System.Drawing.Size(84, 17);
            this.checkBoxDbgDrawAngSprings.TabIndex = 6;
            this.checkBoxDbgDrawAngSprings.Text = "Dbg AngSpr";
            this.checkBoxDbgDrawAngSprings.UseVisualStyleBackColor = true;
            // 
            // labelBestFitness
            // 
            this.labelBestFitness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBestFitness.AutoSize = true;
            this.labelBestFitness.Location = new System.Drawing.Point(938, 296);
            this.labelBestFitness.Name = "labelBestFitness";
            this.labelBestFitness.Size = new System.Drawing.Size(67, 13);
            this.labelBestFitness.TabIndex = 8;
            this.labelBestFitness.Text = "Best Fitness:";
            // 
            // textBoxBestFitness
            // 
            this.textBoxBestFitness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBestFitness.Enabled = false;
            this.textBoxBestFitness.Location = new System.Drawing.Point(941, 312);
            this.textBoxBestFitness.Name = "textBoxBestFitness";
            this.textBoxBestFitness.Size = new System.Drawing.Size(100, 20);
            this.textBoxBestFitness.TabIndex = 11;
            // 
            // checkBoxShowBest
            // 
            this.checkBoxShowBest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxShowBest.AutoSize = true;
            this.checkBoxShowBest.Location = new System.Drawing.Point(599, 443);
            this.checkBoxShowBest.Name = "checkBoxShowBest";
            this.checkBoxShowBest.Size = new System.Drawing.Size(77, 17);
            this.checkBoxShowBest.TabIndex = 14;
            this.checkBoxShowBest.Text = "Show Best";
            this.checkBoxShowBest.UseVisualStyleBackColor = true;
            // 
            // checkBoxMakeVideos
            // 
            this.checkBoxMakeVideos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxMakeVideos.AutoSize = true;
            this.checkBoxMakeVideos.Location = new System.Drawing.Point(599, 420);
            this.checkBoxMakeVideos.Name = "checkBoxMakeVideos";
            this.checkBoxMakeVideos.Size = new System.Drawing.Size(88, 17);
            this.checkBoxMakeVideos.TabIndex = 14;
            this.checkBoxMakeVideos.Text = "Make Videos";
            this.checkBoxMakeVideos.UseVisualStyleBackColor = true;
            // 
            // checkBoxOnlyRunRelevant
            // 
            this.checkBoxOnlyRunRelevant.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBoxOnlyRunRelevant.AutoSize = true;
            this.checkBoxOnlyRunRelevant.Checked = true;
            this.checkBoxOnlyRunRelevant.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxOnlyRunRelevant.Location = new System.Drawing.Point(918, 483);
            this.checkBoxOnlyRunRelevant.Name = "checkBoxOnlyRunRelevant";
            this.checkBoxOnlyRunRelevant.Size = new System.Drawing.Size(116, 17);
            this.checkBoxOnlyRunRelevant.TabIndex = 15;
            this.checkBoxOnlyRunRelevant.Text = "Only Run Relevant";
            this.checkBoxOnlyRunRelevant.UseVisualStyleBackColor = true;
            // 
            // comboBoxThreadPriority
            // 
            this.comboBoxThreadPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxThreadPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxThreadPriority.FormattingEnabled = true;
            this.comboBoxThreadPriority.Items.AddRange(new object[] {
            "Off",
            "Lowest",
            "Below Normal",
            "Normal",
            "Above Normal",
            "Highest"});
            this.comboBoxThreadPriority.Location = new System.Drawing.Point(918, 456);
            this.comboBoxThreadPriority.Name = "comboBoxThreadPriority";
            this.comboBoxThreadPriority.Size = new System.Drawing.Size(121, 21);
            this.comboBoxThreadPriority.TabIndex = 16;
            // 
            // labelThreadPriority
            // 
            this.labelThreadPriority.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelThreadPriority.AutoSize = true;
            this.labelThreadPriority.Location = new System.Drawing.Point(915, 440);
            this.labelThreadPriority.Name = "labelThreadPriority";
            this.labelThreadPriority.Size = new System.Drawing.Size(78, 13);
            this.labelThreadPriority.TabIndex = 17;
            this.labelThreadPriority.Text = "Thread Priority:";
            // 
            // textBoxBestChildFitness
            // 
            this.textBoxBestChildFitness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxBestChildFitness.Enabled = false;
            this.textBoxBestChildFitness.Location = new System.Drawing.Point(941, 351);
            this.textBoxBestChildFitness.Name = "textBoxBestChildFitness";
            this.textBoxBestChildFitness.Size = new System.Drawing.Size(100, 20);
            this.textBoxBestChildFitness.TabIndex = 18;
            // 
            // labelBestChildFitness
            // 
            this.labelBestChildFitness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelBestChildFitness.AutoSize = true;
            this.labelBestChildFitness.Location = new System.Drawing.Point(938, 335);
            this.labelBestChildFitness.Name = "labelBestChildFitness";
            this.labelBestChildFitness.Size = new System.Drawing.Size(93, 13);
            this.labelBestChildFitness.TabIndex = 19;
            this.labelBestChildFitness.Text = "Best Child Fitness:";
            // 
            // textBoxMu
            // 
            this.textBoxMu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMu.Enabled = false;
            this.textBoxMu.Location = new System.Drawing.Point(835, 351);
            this.textBoxMu.Name = "textBoxMu";
            this.textBoxMu.Size = new System.Drawing.Size(48, 20);
            this.textBoxMu.TabIndex = 20;
            // 
            // textBoxSigma
            // 
            this.textBoxSigma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSigma.Enabled = false;
            this.textBoxSigma.Location = new System.Drawing.Point(887, 351);
            this.textBoxSigma.Name = "textBoxSigma";
            this.textBoxSigma.Size = new System.Drawing.Size(48, 20);
            this.textBoxSigma.TabIndex = 21;
            // 
            // labelMu
            // 
            this.labelMu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelMu.AutoSize = true;
            this.labelMu.Location = new System.Drawing.Point(832, 335);
            this.labelMu.Name = "labelMu";
            this.labelMu.Size = new System.Drawing.Size(38, 13);
            this.labelMu.TabIndex = 22;
            this.labelMu.Text = "Pop μ:";
            // 
            // labelSigma
            // 
            this.labelSigma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSigma.AutoSize = true;
            this.labelSigma.Location = new System.Drawing.Point(884, 335);
            this.labelSigma.Name = "labelSigma";
            this.labelSigma.Size = new System.Drawing.Size(39, 13);
            this.labelSigma.TabIndex = 23;
            this.labelSigma.Text = "Pop 𝝈:";
            // 
            // textBoxLeaps
            // 
            this.textBoxLeaps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLeaps.Enabled = false;
            this.textBoxLeaps.Location = new System.Drawing.Point(835, 390);
            this.textBoxLeaps.Name = "textBoxLeaps";
            this.textBoxLeaps.Size = new System.Drawing.Size(100, 20);
            this.textBoxLeaps.TabIndex = 24;
            // 
            // labelLeaps
            // 
            this.labelLeaps.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLeaps.AutoSize = true;
            this.labelLeaps.Location = new System.Drawing.Point(832, 374);
            this.labelLeaps.Name = "labelLeaps";
            this.labelLeaps.Size = new System.Drawing.Size(49, 13);
            this.labelLeaps.TabIndex = 25;
            this.labelLeaps.Text = "# Leaps:";
            // 
            // labelNumChildrenPerMating
            // 
            this.labelNumChildrenPerMating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelNumChildrenPerMating.AutoSize = true;
            this.labelNumChildrenPerMating.Location = new System.Drawing.Point(938, 48);
            this.labelNumChildrenPerMating.Name = "labelNumChildrenPerMating";
            this.labelNumChildrenPerMating.Size = new System.Drawing.Size(101, 13);
            this.labelNumChildrenPerMating.TabIndex = 26;
            this.labelNumChildrenPerMating.Text = "# Children / Mating:";
            // 
            // textBoxNumChildrenPerMating
            // 
            this.textBoxNumChildrenPerMating.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxNumChildrenPerMating.Location = new System.Drawing.Point(941, 64);
            this.textBoxNumChildrenPerMating.Name = "textBoxNumChildrenPerMating";
            this.textBoxNumChildrenPerMating.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumChildrenPerMating.TabIndex = 27;
            this.textBoxNumChildrenPerMating.Text = "2";
            // 
            // labelFlightPenaltyFactor
            // 
            this.labelFlightPenaltyFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelFlightPenaltyFactor.AutoSize = true;
            this.labelFlightPenaltyFactor.Location = new System.Drawing.Point(832, 205);
            this.labelFlightPenaltyFactor.Name = "labelFlightPenaltyFactor";
            this.labelFlightPenaltyFactor.Size = new System.Drawing.Size(106, 13);
            this.labelFlightPenaltyFactor.TabIndex = 28;
            this.labelFlightPenaltyFactor.Text = "Flight Penalty Factor:";
            // 
            // textBoxFlightPenaltyFactor
            // 
            this.textBoxFlightPenaltyFactor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFlightPenaltyFactor.Location = new System.Drawing.Point(835, 221);
            this.textBoxFlightPenaltyFactor.Name = "textBoxFlightPenaltyFactor";
            this.textBoxFlightPenaltyFactor.Size = new System.Drawing.Size(100, 20);
            this.textBoxFlightPenaltyFactor.TabIndex = 29;
            this.textBoxFlightPenaltyFactor.Text = "3e-3";
            // 
            // panelCanvas
            // 
            this.panelCanvas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelCanvas.BackColor = System.Drawing.Color.White;
            this.panelCanvas.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelCanvas.Location = new System.Drawing.Point(12, 12);
            this.panelCanvas.Name = "panelCanvas";
            this.panelCanvas.Size = new System.Drawing.Size(814, 398);
            this.panelCanvas.TabIndex = 0;
            this.panelCanvas.Paint += new System.Windows.Forms.PaintEventHandler(this.panelCanvas_Paint);
            this.panelCanvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelCanvas_MouseDown);
            this.panelCanvas.MouseLeave += new System.EventHandler(this.panelCanvas_MouseLeave);
            this.panelCanvas.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelCanvas_MouseMove);
            this.panelCanvas.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelCanvas_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1046, 512);
            this.Controls.Add(this.textBoxFlightPenaltyFactor);
            this.Controls.Add(this.labelFlightPenaltyFactor);
            this.Controls.Add(this.textBoxNumChildrenPerMating);
            this.Controls.Add(this.labelNumChildrenPerMating);
            this.Controls.Add(this.labelLeaps);
            this.Controls.Add(this.textBoxLeaps);
            this.Controls.Add(this.labelSigma);
            this.Controls.Add(this.labelMu);
            this.Controls.Add(this.textBoxSigma);
            this.Controls.Add(this.textBoxMu);
            this.Controls.Add(this.labelBestChildFitness);
            this.Controls.Add(this.textBoxBestChildFitness);
            this.Controls.Add(this.labelThreadPriority);
            this.Controls.Add(this.comboBoxThreadPriority);
            this.Controls.Add(this.checkBoxOnlyRunRelevant);
            this.Controls.Add(this.checkBoxMakeVideos);
            this.Controls.Add(this.checkBoxShowBest);
            this.Controls.Add(this.checkBoxDropOnHead);
            this.Controls.Add(this.checkBoxAdditiveNoise);
            this.Controls.Add(this.textBoxBestFitness);
            this.Controls.Add(this.textBoxGeneration);
            this.Controls.Add(this.textBoxAdditiveNoiseAmplitude);
            this.Controls.Add(this.textBoxCrossoverPts);
            this.Controls.Add(this.textBoxContactPenaltyFactor);
            this.Controls.Add(this.textBoxMutationRate);
            this.Controls.Add(this.textBoxNumMatings);
            this.Controls.Add(this.textBoxEvaluationCycles);
            this.Controls.Add(this.textBoxPopSize);
            this.Controls.Add(this.labelAdditiveNoiseAmplitude);
            this.Controls.Add(this.labelCrossoverPts);
            this.Controls.Add(this.labelBestFitness);
            this.Controls.Add(this.labelContactPenaltyFactor);
            this.Controls.Add(this.labelGeneration);
            this.Controls.Add(this.labelMutationRate);
            this.Controls.Add(this.labelNumMatings);
            this.Controls.Add(this.labelEvaluationCycles);
            this.Controls.Add(this.labelPopSize);
            this.Controls.Add(this.comboBoxIntegrationMethod);
            this.Controls.Add(this.checkBoxBrain);
            this.Controls.Add(this.checkBoxDrawName);
            this.Controls.Add(this.checkBoxDrawDNA);
            this.Controls.Add(this.checkBoxDbgDrawAngSprings);
            this.Controls.Add(this.checkBoxDbgDrawJoints);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.labelIntegrationMethod);
            this.Controls.Add(this.labelTimestep);
            this.Controls.Add(this.labelOutputGain);
            this.Controls.Add(this.labelBrainSubiters);
            this.Controls.Add(this.labelMainSubiters);
            this.Controls.Add(this.labelPhysSubiters);
            this.Controls.Add(this.labelIterations);
            this.Controls.Add(this.textBoxBrainSubiters);
            this.Controls.Add(this.textBoxMainSubiters);
            this.Controls.Add(this.textBoxTimestep);
            this.Controls.Add(this.textBoxPhysSubiters);
            this.Controls.Add(this.textBoxOutputGain);
            this.Controls.Add(this.textBoxIterations);
            this.Controls.Add(this.buttonPause);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.panelCanvas);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GANNSim";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Canvas panelCanvas;
        private System.Windows.Forms.Button buttonStart;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox textBoxIterations;
        private System.Windows.Forms.Label labelIterations;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.TextBox textBoxOutputGain;
        private System.Windows.Forms.Label labelOutputGain;
        private System.Windows.Forms.TextBox textBoxTimestep;
        private System.Windows.Forms.Label labelTimestep;
        private System.Windows.Forms.CheckBox checkBoxDbgDrawJoints;
        private System.Windows.Forms.CheckBox checkBoxBrain;
        private System.Windows.Forms.ComboBox comboBoxIntegrationMethod;
        private System.Windows.Forms.Label labelIntegrationMethod;
        private System.Windows.Forms.Label labelPopSize;
        private System.Windows.Forms.TextBox textBoxPopSize;
        private System.Windows.Forms.Label labelEvaluationCycles;
        private System.Windows.Forms.TextBox textBoxEvaluationCycles;
        private System.Windows.Forms.Label labelGeneration;
        private System.Windows.Forms.TextBox textBoxGeneration;
        private System.Windows.Forms.TextBox textBoxNumMatings;
        private System.Windows.Forms.Label labelNumMatings;
        private System.Windows.Forms.Label labelMutationRate;
        private System.Windows.Forms.TextBox textBoxMutationRate;
        private System.Windows.Forms.Label labelCrossoverPts;
        private System.Windows.Forms.TextBox textBoxCrossoverPts;
        private System.Windows.Forms.CheckBox checkBoxAdditiveNoise;
        private System.Windows.Forms.Label labelAdditiveNoiseAmplitude;
        private System.Windows.Forms.TextBox textBoxAdditiveNoiseAmplitude;
        private System.Windows.Forms.TextBox textBoxPhysSubiters;
        private System.Windows.Forms.Label labelPhysSubiters;
        private System.Windows.Forms.TextBox textBoxMainSubiters;
        private System.Windows.Forms.Label labelMainSubiters;
        private System.Windows.Forms.TextBox textBoxBrainSubiters;
        private System.Windows.Forms.Label labelBrainSubiters;
        private System.Windows.Forms.Label labelContactPenaltyFactor;
        private System.Windows.Forms.TextBox textBoxContactPenaltyFactor;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonPause;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.CheckBox checkBoxDrawDNA;
        private System.Windows.Forms.CheckBox checkBoxDrawName;
        private System.Windows.Forms.CheckBox checkBoxDropOnHead;
        private System.Windows.Forms.CheckBox checkBoxDbgDrawAngSprings;
        private System.Windows.Forms.Label labelBestFitness;
        private System.Windows.Forms.TextBox textBoxBestFitness;
        private System.Windows.Forms.CheckBox checkBoxShowBest;
        private System.Windows.Forms.CheckBox checkBoxMakeVideos;
    private System.Windows.Forms.CheckBox checkBoxOnlyRunRelevant;
    private System.Windows.Forms.ComboBox comboBoxThreadPriority;
    private System.Windows.Forms.Label labelThreadPriority;
    private System.Windows.Forms.TextBox textBoxBestChildFitness;
    private System.Windows.Forms.Label labelBestChildFitness;
    private System.Windows.Forms.TextBox textBoxMu;
    private System.Windows.Forms.TextBox textBoxSigma;
    private System.Windows.Forms.Label labelMu;
    private System.Windows.Forms.Label labelSigma;
        private System.Windows.Forms.TextBox textBoxLeaps;
        private System.Windows.Forms.Label labelLeaps;
        private System.Windows.Forms.Label labelNumChildrenPerMating;
        private System.Windows.Forms.TextBox textBoxNumChildrenPerMating;
        private System.Windows.Forms.Label labelFlightPenaltyFactor;
        private System.Windows.Forms.TextBox textBoxFlightPenaltyFactor;
    }
}

