using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GANNSim.dynamics;
using MathLib.linalg._2d;

namespace GANNSim.morphology
{
    class Joint
    {
        static int m_global_id_ctr;
        int m_id;
        Vec2 m_orig_pos;
        State2D m_state_curr;
        State2D m_state_prev;
        float m_mass;
        bool m_in_contact;
        float m_contact_force;
        string m_name;

        List<Limb> m_limbs;

        List<AngularSpring> m_springs;

        static Joint()
        {
            m_global_id_ctr = 0;
        }

        public Joint(string name, Vec2 pos, float mass)
        {
            m_id = m_global_id_ctr++;
            m_orig_pos = pos;
            m_state_curr = new State2D(pos, new Vec2(), new Vec2());
            m_state_prev = new State2D(pos, new Vec2(), new Vec2());
            m_mass = mass;
            m_in_contact = false;
            m_contact_force = 0.0f;
            this.HasContactSensor = true;
            this.IsHead = false;
            m_limbs = new List<Limb>();
            m_springs = new List<AngularSpring>();
            m_name = name;
        }

        public void AddLimb(Limb limb)
        {
            m_limbs.Add(limb);
            limb.AddJoint(this);
        }

        public int ID
        {
            get { return m_id; }
            set
            {
                m_id = value;
                m_global_id_ctr = m_id + 1;
            }
        }

        public Vec2 CurrPos
        {
            get
            {
                //if (m_state_curr.P.IsNaN)
                //    Console.WriteLine("Joint ID = {0}", m_id);
                return m_state_curr.P.Copy();
            }
            set { m_state_prev.P = m_state_curr.P = value; }
        }

        public Vec2 CurrVel
        {
            get { return m_state_curr.V.Copy(); }
        }

        public Vec2 CurrAcc
        {
            get { return m_state_curr.A.Copy(); }
            set { m_state_curr.A = value; }
        }

        public Vec2 OrigPos
        {
            get { return m_orig_pos; }
        }

        public float Mass
        {
            get { return m_mass; }
        }

        public bool InContact
        {
            get { return m_in_contact; }
        }

        public void SetNetLinearSpringAcceleration()
        {
            m_state_curr.A.SetZero();
            foreach (Limb limb in m_limbs)
                m_state_curr.A += limb.CalcForce(this) / m_mass;
        }

        public void UpdateDynamics(Integration.Method2D integration_method, float dt, 
                                   Vec2 g, float damping, float contact_stiffness, float mu)
        {
            // Update particle motion.
            m_state_curr.A += g;
            m_state_curr.A += (calc_contact_force(contact_stiffness) - damping * m_state_curr.V) / m_mass;
            integration_method(dt, m_state_curr, m_state_prev);

            if (m_in_contact)
            {
                //m_state_prev.V.Y = m_state_curr.V.Y;
                m_state_curr.V.Y *= 0.5f; // Velocity based damping near surface.
                m_state_prev.V.Y = m_state_curr.V.Y;
                // If near surface, add friction.
                //m_state_prev.V.X = m_state_curr.V.X;
                if (!m_state_curr.V.IsAnyNaN)
                {
                    m_state_curr.V.X -= Math.Sign(m_state_curr.V.X) * Math.Abs(m_state_curr.A.Y) * mu * dt;
                    m_state_prev.V.X = m_state_curr.V.X;
                }
            }
        }

        private Vec2 calc_contact_force(float stiffness)
        {
            float curr_y = CurrPos.Y;
            float ground_y = Constants.m_ground_y;
            if (curr_y < ground_y)
            {
                m_in_contact = true;
                m_contact_force = -stiffness * (curr_y - ground_y);
                return new Vec2(0.0f, m_contact_force);
            }
            else
            {
                m_in_contact = false;
                m_contact_force = 0.0f;
                return new Vec2();
            }
        }

        public float ContactForce
        {
            get { return m_contact_force; }
        }

        public bool HasContactSensor
        {
            get;
            set;
        }

        public bool IsHead
        {
            get;
            set;
        }

        public void DebugDraw(System.Drawing.Graphics g, int alpha)
        {
            float y_max = Constants.m_canvas_height;
            System.Drawing.Pen acc_pen = new System.Drawing.Pen(
                System.Drawing.Color.FromArgb(alpha, System.Drawing.Color.Blue));
            g.DrawLine(acc_pen, 
                CurrPos.ToPointF(y_max), 
                (CurrPos + CurrAcc).ToPointF(y_max));
        }
    }
}
