using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLib.utils;

namespace GANNSim.species
{
    class Chromosome
    {
        struct CrossoverSegment
        {
            public CrossoverSegment(int start_idx_, int end_idx_)
            {
                start_idx = start_idx_;
                end_idx = end_idx_;
            }
            public int start_idx;
            public int end_idx;
        }

        float[] m_dna;
        static Random m_random;
        List<int> m_crossover_pts;
        List<CrossoverSegment> m_crossover_segments;

        public enum NoiseFunc { UniformUnity, UniformPlusMinusOne }
        NoiseFunc m_noise_func;

        static Chromosome()
        {
            m_random = new Random((int)DateTime.Now.Ticks);
        }

        public Chromosome(int num_basepairs, NoiseFunc noise_func)
        {
            m_noise_func = noise_func;
            m_dna = new float[num_basepairs];
            for (int basepair_idx = 0; basepair_idx < num_basepairs; basepair_idx++)
                m_dna[basepair_idx] = noise();
            m_crossover_pts = new List<int>();
            m_crossover_segments = new List<CrossoverSegment>();
        }

        public Chromosome(float[] newdna, NoiseFunc noise_func)
        {
            m_noise_func = noise_func;
            m_dna = (float[])newdna.Clone();
            m_crossover_pts = new List<int>();
            m_crossover_segments = new List<CrossoverSegment>();
        }

        public void Randomize()
        {
            for (int basepair_idx = 0; basepair_idx < m_dna.Length; basepair_idx++)
                m_dna[basepair_idx] = noise();
        }

        public float BasePair(int idx)
        {
            if (m_dna.Length == 0)
                return float.NaN;
            return m_dna[idx];
        }

        private float noise()
        {
            switch (m_noise_func)
            {
                case NoiseFunc.UniformUnity:
                    return (float)m_random.NextDouble();
                case NoiseFunc.UniformPlusMinusOne:
                    return pm1_random();
                default:
                    return float.NaN;
            }
        }

        private float pm1_random()
        {
            double rand_pm1 = (2.0 * m_random.NextDouble() - 1.0);
            return (float)rand_pm1;
        }

