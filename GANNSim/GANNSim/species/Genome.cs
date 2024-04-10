using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MathLib.linalg._2d;

namespace GANNSim.species
{
    class Genome
    {
        public Chromosome m_chromosome_neural_weights;
        public Chromosome m_chromosome_neural_alphas;

        float[] m_buckets_weights;
        float[] m_buckets_alphas;
        float m_draw_width, m_draw_height;
        string m_name;

        public void Init(Chromosome chromosome_neural_weights, Chromosome chromosome_neural_alphas)
        {
            m_chromosome_neural_weights = chromosome_neural_weights;
            m_chromosome_neural_alphas = chromosome_neural_alphas;
            calc_draw_data();
            this.IsChild = false;
        }

        public void Init(int num_weights, int num_neurons)
        {
            m_chromosome_neural_weights = new Chromosome(
                num_weights,
                Chromosome.NoiseFunc.UniformPlusMinusOne);
            m_chromosome_neural_alphas = new Chromosome(
                num_neurons,
                Chromosome.NoiseFunc.UniformUnity);
            calc_draw_data();
            this.IsChild = false;
        }

        public void Randomize()
        {
            m_chromosome_neural_weights.Randomize();
            m_chromosome_neural_alphas.Randomize();
            this.calc_draw_data();
        }

        public Genome[] Mate(Genome parnter_genome, int num_crossover_pts, int num_children_per_mating)
        {
            Genome[] child_genomes;
            if (num_children_per_mating <= 0)
                num_children_per_mating = 1;
            child_genomes = new Genome[num_children_per_mating];

            Chromosome[] neural_weights_chromosomes = this.m_chromosome_neural_weights.Mate(parnter_genome.m_chromosome_neural_weights, num_crossover_pts, num_children_per_mating);
            Chromosome[] neural_alphas_chromosomes = this.m_chromosome_neural_alphas.Mate(parnter_genome.m_chromosome_neural_alphas, num_crossover_pts, num_children_per_mating);
            for (int child_idx = 0; child_idx < child_genomes.Length; child_idx++)
            {
                child_genomes[child_idx] = new Genome();
                child_genomes[child_idx].m_chromosome_neural_weights = neural_weights_chromosomes[child_idx];
                child_genomes[child_idx].m_chromosome_neural_alphas = neural_alphas_chromosomes[child_idx];
                child_genomes[child_idx].calc_draw_data();
                child_genomes[child_idx].IsChild = true;
            }
            return child_genomes;
        }

        public void Mutate(float mutation_rate, bool use_additive_noise, float additive_noise_amp)
        {
            m_chromosome_neural_weights.Mutate(mutation_rate, use_additive_noise, additive_noise_amp);
            m_chromosome_neural_alphas.Mutate(mutation_rate, use_additive_noise, additive_noise_amp);
            this.calc_draw_data();
        }

        public bool IsChild
        {
            get;
            set;
        }

        private void calc_draw_data()
        {
            calc_draw_data_DNA();
            calc_draw_data_name();
        }

        private void calc_draw_data_DNA()
        {
            generate_buckets(ref m_buckets_weights, m_chromosome_neural_weights);
            generate_buckets(ref m_buckets_alphas, m_chromosome_neural_alphas);
        }

