using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MathLib.linalg._2d;

namespace GANNSim.dynamics
{
    class Integration
    {
        public delegate void Method1D(float dt, State1D state_curr, State1D state_prev);
        public delegate void Method2D(float dt, State2D state_curr, State2D state_prev);
        
        
        public static void ForwardEuler(float dt, State1D state_curr, State1D state_prev)
        {
            state_curr.P = state_prev.P + state_prev.V * dt;
            state_curr.V = state_prev.V + state_curr.A * dt;

            state_prev.CopyFrom(state_curr);
        }
        public static void ForwardEuler(float dt, State2D state_curr, State2D state_prev)
        {
            state_curr.P = state_prev.P + state_prev.V * dt;
            state_curr.V = state_prev.V + state_curr.A * dt;

            state_prev.CopyFrom(state_curr);
        }

        // Start with a semi-implicit Euler. Then try leap-frog and then higher order methods.
        // In order to preserve as much energy as possible.
        // A_net is the net acceleration on the spring (displacement).
        public static void SemiImplicitEuler(float dt, State1D state_curr, State1D state_prev)
        {
            state_curr.V = state_prev.V + state_curr.A * dt;
            state_curr.P = state_prev.P + state_curr.V * dt;

            state_prev.CopyFrom(state_curr);
        }
        public static void SemiImplicitEuler(float dt, State2D state_curr, State2D state_prev)
        {
            state_curr.V = state_prev.V + state_curr.A * dt;
            state_curr.P = state_prev.P + state_curr.V * dt;

            state_prev.CopyFrom(state_curr);
        }

        public static void VelocityVerlet(float dt, State1D state_curr, State1D state_prev)
        {
            // ?
            //state_curr.P = state_prev.P + state_prev.V * dt + 0.5f * state_curr.A * dt * dt;
            //state_curr.V = state_prev.V + 0.5f * (state_prev.A + state_curr.A) * dt;

            //state_prev.CopyFrom(state_curr);

            float v_n_plus_half = state_prev.V + 0.5f * dt * state_prev.A;
            state_curr.P = state_prev.P + dt * v_n_plus_half;
            state_curr.V = v_n_plus_half + 0.5f * dt * state_curr.A;

            state_prev.CopyFrom(state_curr);
        }
        public static void VelocityVerlet(float dt, State2D state_curr, State2D state_prev)
        {
            // ?
            //state_curr.P = state_prev.P + state_prev.V * dt + 0.5f * state_curr.A * dt * dt;
            //state_curr.V = state_prev.V + 0.5f * (state_prev.A + state_curr.A) * dt;

            //state_prev.CopyFrom(state_curr);

            Vec2 v_n_plus_half = state_prev.V + 0.5f * dt * state_prev.A;
            state_curr.P = state_prev.P + dt * v_n_plus_half;
            state_curr.V = v_n_plus_half + 0.5f * dt * state_curr.A;

            state_prev.CopyFrom(state_curr);
        }
    }
}
