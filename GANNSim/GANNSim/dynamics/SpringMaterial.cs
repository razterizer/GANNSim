using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GANNSim.dynamics
{
    class SpringMaterial
    {
        float m_rest_dist_init;
        float m_stiffness;
        float m_damping;

        public SpringMaterial(float rest_dist, float stiffness, float damping)
        {
            this.RestDistance = rest_dist;
            m_rest_dist_init = rest_dist;
            m_stiffness = stiffness;
            m_damping = damping;
        }

        public void ResetRestDistance()
        {
            this.RestDistance = m_rest_dist_init;
        }

        public float RestDistance
        {
            get;
            set;
        }

        public float RestDistanceInit
        {
            get { return m_rest_dist_init; }
        }

        public float Stiffness
        {
            get { return m_stiffness; }
        }

        public float Damping
        {
            get { return m_damping; }
        }
    }
}
