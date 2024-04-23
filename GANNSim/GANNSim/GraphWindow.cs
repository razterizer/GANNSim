using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GANNSim
{
    public partial class GraphWindow : Form
    {

        //private List<Stats> m_stats = new List<Stats>();
        private List<double> best_fitness = new List<double>();
        private List<double> best_child_fitness = new List<double>();
        private List<double> pop_mu = new List<double>();
        private List<double> pop_min = new List<double>();
        private List<double> pop_max = new List<double>();
        private List<double> pop_sigma = new List<double>();
        private double max_best_fitness = Double.NegativeInfinity;
        private double min_best_fitness = Double.PositiveInfinity;
        private double max_best_child_fitness = Double.NegativeInfinity;
        private double min_best_child_fitness = Double.PositiveInfinity;
        private double max_pop_mu = Double.NegativeInfinity;
        private double min_pop_mu = Double.PositiveInfinity;
        private double max_pop_range = Double.NegativeInfinity;
        private double min_pop_range = Double.PositiveInfinity;
        private double max_pop_sigma = Double.NegativeInfinity;
        private double min_pop_sigma = Double.PositiveInfinity;
        private bool enable_draw_best_fitness = true;
        private bool enable_draw_best_child_fitness = true;
        private bool enable_draw_pop_mu = true;
        private bool enable_draw_pop_range = true;
        private bool enable_draw_pop_sigma = true;
        private Bitmap m_back_buffer_fitness = null;
        private Bitmap m_back_buffer_stats = null;
        private Bitmap m_back_buffer_distribution = null;
        private bool m_resized = false;
        private List<double[]> m_pop_fitness_buckets = new List<double[]>();
        private List<double> m_pop_fitness_t_param = new List<double>();
        //private int m_pop_size = 0;
        private Point m_legend_fitness_pos_orig = Point.Empty;
        private Point m_legend_stats_pos_orig = Point.Empty;
        private Point m_legend_fitness_pos_prev = Point.Empty;
        private Point m_legend_stats_pos_prev = Point.Empty;
        private const double m_legend_snap_distance = 10;
        private const double m_legend_snap_distance_sq = m_legend_snap_distance * m_legend_snap_distance;

        public GraphWindow()
        {
            InitializeComponent();
        }

        private void GraphWindow_Load(object sender, EventArgs e)
        {
            m_legend_fitness_pos_orig = panelLegendFitness.Location;
            m_legend_stats_pos_orig = panelLegendStats.Location;
        }

        public void AddStats(Stats s)
        {
            //m_stats.Add(s);

            // We're only interested in the positive values because the negative fitness values can be extremely large.
            s.best_fitness = Math.Max(0, s.best_fitness);
            s.best_child_fitness = Math.Max(0, s.best_child_fitness);
            best_fitness.Add(s.best_fitness);
            best_child_fitness.Add(s.best_child_fitness);
            pop_mu.Add(s.pop_mu);
            pop_min.Add(s.pop_min);
            pop_max.Add(s.pop_max);
            pop_sigma.Add(s.pop_sigma);

#if false
            m_pop_size = s.pop_fitness.Count;
            int num_buckets = panelGraphDistribution.ClientRectangle.Height; //Math.Max(20, m_pop_size/4); //panelGraphStats.ClientRectangle.Height;
            double[] buckets = new double[num_buckets];
            //int fitnesses_per_bucket = (int)(m_pop_size / num_buckets);
            double bucket_fitness_range = (s.pop_max - s.pop_min) / num_buckets;
            double bucket_min = s.pop_min;
            double bucket_max = bucket_min + bucket_fitness_range;
            for (int b_idx = 0; b_idx < num_buckets; ++b_idx)
            {
                buckets[b_idx] = 0;
                //for (int bf_idx = 0; bf_idx < fitnesses_per_bucket; ++bf_idx)
                //    buckets[b_idx] += s.pop_fitness[b_idx * fitnesses_per_bucket + bf_idx];
                foreach (double f in s.pop_fitness)
                    if (bucket_min <= f && f < bucket_max)
                        buckets[b_idx]++;
                bucket_min += bucket_fitness_range;
                bucket_max += bucket_fitness_range;
            }
            m_pop_fitness_buckets.Add(buckets);
#endif

            min_best_fitness = Math.Min(min_best_fitness, s.best_fitness);
            max_best_fitness = Math.Max(max_best_fitness, s.best_fitness);

            min_best_child_fitness = Math.Min(min_best_child_fitness, s.best_child_fitness);
            max_best_child_fitness = Math.Max(max_best_child_fitness, s.best_child_fitness);

            min_pop_mu = Math.Min(min_pop_mu, s.pop_mu);
            max_pop_mu = Math.Max(max_pop_mu, s.pop_mu);

            min_pop_range = Math.Min(min_pop_range, s.pop_min);
            max_pop_range = Math.Max(max_pop_range, s.pop_max);

            min_pop_sigma = Math.Min(min_pop_sigma, s.pop_sigma);
            max_pop_sigma = Math.Max(max_pop_sigma, s.pop_sigma);

            m_pop_fitness_t_param.Clear();
            double range_scale = 1 / (max_pop_range - min_pop_range);
            foreach (double fitness in s.pop_fitness)
            {
                double t = (fitness - min_pop_range)*range_scale;
                m_pop_fitness_t_param.Add(t);
            }

            RefreshGraphs();
        }

        private void panelGraph_Paint(object sender, PaintEventArgs e)
        {
            draw_backbuffer_fitness();
            e.Graphics.DrawImageUnscaled(m_back_buffer_fitness, Point.Empty);
        }

        private void panelGraphStats_Paint(object sender, PaintEventArgs e)
        {
            draw_backbuffer_stats();
            e.Graphics.DrawImageUnscaled(m_back_buffer_stats, Point.Empty);
        }

        private void panelGraphDistribution_Paint(object sender, PaintEventArgs e)
        {
            draw_backbuffer_distribution();
            e.Graphics.DrawImageUnscaled(m_back_buffer_distribution, Point.Empty);
        }

        private void DrawList(RectangleF rec, Graphics gfx, List<double> list, Pen line_pen, Brush point_brush, double min_val, double max_val, bool hilight_increments, bool semilogy)
        {
            if (max_val == min_val)
            {
                if (semilogy && min_val > 0)
                    min_val *= 0.99;
                else
                    min_val -= 1;
            }

            if (semilogy)
            {
                if (min_val <= 0 || max_val <= 0)
                    return;
                min_val = Math.Log(min_val);
                max_val = Math.Log(max_val);
            }

            float height = rec.Height - 20;
            float bottom = rec.Bottom - 10;

            double val0 = semilogy ? Math.Log(list[0]) : list[0];
            float ty0 = (float)((val0 - min_val) / (max_val - min_val));
            float x0 = rec.Left;
            float y0 = bottom - ty0 * height;
            if (hilight_increments)
                gfx.FillEllipse(point_brush, new RectangleF(x0 - 2, y0 - 2, 4, 4));
            for (int g = 1; g <= list.Count - 1; ++g)
            {
                double val1 = semilogy ? Math.Log(list[g]) : list[g];
                float tx1 = ((float)g / (float)list.Count);
                float ty1 = (float)((val1 - min_val) / (max_val - min_val));
                float x1 = rec.Left + tx1 * rec.Width;
                float y1 = bottom - ty1 * height;
                gfx.DrawLine(line_pen, x0, y0, x1, y1);
                if (hilight_increments && val1 > val0)
                    gfx.FillEllipse(point_brush, new RectangleF(x1 - 2, y1 - 2, 4, 4));
                x0 = x1;
                y0 = y1;
                val0 = val1;
            }
        }

        private void FillList(RectangleF rec, Graphics gfx, List<double> list_upper, List<double> list_lower, Brush area_brush, double min_val, double max_val)
        {
            if (max_val == min_val)
                min_val -= 1;

            float height = rec.Height - 20;
            float bottom = rec.Bottom - 10;

            int Nl = list_lower.Count;
            int Nu = list_upper.Count;
            PointF[] polygon = new PointF[Nl + Nu];

            for (int g = 0; g < list_lower.Count; ++g)
            {
                float tx = ((float)g / (float)list_lower.Count);
                float ty = (float)((list_lower[g] - min_val) / (max_val - min_val));
                float x = rec.Left + tx * rec.Width;
                float y = bottom - ty * height;
                polygon[g] = new PointF(x, y);
            }
            for (int g = 0; g < list_upper.Count; ++g)
            {
                float tx = ((float)g / (float)list_upper.Count);
                float ty = (float)((list_upper[g] - min_val) / (max_val - min_val));
                float x = rec.Left + tx * rec.Width;
                float y = bottom - ty * height;
                polygon[Nl + Nu - g - 1] = new PointF(x, y);
            }
            gfx.FillPolygon(area_brush, polygon);
        }

        private void DrawPopFitness(RectangleF rec, Graphics gfx, double min_val, double max_val)
        {
            float draw_width = rec.Width / (float)m_pop_fitness_buckets.Count;
            for (int gb_idx = 0; gb_idx < m_pop_fitness_buckets.Count; ++gb_idx)
            {
                var buckets = m_pop_fitness_buckets[gb_idx];
                float tx = ((float)gb_idx / (float)m_pop_fitness_buckets.Count);
                for (int b_idx = 0; b_idx < buckets.Length; ++b_idx)
                {
                    double bucket = buckets[b_idx];
                    int gray = (int)Math.Round(255.0 * bucket / buckets.Max());
                    //gray = Math.Max(gray, 127);
                    Pen pen = new Pen(Color.FromArgb(gray, gray, gray), 1);
                    gfx.DrawEllipse(pen, new RectangleF(tx * rec.Width, rec.Bottom - b_idx, draw_width, 1));
                }
            }
        }

        private void GraphWindow_SizeChanged(object sender, EventArgs e)
        {
            m_resized = true;
            RefreshGraphs();
            m_resized = false;
        }

        public void Reset()
        {
            //m_stats.Clear();

            best_fitness.Clear();
            best_child_fitness.Clear();
            pop_mu.Clear();
            pop_min.Clear();
            pop_max.Clear();
            pop_sigma.Clear();

            max_best_fitness = Double.NegativeInfinity;
            min_best_fitness = Double.PositiveInfinity;
            max_best_child_fitness = Double.NegativeInfinity;
            min_best_child_fitness = Double.PositiveInfinity;
            max_pop_mu = Double.NegativeInfinity;
            min_pop_mu = Double.PositiveInfinity;
            max_pop_range = Double.NegativeInfinity;
            min_pop_range = Double.PositiveInfinity;
            max_pop_sigma = Double.NegativeInfinity;
            min_pop_sigma = Double.PositiveInfinity;

            RefreshGraphs();
        }

        public void RefreshGraphs()
        {
            panelGraphFitness.Refresh();
            panelGraphStats.Refresh();
            panelGraphDistribution.Refresh();
        }

        private void labelLegendBestFitness_Click(object sender, EventArgs e)
        {
            enable_draw_best_fitness = !enable_draw_best_fitness;
            var font = new Font(labelLegendBestFitness.Font, enable_draw_best_fitness ? FontStyle.Regular : FontStyle.Strikeout);
            labelLegendBestFitness.Font = font;
            panelGraphFitness.Refresh();
        }

        private void labelLegendBestChildFitness_Click(object sender, EventArgs e)
        {
            enable_draw_best_child_fitness = !enable_draw_best_child_fitness;
            var font = new Font(labelLegendBestChildFitness.Font, enable_draw_best_child_fitness ? FontStyle.Regular : FontStyle.Strikeout);
            labelLegendBestChildFitness.Font = font;
            panelGraphFitness.Refresh();
        }

        private void labelLegendMu_Click(object sender, EventArgs e)
        {
            enable_draw_pop_mu = !enable_draw_pop_mu;
            var font = new Font(labelLegendMu.Font, enable_draw_pop_mu ? FontStyle.Regular : FontStyle.Strikeout);
            labelLegendMu.Font = font;
            labelLegendRange.Enabled = enable_draw_pop_mu;
            panelGraphStats.Refresh();
        }

        private void labelLegendRange_Click(object sender, EventArgs e)
        {
            enable_draw_pop_range = !enable_draw_pop_range;
            var font = new Font(labelLegendRange.Font, enable_draw_pop_range ? FontStyle.Regular : FontStyle.Strikeout);
            labelLegendRange.Font = font;
            panelGraphStats.Refresh();
        }

        private void labelLegendSigma_Click(object sender, EventArgs e)
        {
            enable_draw_pop_sigma = !enable_draw_pop_sigma;
            var font = new Font(labelLegendSigma.Font, enable_draw_pop_sigma ? FontStyle.Regular : FontStyle.Strikeout);
            labelLegendSigma.Font = font;
            panelGraphStats.Refresh();
        }

        private void draw_backbuffer_fitness()
        {
            if (m_back_buffer_fitness == null || m_resized)
                m_back_buffer_fitness = new Bitmap(panelGraphFitness.Width, panelGraphFitness.Height);
            Graphics g = Graphics.FromImage(m_back_buffer_fitness);

            g.Clear(Color.White);

            var rec = g.VisibleClipBounds;
            //g.DrawLine(Pens.Black, rec.Left, (rec.Top + rec.Bottom) / 2, rec.Right, (rec.Top + rec.Bottom) / 2);
            if (best_fitness == null || best_fitness.Count < 2)
                return;

            if (enable_draw_best_fitness)
                DrawList(rec, g, best_fitness, Pens.Black, Brushes.Gray, min_best_fitness, max_best_fitness, true, false);
            if (enable_draw_best_child_fitness)
                DrawList(rec, g, best_child_fitness, Pens.Red, Brushes.Transparent, min_best_child_fitness, max_best_child_fitness, false, false);

            g.Dispose();
        }

        private void draw_backbuffer_stats()
        {
            if (m_back_buffer_stats == null || m_resized)
                m_back_buffer_stats = new Bitmap(panelGraphStats.Width, panelGraphStats.Height);
            Graphics g = Graphics.FromImage(m_back_buffer_stats);

            g.Clear(Color.White);

            var rec = g.VisibleClipBounds;
            //g.DrawLine(Pens.Black, rec.Left, (rec.Top + rec.Bottom) / 2, rec.Right, (rec.Top + rec.Bottom) / 2);
            if (pop_mu == null || pop_mu.Count < 2)
                return;

            if (enable_draw_pop_mu)
            {
                double min_val = min_pop_mu;
                double max_val = max_pop_mu;
                if (enable_draw_pop_range)
                {
                    //min_val = Math.Min(min_val, min_pop_range);
                    //max_val = Math.Min(max_val, max_pop_range);
                    min_val = min_pop_range;
                    max_val = max_pop_range;

                    FillList(rec, g, pop_max, pop_mu, Brushes.LightBlue, min_val, max_val);
                    FillList(rec, g, pop_mu, pop_min, Brushes.Pink, min_val, max_val);
                    DrawList(rec, g, pop_max, Pens.CornflowerBlue, Brushes.Transparent, min_val, max_val, false, false);
                    DrawList(rec, g, pop_min, Pens.LightSalmon, Brushes.Transparent, min_val, max_val, false, false);
                }
                DrawList(rec, g, pop_mu, Pens.Green, Brushes.Transparent, min_val, max_val, false, false);
            }
            if (enable_draw_pop_sigma)
                DrawList(rec, g, pop_sigma, Pens.Blue, Brushes.Transparent, min_pop_sigma, max_pop_sigma, false, false);

            //DrawPopFitness(rec, g, min_pop_range, max_pop_range);

            g.Dispose();
        }

        private void draw_backbuffer_distribution()
        {
            if (m_back_buffer_distribution == null || m_resized)
                m_back_buffer_distribution = new Bitmap(panelGraphDistribution.Width, panelGraphDistribution.Height);
            Graphics g = Graphics.FromImage(m_back_buffer_distribution);

            g.Clear(Color.White);

            var rec = g.VisibleClipBounds;

            double x1 = rec.Left;
            double x2 = rec.Right;
            foreach (double t in m_pop_fitness_t_param)
            {
                double y = (1 - t) * rec.Height;
                g.DrawLine(Pens.Black, (int)x1, (int)y, (int)x2, (int)y);
            }

            //DrawPopFitness(rec, g, min_pop_range, max_pop_range);

            g.Dispose();
        }

        private void panelLegendFitness_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                m_legend_fitness_pos_prev = e.Location;
        }

        private void panelLegendFitness_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = panelLegendFitness.Location.X;
                int y = panelLegendFitness.Location.Y;
                int dx = e.Location.X - m_legend_fitness_pos_prev.X;
                int dy = e.Location.Y - m_legend_fitness_pos_prev.Y;
                panelLegendFitness.Location = new Point(x + dx, y + dy);
            }
        }

        private void panelLegendFitness_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int snap_dx = panelLegendFitness.Location.X - m_legend_fitness_pos_orig.X;
                int snap_dy = panelLegendFitness.Location.Y - m_legend_fitness_pos_orig.Y;
                int snap_dist_sq = snap_dx * snap_dx + snap_dy * snap_dy;
                if (snap_dist_sq <= m_legend_snap_distance_sq)
                    panelLegendFitness.Location = m_legend_fitness_pos_orig;
            }
        }

        private void panelLegendStats_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                m_legend_stats_pos_prev = e.Location;
        }

        private void panelLegendStats_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = panelLegendStats.Location.X;
                int y = panelLegendStats.Location.Y;
                int dx = e.Location.X - m_legend_stats_pos_prev.X;
                int dy = e.Location.Y - m_legend_stats_pos_prev.Y;
                panelLegendStats.Location = new Point(x + dx, y + dy);
            }
        }

        private void panelLegendStats_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int snap_dx = panelLegendStats.Location.X - m_legend_stats_pos_orig.X;
                int snap_dy = panelLegendStats.Location.Y - m_legend_stats_pos_orig.Y;
                int snap_dist_sq = snap_dx * snap_dx + snap_dy * snap_dy;
                if (snap_dist_sq <= m_legend_snap_distance_sq)
                    panelLegendStats.Location = m_legend_stats_pos_orig;
            }
        }

    }
}
