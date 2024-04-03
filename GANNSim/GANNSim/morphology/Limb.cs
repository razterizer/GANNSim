using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GANNSim.dynamics;
using MathLib.linalg._2d;

namespace GANNSim.morphology
{
    class Limb
    {
        #region DATA
        static int m_global_id_ctr;
        int m_id;
        int m_curr_joint_add_idx;
        Vec2 F_trans;
        Joint[] m_joints;
        Spring m_spring;
        string m_name;
        #endregion

        #region CONSTRUCTORS
        static Limb()
        {
            m_global_id_ctr = 0;
        }

        public Limb(string name)
        {
            m_id = m_global_id_ctr++;
            m_joints = new Joint[2];
            m_spring = null;
            m_curr_joint_add_idx = 0;
            this.IsMuscle = false;
            F_trans = new Vec2();
            m_name = name;
        }
        #endregion

        #region PROPERTIES
        public int ID
        {
            get { return m_id; }
            set
            {
                m_id = value;
                m_global_id_ctr = m_id + 1;
            }
        }

        public Joint Joint_A
        {
            get { return m_joints[0]; }
        }

        public Joint Joint_B
        {
            get { return m_joints[1]; }
        }

        public float Length
        {
            get { return (float)Math.Abs(m_spring.CurrState.P); }
        }

        public float LengthSquared
        {
            get { return Vec2.DistanceSquared(Joint_B.CurrPos, Joint_A.CurrPos); }
        }

        public float Velocity
        {
            get { return Math.Abs(m_spring.CurrState.V); }
        }

        public Vec2 Dir
        {
            get { return Joint_B.CurrPos - Joint_A.CurrPos; }
        }

        public Vec2 NormalizedDir
        {
            get { return this.Dir.GetNormalized(); }
        }

        public bool IsMuscle { get; set; }

        public float MuscleGain { get; set; }
        #endregion

        #region METHODS
        // Only to be used in Joint.
        public void AddJoint(Joint joint)
        {
            m_joints[m_curr_joint_add_idx++] = joint;
        }

        public void SetLinearSpring(float stiffness, float damping)
        {
            if (m_curr_joint_add_idx < 2)
                throw new IndexOutOfRangeException("Limb must have exactly 2 joints available.");
            SpringMaterial spring_material = new SpringMaterial(
                Vec2.Distance(Joint_A.CurrPos, Joint_B.CurrPos),
                stiffness,
                damping);

            m_spring = new Spring(spring_material);
        }

        public void SetLengthOffset(float offset, float threshold)
        {
            if (Math.Abs(offset) > threshold)
            {
                offset -= threshold;
                float curr_len = this.Length;
                float rest_len = m_spring.Material.RestDistance;
                if ((offset > 0.0f && rest_len + offset > curr_len) ||
                    (offset < 0.0f && rest_len + offset < curr_len))
                {
                    //Joint_B.CurrPos = Joint_A.CurrPos + this.NormalizedDir * (rest_len + offset);
                    m_spring.Material.RestDistance = m_spring.Material.RestDistanceInit + offset;
                }
            }
        }

        public void AddTransientForce(float F_ext)
        {
            F_trans += F_ext * NormalizedDir;
        }

        public Vec2 CalcForce(Joint joint)
        {
            Vec2 F = m_spring.CalcSpringForce() * NormalizedDir + F_trans;
            F_trans.SetZero();
            if (joint == Joint_B)
                return +F;
            else
                return -F;
        }

        public void UpdateSpring(Integration.Method1D integration_method, float dt)
        {
            //?
            m_spring.SetPos(Vec2.Distance(m_joints[1].CurrPos, m_joints[0].CurrPos));
            m_spring.SetVel(Vec2.Dot(NormalizedDir, m_joints[1].CurrVel - m_joints[0].CurrVel));
            
            m_spring.Update(integration_method,
                dt,
                Vec2.Dot(Joint_B.CurrAcc - Joint_A.CurrAcc, NormalizedDir));
        }
        
        public override string ToString()
        {
            return "Limb " + ID;
        }
        #endregion
    }
}
