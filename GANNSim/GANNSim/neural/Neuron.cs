using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GANNSim.neural
{
    class Neuron
    {
        float m_alpha;
        float m_bias_weight;
        Dictionary<Input, float> m_input_weights;
        Dictionary<Neuron, float> m_neuron_weights;
        float m_output_curr;
        float m_output_prev;
        static Random m_random;

        static Neuron()
        {
            m_random = new Random((int)DateTime.Now.Ticks);
        }

        public Neuron()
        {
            m_input_weights = new Dictionary<Input, float>();
            m_neuron_weights = new Dictionary<Neuron, float>();

            m_alpha = 0.0f;// (float)m_random.NextDouble();
            m_bias_weight = 0.0f;// weight_rand();
            m_output_curr = 0.0f;
            m_output_prev = 0.0f;
        }

        private float weight_rand()
        {
            return (float)(2.0 * m_random.NextDouble() - 1.0);
        }

        public void AddInput(Input input)
        {
            m_input_weights.Add(input, 0.0f); //weight_rand());
        }

        public void AddInputs(ICollection<Input> inputs)
        {
            foreach (Input i in inputs)
                m_input_weights.Add(i, 0.0f);//weight_rand());
        }

        public void AddNeuron(Neuron neuron)
        {
            m_neuron_weights.Add(neuron, 0.0f);//weight_rand());
        }

        public int NumWeights
        {
            get { return 1 + m_input_weights.Count + m_neuron_weights.Count; }
        }

        public void SetWeights(float[] weights)
        {
            int ctr = 0;
            m_bias_weight = weights[ctr++];
            Input[] input_keys = m_input_weights.Keys.ToArray();
            Neuron[] neuron_keys = m_neuron_weights.Keys.ToArray();
            foreach (Input input in input_keys)
                m_input_weights[input] = weights[ctr++];
            foreach (Neuron neuron in neuron_keys)
                m_neuron_weights[neuron] = weights[ctr++];
        }

        public void SetAlpha(float alpha)
        {
            m_alpha = alpha;
        }

        private float calc_sum()
        {
            float y = m_bias_weight;
            float w;
            float x;
            foreach (KeyValuePair<Input, float> iw_pair in m_input_weights)
            {
                w = iw_pair.Value;
                x = iw_pair.Key.Signal;
                y += w * x;
            }
            foreach (KeyValuePair<Neuron, float> iw_pair in m_neuron_weights)
            {
                w = iw_pair.Value;
                x = iw_pair.Key.OutputPrev;
                y += w * x;
            }
            return y;
        }

        private float sigmoid(float y, float alpha)
        {
            return (float)Math.Atan(alpha * y);
        }

        public void Update()
        {
            m_output_prev = m_output_curr;
            float y = calc_sum();
            m_output_curr = sigmoid(y, m_alpha);
        }

        public float OutputCurr
        {
            get { return m_output_curr; }
        }
        public float OutputPrev
        {
            get { return m_output_prev; }
        }

        public void Reset()
        {
            m_input_weights.Clear();
            m_neuron_weights.Clear();
        }
    }
}
