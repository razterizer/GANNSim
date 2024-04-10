using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GANNSim.species;

namespace GANNSim.neural
{
    class Brain
    {
        public struct HiddenLayerData
        {
            public int num_neurons;
            public bool recurrent;
            
            public HiddenLayerData(int num_neurons_, bool recurrent_)
            {
                num_neurons = num_neurons_;
                recurrent = recurrent_;
            }
        }

        List<Neuron> m_neurons_all;
        List<Neuron> m_neurons_input_layer;
        List<List<Neuron>> m_neurons_hidden_layers;
        List<Neuron> m_neurons_output_layer;

        int m_num_inputs;
        HiddenLayerData[] m_hidden_layer_data;
        int m_num_outputs;

        public Brain(IEnumerable<Input> inputs, HiddenLayerData[] hidden_layer_data, int num_outputs)
        {
            m_num_inputs = inputs.Count();
            this.Inputs = new List<Input>();
            foreach (Input i in inputs)
                this.Inputs.Add(i);

            m_hidden_layer_data = hidden_layer_data;
            m_num_outputs = num_outputs;

            m_neurons_all = new List<Neuron>();
            m_neurons_input_layer = new List<Neuron>();
            m_neurons_hidden_layers = new List<List<Neuron>>();
            m_neurons_output_layer = new List<Neuron>();
        }

        public void Build(Chromosome chromosome_weights, Chromosome chromosome_alphas)
        {
            create_neurons();

            connect_neurons();

            set_neural_data(chromosome_weights, chromosome_alphas);
        }

        private void create_neurons()
        {
            Neuron neuron;
            foreach (Input input in this.Inputs)
            {
                neuron = new Neuron();
                neuron.AddInput(input);
                m_neurons_all.Add(neuron);
                m_neurons_input_layer.Add(neuron);
            }

            for (int hidden_layer_idx = 0; hidden_layer_idx < m_hidden_layer_data.Length; hidden_layer_idx++)
            {
                List<Neuron> hidden_layer = new List<Neuron>();
                m_neurons_hidden_layers.Add(hidden_layer);
                for (int hidden_neuron_idx = 0; hidden_neuron_idx < m_hidden_layer_data[hidden_layer_idx].num_neurons; hidden_neuron_idx++)
                {
                    neuron = new Neuron();
                    hidden_layer.Add(neuron);
                    m_neurons_all.Add(neuron);
                }
            }

            for (int output_neuron_idx = 0; output_neuron_idx < m_num_outputs; output_neuron_idx++)
            {
                neuron = new Neuron();
                m_neurons_output_layer.Add(neuron);
                m_neurons_all.Add(neuron);
            }
        }

        private void connect_neurons()
        {
            // Connect neurons.
            if (m_neurons_hidden_layers.Count > 0)
            {
                foreach (Neuron nHA in m_neurons_hidden_layers[0])
                {
                    foreach (Neuron nI in m_neurons_input_layer)
                        nHA.AddNeuron(nI);
                    if (m_hidden_layer_data[0].recurrent)
                        foreach (Neuron nHB in m_neurons_hidden_layers[0])
                            nHA.AddNeuron(nHB);
                }
            }

            for (int hidden_layer_idx = 1; hidden_layer_idx < m_neurons_hidden_layers.Count; hidden_layer_idx++)
            {
                foreach (Neuron nHA in m_neurons_hidden_layers[hidden_layer_idx])
                {
                    foreach (Neuron nHB in m_neurons_hidden_layers[hidden_layer_idx - 1])
                        nHA.AddNeuron(nHB);
                    if (m_hidden_layer_data[hidden_layer_idx].recurrent)
                        foreach (Neuron nHB in m_neurons_hidden_layers[hidden_layer_idx])
                            nHA.AddNeuron(nHB);
                }
            }

            if (m_neurons_hidden_layers.Count > 0)
            {
                foreach (Neuron nO in m_neurons_output_layer)
                    foreach (Neuron nH in m_neurons_hidden_layers.Last())
                        nO.AddNeuron(nH);
            }
            else
            {
                foreach (Neuron nO in m_neurons_output_layer)
                    foreach (Neuron nI in m_neurons_input_layer)
                        nO.AddNeuron(nI);
            }
        }

