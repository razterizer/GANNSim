using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GANNSim.dynamics
{
    class State1D
    {
        public State1D(float distance, float velocity, float acceleration)
        {
            this.P = distance;
            this.V = velocity;
            this.A = acceleration;
        }

        public void CopyFrom(State1D state)
        {
            this.P = state.P;
            this.V = state.V;
            this.A = state.A;
        }

        public float P
        {
            get;
            set;
        }

        public float V
        {
            get;
            set;
        }

        public float A
        {
            get;
            set;
        }
    }
}
