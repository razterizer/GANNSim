using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GANNSim.dynamics;
using MathLib.linalg._2d;
using MathLib.linalg._3d;
using MathLib.linalg.common;
using MathLib.utils;

namespace GANNSim.morphology
{
    class AngularSpring
    {
        #region DATA
        static int m_global_id_ctr;
        int m_id;
        Joint m_joint;
        Limb[] m_limbs;
        Spring m_spring; // TODO: Update spring. ?
        float m_curr_angle, m_prev_angle;
        Vec2 m_F_A, m_F_B, m_F_0;
        #endregion

        #region CONSTRUCTORS
        static AngularSpring()
        {
            m_global_id_ctr = 0;
        }

        public AngularSpring(Joint joint, Limb limb_A, Limb limb_B, float stiffness, float damping)
        {
            m_id = m_global_id_ctr++;
            m_joint = joint;
            m_limbs = new Limb[2];

            float rot_dir;
            float ang = CalcLimbAngle(limb_A, limb_B, out rot_dir);
            if (rot_dir < 0.0f)
            {
                ang = (float)(2.0 * Math.PI - ang);
                m_limbs[0] = limb_B;
                m_limbs[1] = limb_A;
            }
            else
            {
                m_limbs[0] = limb_A;
                m_limbs[1] = limb_B;
            }
            SpringMaterial spring_material = new SpringMaterial(
                ang, //(float)(Math.PI / 180.0 * 90.0), 
                stiffness,
                damping);

            m_spring = new Spring(spring_material);
            m_curr_angle = m_prev_angle = ang;
            m_F_0 = new Vec2();
            m_F_A = new Vec2();
            m_F_B = new Vec2();
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

        public Joint Joint
        {
            get { return m_joint; }
        }

        public Limb LimbA
        {
            get { return m_limbs[0]; }
        }

        public Limb LimbB
        {
            get { return m_limbs[1]; }
        }

        public bool IsMuscle { get; set; }

        public float MuscleGain { get; set; }
        #endregion

        #region METHODS
        private static float CalcLimbAngle(Limb limb_A, Limb limb_B, out float rot_dir)
        {
            rot_dir = float.NaN;
            Vec2 p0 = null; // joint
            Vec2 pA = null;
            Vec2 pB = null;
            if (limb_A.Joint_A == limb_B.Joint_A)
            {
                p0 = limb_A.Joint_A.CurrPos;
                pA = limb_A.Joint_B.CurrPos;
                pB = limb_B.Joint_B.CurrPos;
            }
            else if (limb_A.Joint_A == limb_B.Joint_B)
            {
                p0 = limb_A.Joint_A.CurrPos;
                pA = limb_A.Joint_B.CurrPos;
                pB = limb_B.Joint_A.CurrPos;
            }
            else if (limb_A.Joint_B == limb_B.Joint_A)
            {
                p0 = limb_A.Joint_B.CurrPos;
                pA = limb_A.Joint_A.CurrPos;
                pB = limb_B.Joint_B.CurrPos;
            }
            else if (limb_A.Joint_B == limb_B.Joint_B)
            {
                p0 = limb_A.Joint_B.CurrPos;
                pA = limb_A.Joint_A.CurrPos;
                pB = limb_B.Joint_A.CurrPos;
            }
            if (p0 == null)
                return float.NaN; // Error case.
            else
            {
                Vec2 dir_A = (pA - p0).GetNormalized();
                Vec2 dir_B = (pB - p0).GetNormalized();

                // Only considers the angle along the projection ("x-axis").
                float proj = Vec2.Dot(dir_A, dir_B);
                UtilsFloat.ClampSet(ref proj, -1.0f, 1.0f);
                float ang = (float)Math.Acos(proj);

                //Console.WriteLine("acos = {0}", 180.0f / Math.PI * ang);

                // Only considers the rotational component ("y-axis").
                rot_dir = Vec2.RotDir(dir_A, dir_B);

                //Console.WriteLine("rot_dir = {0}", rot_dir);

                // Decide which half-plane along y (across the x-axis) where the smallest angle is.
                // If Limb B has a negative y-component when Limb A is parallel with the x-axis, 
                // then we continue the rotation with an increasing angle.
                if (rot_dir < 0)
                    ang = (float)(2.0 * Math.PI - ang);

                //Console.WriteLine("angle = {0}", 180.0f / Math.PI * ang);
                //Console.WriteLine("-------------------");

                //int apa = 0;
                //if (float.IsNaN(ang))
                //{
                //    apa = 1;
                //}

                return ang;
            }
        }

        public float CalcSpringAngle()
        {
            float rot_dir;
            return CalcLimbAngle(LimbA, LimbB, out rot_dir);
        }

        public float CalcSpringAngularVelocity()
        {
            Joint joint_A = this.LimbA.Joint_A == this.Joint ? this.LimbA.Joint_B : this.LimbA.Joint_A;
            Joint joint_B = this.LimbB.Joint_A == this.Joint ? this.LimbB.Joint_B : this.LimbB.Joint_A;

            Vec2 r_A = joint_A.CurrPos - this.Joint.CurrPos;
            Vec2 r_B = joint_B.CurrPos - this.Joint.CurrPos;
            Vec3 r_z = Vec2.Cross(r_A, r_B);
            if (r_z.Z < 0.0f)
                r_z.Z = -1.0f;
            else
                r_z.Z = 1.0f;
            Vec3 perp_A = Vec3.Cross(r_A, r_z);
            Vec3 perp_B = Vec3.Cross(r_z, r_B);
            Vec2 u_A = new Vec2(perp_A / perp_A.Length / r_A.Length);
            Vec2 u_B = new Vec2(perp_B / perp_B.Length / r_B.Length);
            Vec2 u_0 = -(u_A + u_B);
            float dang_dt =
                Vec2.Dot(joint_A.CurrVel, u_A) +
                Vec2.Dot(this.Joint.CurrVel, u_0) +
                Vec2.Dot(joint_B.CurrVel, u_B);
            return dang_dt;
        }

        public void SetSpringAngleOffset(float offset, float threshold)
        {
            if (Math.Abs(offset) > threshold)
            {
                offset -= threshold;
                float curr_len = CalcSpringAngle();
                float rest_len = m_spring.Material.RestDistance;
                if ((offset > 0.0f && rest_len + offset > curr_len) ||
                    (offset < 0.0f && rest_len + offset < curr_len))
                {
                    //Joint_B.CurrPos = Joint_A.CurrPos + this.NormalizedDir * (rest_len + offset);
                    m_spring.Material.RestDistance = m_spring.Material.RestDistanceInit + offset;
                }
            }
        }

        public void UpdateNetAngularSpringAccelerations()
        {
            Joint joint_A = this.LimbA.Joint_A == this.Joint ? this.LimbA.Joint_B : this.LimbA.Joint_A;
            Joint joint_B = this.LimbB.Joint_A == this.Joint ? this.LimbB.Joint_B : this.LimbB.Joint_A;

            Vec2 r_A = joint_A.CurrPos - this.Joint.CurrPos;
            Vec2 r_B = joint_B.CurrPos - this.Joint.CurrPos;
            Vec3 r_z = Vec2.Cross(r_A, r_B);
            if (r_z.Z < 0.0f)
                r_z.Z = -1.0f;
            else
                r_z.Z = 1.0f;
            Vec3 perp_A = Vec3.Cross(r_A, r_z);
            Vec3 perp_B = Vec3.Cross(r_z, r_B);
            Vec2 u_A = new Vec2(perp_A / perp_A.Length / r_A.Length);
            Vec2 u_B = new Vec2(perp_B / perp_B.Length / r_B.Length);
            Vec2 u_0 = -(u_A + u_B);

            m_prev_angle = m_curr_angle;
            float ang_curr = CalcSpringAngle();
            m_curr_angle = ang_curr;
            float ang_0 = m_spring.Material.RestDistance;
            float delta_ang = ang_curr - ang_0;
            //const float rad2deg = 180.0f / (float)Math.PI;
            //Console.WriteLine("id = {0}, prev = {1}, curr = {2}, rest = {3}", m_id, rad2deg * m_prev_angle, rad2deg * m_curr_angle, rad2deg * ang_0);
            //Console.WriteLine("        curr - prev = {0}", rad2deg * (m_curr_angle - m_prev_angle));
            //Console.WriteLine("        curr - rest = {0}", rad2deg * delta_ang);
            if (ang_curr > Math.PI)
            {
                u_0 = -u_0;
                u_A = -u_A;
                u_B = -u_B;
            }
            float k = m_spring.Material.Stiffness;
            float F_k = -k * delta_ang; //(float)Math.Sin(-delta_ang);
            //Console.WriteLine("        F_k = {0}", F_k);

            float dang_dt =
                Vec2.Dot(joint_A.CurrVel, u_A) +
                Vec2.Dot(this.Joint.CurrVel, u_0) +
                Vec2.Dot(joint_B.CurrVel, u_B);
            float c = m_spring.Material.Damping;
            float F_d = -c * dang_dt;
            //Console.WriteLine("        F_d = {0}", F_d);

            float F_tot = F_k + F_d;
            Vec2 F_A = F_tot * u_A;
            Vec2 F_B = F_tot * u_B;
            Vec2 F_0 = F_tot * u_0;
            m_F_A = F_A;
            m_F_B = F_B;
            m_F_0 = F_0;
            //Console.WriteLine("        F_A = {0}", F_A);
            //Console.WriteLine("        F_B = {0}", F_B);
            //Console.WriteLine("        F_0 = {0}", F_0);

            joint_A.CurrAcc += F_A / joint_A.Mass;
            joint_B.CurrAcc += F_B / joint_B.Mass;
            this.Joint.CurrAcc += F_0 / this.Joint.Mass;
        }

        public void Draw(Graphics g)
        {
            Vec2 p0 = m_joint.CurrPos;
            RectangleF rec = new RectangleF(
                p0.X - 10.0f, 
                (Constants.m_canvas_height - p0.Y) - 10.0f, 
                20.0f, 20.0f);
            Vec2 pA = m_limbs[0].Joint_A == m_joint ? 
                m_limbs[0].Joint_B.CurrPos : 
                m_limbs[0].Joint_A.CurrPos;
            float start_ang = (float)Math.Atan2(pA.Y - p0.Y, pA.X - p0.X);
            const float rad2deg = 180.0f / (float)Math.PI;
            g.DrawArc(Pens.Blue, rec, -start_ang * rad2deg, -m_curr_angle * rad2deg);

            Joint joint_A = this.LimbA.Joint_A == this.Joint ? this.LimbA.Joint_B : this.LimbA.Joint_A;
            Joint joint_B = this.LimbB.Joint_A == this.Joint ? this.LimbB.Joint_B : this.LimbB.Joint_A;
            g.DrawLine(Pens.Red, 
                joint_A.CurrPos.ToPointF(Constants.m_canvas_height), 
                (joint_A.CurrPos + m_F_A).ToPointF(Constants.m_canvas_height));
            g.DrawLine(Pens.Green,
                joint_B.CurrPos.ToPointF(Constants.m_canvas_height),
                (joint_B.CurrPos + m_F_B).ToPointF(Constants.m_canvas_height));
            g.DrawLine(Pens.Orange,
                m_joint.CurrPos.ToPointF(Constants.m_canvas_height),
                (m_joint.CurrPos + m_F_0).ToPointF(Constants.m_canvas_height));
        }
        #endregion
    }
}