        // Crossover
        public Chromosome[] Mate(Chromosome partner, int num_crossover_pts, bool two_children)
        {
            num_crossover_pts = Math.Min(num_crossover_pts, this.m_dna.Length - 1);
            m_crossover_pts.Clear();
            // Add crossover points.
            for (int pt_idx = 0; pt_idx < num_crossover_pts; pt_idx++)
            {
                bool pt_exists;
                m_crossover_pts.Add(-1);
                do
                {
                    pt_exists = false;
                    m_crossover_pts[pt_idx] = m_random.Next(m_dna.Length - 1); // pt_idx_max = idx_max - 1
                    for (int test_pt_idx = 0; test_pt_idx < pt_idx; test_pt_idx++)
                    {
                        if (m_crossover_pts[pt_idx] == m_crossover_pts[test_pt_idx])
                        {
                            pt_exists = true;
                            break;
                        }
                    }
                } while (pt_exists);
            }
            //m_crossover_pts.Add(this.m_dna.Length - 1); // Add the end of dna so we get complete intervals to loop over.
            //num_crossover_pts++;
            m_crossover_pts.Sort();

            int start_idx = 0;
            CrossoverSegment xo_seg;
            foreach (int xo_pt in m_crossover_pts)
            {
                xo_seg = new CrossoverSegment(start_idx, xo_pt);
                start_idx = xo_pt + 1;
                m_crossover_segments.Add(xo_seg);
            }
            if (start_idx < this.m_dna.Length - 1)
            {
                xo_seg = new CrossoverSegment(start_idx, this.m_dna.Length - 1);
                m_crossover_segments.Add(xo_seg);
            }

            Chromosome[] children;
            if (two_children)
                children = new Chromosome[2];
            else
                children = new Chromosome[1];

            for (int child_idx = 0; child_idx < children.Length; child_idx++)
            {
                // Crossover at each and other crossover segment.
                float[] child_dna = new float[this.m_dna.Length];
                for (int crossoverseg_idx = 0; crossoverseg_idx < m_crossover_segments.Count; crossoverseg_idx++)
                {
                    CrossoverSegment crossover_seg = m_crossover_segments[crossoverseg_idx];
                    if (crossoverseg_idx % 2 == child_idx) // Nifty way of selecting the complement DNA for the second child.
                    {
                        for (int basepair_idx = crossover_seg.start_idx; basepair_idx <= crossover_seg.end_idx; basepair_idx++)
                        {
                            child_dna[basepair_idx] = this.m_dna[basepair_idx];
                        }
                    }
                    else
                    {
                        for (int basepair_idx = crossover_seg.start_idx; basepair_idx <= crossover_seg.end_idx; basepair_idx++)
                        {
                            child_dna[basepair_idx] = partner.m_dna[basepair_idx];
                        }
                    }
                }

                // make a new DNA object
                children[child_idx] = new Chromosome(child_dna, m_noise_func); // Assume the partner has the same noise-func.
            }
            return children;

            // Crossover at each and other crossover point.
            //float[] child_dna = new float[this.m_dna.Length];
            //for (int crossover_idx = 0; crossover_idx < num_crossover_pts; crossover_idx++)
            //{
            //    float[] parent_dna = null;
            //    int index_start = 0, index_end = 0;
            //    if (crossover_idx % 2 == 0)
            //    {
            //        if (crossover_idx > 0)
            //            index_start = m_crossover_pts[crossover_idx - 1] + 1;
            //        index_end = m_crossover_pts[crossover_idx];
            //        parent_dna = this.m_dna;
            //    }
            //    else
            //    {
            //        index_start = m_crossover_pts[crossover_idx - 1] + 1;
            //        index_end = m_crossover_pts[crossover_idx];
            //        parent_dna = partner.m_dna;
            //    }
            //    for (int basepair_idx = index_start; basepair_idx <= index_end; basepair_idx++)
            //    {
            //        child_dna[basepair_idx] = parent_dna[basepair_idx];
            //    }
            //}

            // make a new DNA object
            //Chromosome newdna = new Chromosome(child_dna, m_noise_func); // Assume the partner has the same noise-func.
            //return newdna;
        }

        // Based on a mutation probability, picks a new random float
        public void Mutate(float m, bool use_additive_noise, float additive_noise_amp)
        {
            if (use_additive_noise)
            {
                for (int i = 0; i < m_dna.Length; i++)
                {
                    if (m_random.NextDouble() < m)
                    {
                        m_dna[i] += additive_noise_amp * pm1_random();

                        switch (m_noise_func)
                        {
                            case NoiseFunc.UniformUnity:
                                UtilsFloat.ClampSet(ref m_dna[i], 0.0f, 1.0f);
                                break;
                            case NoiseFunc.UniformPlusMinusOne:
                                UtilsFloat.ClampSet(ref m_dna[i], -1.0f, +1.0f);
                                break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_dna.Length; i++)
                {
                    if (m_random.NextDouble() < m)
                    {
                        m_dna[i] = noise();
                    }
                }
            }
        }

        public int NumBasePairs
        {
            get { return m_dna.Length; }
        }

        public float[] DNA
        {
            get { return m_dna; }
        }

        public float[] GetDNASubset(int start_idx, int length)
        {
            float[] dna_subset = new float[length];
            for (int basepair_idx = 0; basepair_idx < length; basepair_idx++)
                dna_subset[basepair_idx] = m_dna[start_idx + basepair_idx];
            return dna_subset;
        }

        public int[] CrossoverPts
        {
            get
            {
                if (m_crossover_pts == null)
                    return null;
                return m_crossover_pts.ToArray();
            }
        }

        public NoiseFunc NoiseFunction
        {
            get { return m_noise_func; }
        }
    }
}