        private void set_neural_data(Chromosome chromosome_weights, Chromosome chromosome_alphas)
        {
            // Set synapse weights
            int weight_start_idx = 0;
            int weight_length;
            int alpha_start_idx = 0;
            foreach (Neuron n in m_neurons_all)
            {
                weight_length = n.NumWeights;
                n.SetWeights(chromosome_weights.GetDNASubset(weight_start_idx, weight_length));
                weight_start_idx += weight_length;

                n.SetAlpha(chromosome_alphas.DNA[alpha_start_idx++]);
            }
            //Console.WriteLine("weight_start_idx = {0}, chromosome_weights.NumBasePairs = {1}", weight_start_idx, chromosome_weights.NumBasePairs);
            if (weight_start_idx != chromosome_weights.NumBasePairs)
                System.Windows.Forms.MessageBox.Show("Mismatch in weight basepairs!");
            //Console.WriteLine("alpha_start_idx = {0}, chromosome_alphas.NumBasePairs = {1}", alpha_start_idx, chromosome_alphas.NumBasePairs);
            if (alpha_start_idx != chromosome_alphas.NumBasePairs)
                System.Windows.Forms.MessageBox.Show("Mismatch in alpha basepairs!");
        }

        public void Update()
        {
            foreach (Neuron n in m_neurons_all)
                n.Update();
        }

        public List<Input> Inputs
        {
            get;
            set;
        }

        public List<Neuron> OutputNeurons
        {
            get { return m_neurons_output_layer; }
        }

        public void Clear()
        {
            foreach (Neuron n in m_neurons_all)
                n.Reset();
            m_neurons_all.Clear();
            m_neurons_input_layer.Clear();
            foreach (List<Neuron> layer in m_neurons_hidden_layers)
                layer.Clear();
            m_neurons_hidden_layers.Clear();
            m_neurons_output_layer.Clear();
            foreach (Input i in this.Inputs)
                i.Clear();
            //m_num_hidden_layer_neurons.Clear();
        }

        public int NumInputs
        {
            get { return m_num_inputs; }
        }

        public int NumOutputs
        {
            get { return m_num_outputs;}
        }

        public int NumNeurons
        {
            get
            {
                int num_neurons = m_num_inputs + m_num_outputs;
                foreach (HiddenLayerData hld in m_hidden_layer_data)
                    num_neurons += hld.num_neurons;
                return num_neurons;
            }
        }

        public int NumLayers
        {
            get { return 2 + m_neurons_hidden_layers.Count; }
        }


        public int NumWeights
        {
            get
            {
                // Signals -> Input Layer
                int num_weights = m_num_inputs * 2; // 1 signal + 1 bias

                if (m_hidden_layer_data.Length > 0)
                {
                    // Input Layer -> First Hidden Layer
                    if (m_hidden_layer_data[0].recurrent)
                        num_weights += (m_num_inputs + m_hidden_layer_data[0].num_neurons + 1) *
                            m_hidden_layer_data[0].num_neurons;
                    else
                        num_weights += (m_num_inputs + 1) * m_hidden_layer_data[0].num_neurons;

                    for (int hidden_layer_idx = 1; hidden_layer_idx < m_hidden_layer_data.Length; hidden_layer_idx++)
                    {
                        // Hidden Layer N - 1 -> Hidden Layer N
                        if (m_hidden_layer_data[hidden_layer_idx].recurrent)
                            num_weights += (m_hidden_layer_data[hidden_layer_idx - 1].num_neurons + m_hidden_layer_data[hidden_layer_idx].num_neurons + 1) *
                                m_hidden_layer_data[hidden_layer_idx].num_neurons;
                        else
                            num_weights += (m_hidden_layer_data[hidden_layer_idx - 1].num_neurons + 1) * m_hidden_layer_data[hidden_layer_idx].num_neurons;
                    }

                    // Last Hidden Layer -> Output Layer
                    num_weights += (m_hidden_layer_data.Last().num_neurons + 1) * m_num_outputs;
                }
                else
                {
                    // Input Layer -> Output Layer
                    num_weights += (m_num_inputs + 1) * m_num_outputs;
                }

                return num_weights;
            }
        }
    }
}
