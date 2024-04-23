
namespace GANNSim
{
  partial class GraphWindow
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
            this.panelGraphFitness = new System.Windows.Forms.Panel();
            this.panelLegendFitness = new System.Windows.Forms.Panel();
            this.labelLegendBestChildFitness = new System.Windows.Forms.Label();
            this.labelLegendBestFitness = new System.Windows.Forms.Label();
            this.panelGraphStats = new System.Windows.Forms.Panel();
            this.panelLegendStats = new System.Windows.Forms.Panel();
            this.labelLegendRange = new System.Windows.Forms.Label();
            this.labelLegendSigma = new System.Windows.Forms.Label();
            this.labelLegendMu = new System.Windows.Forms.Label();
            this.labelFitness = new System.Windows.Forms.Label();
            this.labelStats = new System.Windows.Forms.Label();
            this.panelGraphDistribution = new System.Windows.Forms.Panel();
            this.labelDistribution = new System.Windows.Forms.Label();
            this.panelGraphFitness.SuspendLayout();
            this.panelLegendFitness.SuspendLayout();
            this.panelGraphStats.SuspendLayout();
            this.panelLegendStats.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelGraphFitness
            // 
            this.panelGraphFitness.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGraphFitness.BackColor = System.Drawing.SystemColors.Window;
            this.panelGraphFitness.Controls.Add(this.panelLegendFitness);
            this.panelGraphFitness.Location = new System.Drawing.Point(12, 28);
            this.panelGraphFitness.Name = "panelGraphFitness";
            this.panelGraphFitness.Size = new System.Drawing.Size(838, 221);
            this.panelGraphFitness.TabIndex = 0;
            this.panelGraphFitness.Paint += new System.Windows.Forms.PaintEventHandler(this.panelGraph_Paint);
            // 
            // panelLegendFitness
            // 
            this.panelLegendFitness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLegendFitness.BackColor = System.Drawing.SystemColors.Window;
            this.panelLegendFitness.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLegendFitness.Controls.Add(this.labelLegendBestChildFitness);
            this.panelLegendFitness.Controls.Add(this.labelLegendBestFitness);
            this.panelLegendFitness.Location = new System.Drawing.Point(3, 178);
            this.panelLegendFitness.Name = "panelLegendFitness";
            this.panelLegendFitness.Size = new System.Drawing.Size(95, 40);
            this.panelLegendFitness.TabIndex = 1;
            this.panelLegendFitness.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelLegendFitness_MouseDown);
            this.panelLegendFitness.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelLegendFitness_MouseMove);
            this.panelLegendFitness.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelLegendFitness_MouseUp);
            // 
            // labelLegendBestChildFitness
            // 
            this.labelLegendBestChildFitness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLegendBestChildFitness.AutoSize = true;
            this.labelLegendBestChildFitness.ForeColor = System.Drawing.Color.Red;
            this.labelLegendBestChildFitness.Location = new System.Drawing.Point(2, 23);
            this.labelLegendBestChildFitness.Name = "labelLegendBestChildFitness";
            this.labelLegendBestChildFitness.Size = new System.Drawing.Size(90, 13);
            this.labelLegendBestChildFitness.TabIndex = 1;
            this.labelLegendBestChildFitness.Text = "Best Child Fitness";
            this.labelLegendBestChildFitness.Click += new System.EventHandler(this.labelLegendBestChildFitness_Click);
            // 
            // labelLegendBestFitness
            // 
            this.labelLegendBestFitness.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLegendBestFitness.AutoSize = true;
            this.labelLegendBestFitness.Location = new System.Drawing.Point(2, 2);
            this.labelLegendBestFitness.Name = "labelLegendBestFitness";
            this.labelLegendBestFitness.Size = new System.Drawing.Size(64, 13);
            this.labelLegendBestFitness.TabIndex = 0;
            this.labelLegendBestFitness.Text = "Best Fitness";
            this.labelLegendBestFitness.Click += new System.EventHandler(this.labelLegendBestFitness_Click);
            // 
            // panelGraphStats
            // 
            this.panelGraphStats.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGraphStats.BackColor = System.Drawing.SystemColors.Window;
            this.panelGraphStats.Controls.Add(this.panelLegendStats);
            this.panelGraphStats.Location = new System.Drawing.Point(12, 281);
            this.panelGraphStats.Name = "panelGraphStats";
            this.panelGraphStats.Size = new System.Drawing.Size(838, 253);
            this.panelGraphStats.TabIndex = 1;
            this.panelGraphStats.Paint += new System.Windows.Forms.PaintEventHandler(this.panelGraphStats_Paint);
            // 
            // panelLegendStats
            // 
            this.panelLegendStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLegendStats.BackColor = System.Drawing.SystemColors.Window;
            this.panelLegendStats.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelLegendStats.Controls.Add(this.labelLegendRange);
            this.panelLegendStats.Controls.Add(this.labelLegendSigma);
            this.panelLegendStats.Controls.Add(this.labelLegendMu);
            this.panelLegendStats.Location = new System.Drawing.Point(3, 195);
            this.panelLegendStats.Name = "panelLegendStats";
            this.panelLegendStats.Size = new System.Drawing.Size(106, 55);
            this.panelLegendStats.TabIndex = 2;
            this.panelLegendStats.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelLegendStats_MouseDown);
            this.panelLegendStats.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelLegendStats_MouseMove);
            this.panelLegendStats.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelLegendStats_MouseUp);
            // 
            // labelLegendRange
            // 
            this.labelLegendRange.AutoSize = true;
            this.labelLegendRange.Location = new System.Drawing.Point(3, 20);
            this.labelLegendRange.Name = "labelLegendRange";
            this.labelLegendRange.Size = new System.Drawing.Size(97, 13);
            this.labelLegendRange.TabIndex = 2;
            this.labelLegendRange.Text = "Pop Fitness Range";
            this.labelLegendRange.Click += new System.EventHandler(this.labelLegendRange_Click);
            // 
            // labelLegendSigma
            // 
            this.labelLegendSigma.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLegendSigma.AutoSize = true;
            this.labelLegendSigma.ForeColor = System.Drawing.Color.Blue;
            this.labelLegendSigma.Location = new System.Drawing.Point(3, 38);
            this.labelLegendSigma.Name = "labelLegendSigma";
            this.labelLegendSigma.Size = new System.Drawing.Size(72, 13);
            this.labelLegendSigma.TabIndex = 1;
            this.labelLegendSigma.Text = "Pop Fitness 𝝈";
            this.labelLegendSigma.Click += new System.EventHandler(this.labelLegendSigma_Click);
            // 
            // labelLegendMu
            // 
            this.labelLegendMu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLegendMu.AutoSize = true;
            this.labelLegendMu.ForeColor = System.Drawing.Color.Green;
            this.labelLegendMu.Location = new System.Drawing.Point(3, 2);
            this.labelLegendMu.Name = "labelLegendMu";
            this.labelLegendMu.Size = new System.Drawing.Size(71, 13);
            this.labelLegendMu.TabIndex = 0;
            this.labelLegendMu.Text = "Pop Fitness μ";
            this.labelLegendMu.Click += new System.EventHandler(this.labelLegendMu_Click);
            // 
            // labelFitness
            // 
            this.labelFitness.AutoSize = true;
            this.labelFitness.Location = new System.Drawing.Point(9, 12);
            this.labelFitness.Name = "labelFitness";
            this.labelFitness.Size = new System.Drawing.Size(67, 13);
            this.labelFitness.TabIndex = 0;
            this.labelFitness.Text = "Best Fitness:";
            // 
            // labelStats
            // 
            this.labelStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStats.Location = new System.Drawing.Point(9, 265);
            this.labelStats.Name = "labelStats";
            this.labelStats.Size = new System.Drawing.Size(146, 13);
            this.labelStats.TabIndex = 0;
            this.labelStats.Text = "Population Fitness Statistics:";
            // 
            // panelGraphDistribution
            // 
            this.panelGraphDistribution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelGraphDistribution.BackColor = System.Drawing.SystemColors.Window;
            this.panelGraphDistribution.Location = new System.Drawing.Point(856, 28);
            this.panelGraphDistribution.Name = "panelGraphDistribution";
            this.panelGraphDistribution.Size = new System.Drawing.Size(81, 506);
            this.panelGraphDistribution.TabIndex = 2;
            this.panelGraphDistribution.Paint += new System.Windows.Forms.PaintEventHandler(this.panelGraphDistribution_Paint);
            // 
            // labelDistribution
            // 
            this.labelDistribution.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDistribution.AutoSize = true;
            this.labelDistribution.Location = new System.Drawing.Point(853, 12);
            this.labelDistribution.Name = "labelDistribution";
            this.labelDistribution.Size = new System.Drawing.Size(62, 13);
            this.labelDistribution.TabIndex = 3;
            this.labelDistribution.Text = "Distribution:";
            // 
            // GraphWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 546);
            this.ControlBox = false;
            this.Controls.Add(this.labelDistribution);
            this.Controls.Add(this.panelGraphDistribution);
            this.Controls.Add(this.panelGraphStats);
            this.Controls.Add(this.labelStats);
            this.Controls.Add(this.labelFitness);
            this.Controls.Add(this.panelGraphFitness);
            this.MinimumSize = new System.Drawing.Size(351, 311);
            this.Name = "GraphWindow";
            this.Text = "Graph";
            this.Load += new System.EventHandler(this.GraphWindow_Load);
            this.SizeChanged += new System.EventHandler(this.GraphWindow_SizeChanged);
            this.panelGraphFitness.ResumeLayout(false);
            this.panelLegendFitness.ResumeLayout(false);
            this.panelLegendFitness.PerformLayout();
            this.panelGraphStats.ResumeLayout(false);
            this.panelLegendStats.ResumeLayout(false);
            this.panelLegendStats.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panelGraphFitness;
        private System.Windows.Forms.Panel panelGraphStats;
        private System.Windows.Forms.Label labelFitness;
        private System.Windows.Forms.Label labelStats;
        private System.Windows.Forms.Panel panelLegendFitness;
        private System.Windows.Forms.Label labelLegendBestChildFitness;
        private System.Windows.Forms.Label labelLegendBestFitness;
        private System.Windows.Forms.Panel panelLegendStats;
        private System.Windows.Forms.Label labelLegendSigma;
        private System.Windows.Forms.Label labelLegendMu;
        private System.Windows.Forms.Label labelLegendRange;
        private System.Windows.Forms.Panel panelGraphDistribution;
        private System.Windows.Forms.Label labelDistribution;
    }
}