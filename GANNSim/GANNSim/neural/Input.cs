using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GANNSim.neural
{
    class Input
    {
        string m_name;
        int m_id;

        public Input(string name, int id)
        {
            m_name = name;
            m_id = id;
            this.Signal = 0.0f;
        }

        public string Name
        {
            get { return m_name; }
        }

        public int ID
        {
            get { return m_id; }
        }

        public float Signal
        {
            get;
            set;
        }

        public void Clear()
        {
            this.Signal = 0.0f;
        }
    }
}
