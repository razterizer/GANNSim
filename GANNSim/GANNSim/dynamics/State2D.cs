using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLib.linalg._2d;

namespace GANNSim.dynamics
{
    class State2D
    {
        public State2D(Vec2 distance, Vec2 velocity, Vec2 acceleration)
        {
            this.P = distance;
            this.V = velocity;
            this.A = acceleration;
        }

        public void CopyFrom(State2D state)
        {
            this.P = state.P.Copy();
            this.V = state.V.Copy();
            this.A = state.A.Copy();
        }

        public Vec2 P
        {
            get;
            set;
        }

        public Vec2 V
        {
            get;
            set;
        }

        public Vec2 A
        {
            get;
            set;
        }
    }
}