        private void generate_buckets(ref float[] buckets, Chromosome chromosome)
        {
            m_draw_width = 42.0f;
            m_draw_height = 4.0f;

            int num_basepairs = chromosome.DNA.Length;
            int num_buckets = (int)m_draw_width - 2;

            if (num_basepairs < num_buckets)
            {
                float num_buckets_per_basepair = (float)num_buckets / (float)num_basepairs;

                buckets = new float[num_buckets];

                for (int bucket_idx = 0; bucket_idx < num_buckets; bucket_idx++)
                {
                    buckets[bucket_idx] = chromosome.BasePair((int)((float)bucket_idx / num_buckets_per_basepair));
                    if (chromosome.NoiseFunction == Chromosome.NoiseFunc.UniformPlusMinusOne)
                        buckets[bucket_idx] = (buckets[bucket_idx] + 1.0f) * 0.5f;
                }
            }
            else
            {
                int num_basepairs_per_bucket = num_basepairs / num_buckets;

                buckets = new float[num_buckets];
                for (int bucket_idx = 0; bucket_idx < num_buckets; bucket_idx++)
                {
                    buckets[bucket_idx] = 0.0f;
                    for (int weight_idx = bucket_idx * num_basepairs_per_bucket;
                         weight_idx < (bucket_idx + 1) * num_basepairs_per_bucket;
                         weight_idx++)
                    {
                        buckets[bucket_idx] += chromosome.BasePair(weight_idx);
                    }
                    buckets[bucket_idx] /= num_basepairs_per_bucket;
                    if (chromosome.NoiseFunction == Chromosome.NoiseFunc.UniformPlusMinusOne)
                        buckets[bucket_idx] = (buckets[bucket_idx] + 1.0f) * 0.5f;
                }
            }
        }

        private void calc_draw_data_name()
        {
            float weight_sum = 0.0f;
            foreach (float weight in m_chromosome_neural_weights.DNA)
                weight_sum += weight;
            ulong checksum = (ulong)(Math.Abs(weight_sum) * 1e9f);
            m_name = "";
            while (checksum != 0)
            {
                char letter = (char)(checksum % 26);
                m_name += (char)(letter + 65);
                checksum /= 26;
            }
            if (weight_sum < 0.0f)
                m_name = 'Å' + m_name;
        }

        public void DrawDNA(Graphics g, Vec2 position)
        {
            draw_dna_buckets(g, position, m_buckets_weights, m_chromosome_neural_weights);
            draw_dna_buckets(g, position + new Vec2(0.0f, -m_draw_height), m_buckets_alphas, m_chromosome_neural_alphas);
        }

        private void draw_dna_buckets(Graphics g, Vec2 position, float[] buckets, Chromosome chromosome)
        {
            float half_width = m_draw_width * 0.5f;
            for (int bucket_idx = 0; bucket_idx < buckets.Length; bucket_idx++)
            {
                int col = (int)(255 * buckets[bucket_idx]);
                g.DrawLine(new Pen(Color.FromArgb(col, col, col)),
                    (int)(position.X - half_width + 1.0f + bucket_idx),
                    (int)(Constants.m_canvas_height - position.Y),
                    (int)(position.X - half_width + 1.0f + bucket_idx),
                    (int)(Constants.m_canvas_height - position.Y + m_draw_height));
            }

            //if (chromosome.CrossoverPts != null)
            //{
            //    foreach (int crossover_pt in chromosome.CrossoverPts)
            //    {
            //        float pos = m_draw_width * (float)crossover_pt / (float)chromosome.DNA.Length;
            //        g.DrawLine(Pens.Tomato,
            //            (int)(position.X - half_width + pos),
            //            (int)(Constants.m_canvas_height - position.Y),
            //            (int)(position.X - half_width + pos),
            //            (int)(Constants.m_canvas_height - position.Y + m_draw_height));
            //    }
            //}

            g.DrawRectangle(this.IsChild ? Pens.Red : Pens.Blue,
                (int)(position.X - half_width),
                (int)(Constants.m_canvas_height - position.Y),
                (int)m_draw_width - 1, //? 
                (int)m_draw_height);
        }

        public void DrawCheckSum(Graphics g, int alpha, Vec2 position)
        {
            SizeF size = g.MeasureString(m_name, SystemFonts.DefaultFont);
            SolidBrush guy_brush;
            if (this.IsChild)
                guy_brush = new SolidBrush(Color.FromArgb(alpha, 255, 0, 0));
            else
                guy_brush = new SolidBrush(Color.FromArgb(alpha, 0, 0, 255));
            g.DrawString(m_name, SystemFonts.DefaultFont, guy_brush, 
                position.X - size.Width * 0.5f, 
                Constants.m_canvas_height - position.Y);
        }
    }
}
