using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GANNSim.dynamics
{
    class Spring
    {
        protected SpringMaterial m_material;
        protected State1D m_state_curr;
        protected State1D m_state_prev;

        public Spring(SpringMaterial material)
        {
            m_material = material;
            m_state_curr = new State1D(material.RestDistance, 0.0f, 0.0f);
            m_state_prev = new State1D(material.RestDistance, 0.0f, 0.0f);
        }

        public float CalcSpringForce()
        {
            return -m_material.Stiffness * (m_state_curr.P - m_material.RestDistance)
                   -m_material.Damping * m_state_curr.V;
        }

        public void Update(Integration.Method1D integration_method, float dt, float a_net)
        {
            //m_state_prev.A = m_state_curr.A;
            m_state_curr.A = a_net;
            integration_method(dt, m_state_curr, m_state_prev);
        }

        public SpringMaterial Material
        {
            get { return m_material; }
        }

        public State1D CurrState
        {
            get { return m_state_curr; }
        }

        public void SetPos(float pos)
        {
            m_state_prev.P = pos;//m_state_curr.P;
            m_state_curr.P = pos;
        }

        public void SetVel(float vel)
        {
            m_state_prev.V = vel;
            m_state_curr.V = vel;
        }
    }
}
