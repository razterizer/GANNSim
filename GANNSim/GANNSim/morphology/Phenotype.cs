using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using GANNSim.neural;
using GANNSim.dynamics;
using GANNSim.species;
using System.Xml;
using MathLib.geometry._2d;
using MathLib.linalg._2d;
using MathLib.utils;
using MathLib.complex;

namespace GANNSim.morphology
{
    class Phenotype
    {
        XmlDocument m_design_xml_doc;
        Dictionary<int, Joint> m_joints;
        Dictionary<int, Limb> m_limbs;
        Dictionary<int, AngularSpring> m_angular_springs;
        Brain m_brain;

        float m_last_angle;
        float m_output_gain;

        int m_num_head_contacts;
        bool m_drop_on_head;

        bool m_is_flying;

        bool m_uses_mean_orientation;
        bool m_uses_mean_angvel;

        public enum IntegrationMethod { ForwardEuler, SemiImplicitEuler, VelocityVerlet }

        public Phenotype(XmlDocument design_xml_doc, bool drop_on_head)
        {
            m_design_xml_doc = design_xml_doc;
            m_drop_on_head = drop_on_head;
            m_joints = new Dictionary<int, Joint>();
            m_limbs = new Dictionary<int, Limb>();
            m_angular_springs = new Dictionary<int, AngularSpring>();
            m_last_angle = float.NaN;
            m_output_gain = 0.25f;
            m_num_head_contacts = 0;
            m_is_flying = false;
            m_uses_mean_orientation = false;
            m_uses_mean_angvel = false;

            //m_brain = new Brain(19, new int[] { 8, 2 }, 4);
            //m_brain = new Brain(new Brain.HiddenLayerData[] { }, 4);

            XmlNode brain_node = design_xml_doc.FirstChild["brain"];
            XmlNode input_layer_node = brain_node["input_layer"];
            XmlNode signals_node = input_layer_node["signals"];
            List<Input> inputs = new List<Input>();
            foreach (XmlNode signal_node in signals_node.ChildNodes)
            {
                XmlAttribute active_attr = signal_node.Attributes["active"];
                bool active = bool.Parse(active_attr.Value);

                if (active)
                {
                    XmlAttribute name_attr = signal_node.Attributes["name"];
                    string name = name_attr.Value;
                    if (name == "mean angle")
                        m_uses_mean_orientation = true;
                    if (name == "mean ang.vel")
                        m_uses_mean_angvel = true;
                    XmlAttribute id_attr = signal_node.Attributes["id"];
                    int id = int.Parse(id_attr.Value);

                    Input input = new Input(name, id);
                    inputs.Add(input);
                }
            }

            XmlNode hidden_layers_node = brain_node["hidden_layers"];
            List<Brain.HiddenLayerData> hidden_layer_data = new List<Brain.HiddenLayerData>();
            foreach (XmlNode layer_node in hidden_layers_node)
            {
                XmlNode recurrent_node = layer_node["is_recurrent"];
                bool is_recurrent = bool.Parse(recurrent_node.InnerText);

                XmlNode neurons_h_node = layer_node["neurons"];
                int num_h_neurons = int.Parse(neurons_h_node.InnerText);

                hidden_layer_data.Add(new Brain.HiddenLayerData(num_h_neurons, is_recurrent));
            }

            XmlNode output_layer_node = brain_node["output_layer"];
            XmlNode neurons_o_node = output_layer_node["neurons"];
            int num_o_neurons = int.Parse(neurons_o_node.InnerText);

            m_brain = new Brain(inputs, hidden_layer_data.ToArray(), num_o_neurons);
        }

        public void Build(Genome genome)
        {
            XmlNode body_node = m_design_xml_doc.FirstChild["body"];
            XmlAttribute scaling_attr = body_node.Attributes["scaling"];
            float scaling = float.Parse(scaling_attr.Value, System.Globalization.NumberFormatInfo.InvariantInfo);

            // Joints.

            XmlNode joints_node = body_node["joints"];
            foreach (XmlNode joint_node in joints_node.ChildNodes)
            {
                XmlAttribute id_attr = joint_node.Attributes["id"];
                int id = int.Parse(id_attr.Value);
                XmlNode sensor_node = joint_node["has_sensor"];
                bool has_sensor = bool.Parse(sensor_node.InnerText);
                XmlNode head_node = joint_node["is_head"];
                bool is_head = bool.Parse(head_node.InnerText);
                XmlNode position_node = joint_node["position"];
                Point pos = new Point(
                    int.Parse(position_node.Attributes["x"].Value),
                    (int)Constants.m_canvas_height - int.Parse(position_node.Attributes["y"].Value));
                XmlNode mass_node = joint_node["mass"];
                float mass = float.Parse(mass_node.InnerText, System.Globalization.NumberFormatInfo.InvariantInfo);

                Joint joint = new Joint(id.ToString(), new Vec2(pos.X, pos.Y), mass);
                joint.ID = id;
                joint.HasContactSensor = has_sensor;
                joint.IsHead = is_head;
                m_joints.Add(id, joint);
            }

            // Transform geometry.
            Vec2 start_pos = new Vec2(100.0f, 100.0f);
            Vec2 mean_pos = this.MeanPosition;
            foreach (Joint joint in m_joints.Values)
            {
                joint.CurrPos -= mean_pos;
                joint.CurrPos *= scaling;
                if (m_drop_on_head)
                    joint.CurrPos = new Vec2(joint.CurrPos.X, - joint.CurrPos.Y); // Annoying copying property.
                joint.CurrPos += start_pos;
            }

            // Limbs.

            XmlNode limbs_node = body_node["limbs"];
            foreach (XmlNode limb_node in limbs_node.ChildNodes)
            {
                XmlAttribute id_attr = limb_node.Attributes["id"];
                int id = int.Parse(id_attr.Value);

                XmlNode muscle_node = limb_node["is_muscle"];
                bool is_muscle = bool.Parse(muscle_node.InnerText);

                XmlNode gain_node = limb_node["muscle_gain"];
                float muscle_gain = float.Parse(gain_node.InnerText, System.Globalization.NumberFormatInfo.InvariantInfo);

                XmlNode joint_A_node = limb_node["jointA"];
                XmlAttribute joint_A_id_attr = joint_A_node.Attributes["id"];
                int joint_A_id = int.Parse(joint_A_id_attr.Value);

                XmlNode joint_B_node = limb_node["jointB"];
                XmlAttribute joint_B_id_attr = joint_B_node.Attributes["id"];
                int joint_B_id = int.Parse(joint_B_id_attr.Value);

                XmlNode stiffness_node = limb_node["stiffness"];
                float stiffness = float.Parse(stiffness_node.InnerText, System.Globalization.NumberFormatInfo.InvariantInfo);

                XmlNode damping_node = limb_node["damping"];
                float damping = float.Parse(damping_node.InnerText, System.Globalization.NumberFormatInfo.InvariantInfo);

                Limb limb = new Limb(id.ToString());
                limb.ID = id;
                limb.MuscleGain = muscle_gain;
                limb.IsMuscle = is_muscle;
                foreach (Joint joint in m_joints.Values)
                {
                    if (joint.ID == joint_A_id || joint.ID == joint_B_id)
                        joint.AddLimb(limb);
                }
                limb.SetLinearSpring(stiffness, damping);
                m_limbs.Add(id, limb);
            }

            // Angular springs.

            foreach (XmlNode joint_node in joints_node.ChildNodes)
            {
                XmlAttribute joint_id_attr = joint_node.Attributes["id"];
                int joint_id = int.Parse(joint_id_attr.Value);

                XmlNode angsprings_node = joint_node["angular_springs"];
                foreach (XmlNode spring_node in angsprings_node)
                {
                    XmlAttribute id_attr = spring_node.Attributes["id"];
                    int id = int.Parse(id_attr.Value);

                    XmlNode muscle_node = spring_node["is_muscle"];
                    bool is_muscle = bool.Parse(muscle_node.InnerText);

                    XmlNode gain_node = spring_node["muscle_gain"];
                    float muscle_gain = float.Parse(gain_node.InnerText, System.Globalization.NumberFormatInfo.InvariantInfo);

                    XmlNode limb_A_node = spring_node["limbA"];
                    XmlAttribute limb_A_id_attr = limb_A_node.Attributes["id"];
                    int limb_A_id = int.Parse(limb_A_id_attr.Value);

                    XmlNode limb_B_node = spring_node["limbB"];
                    XmlAttribute limb_B_id_attr = limb_B_node.Attributes["id"];
                    int limb_B_id = int.Parse(limb_B_id_attr.Value);

                    XmlNode stiffness_node = spring_node["stiffness"];
                    float stiffness = float.Parse(stiffness_node.InnerText, System.Globalization.NumberFormatInfo.InvariantInfo);

                    XmlNode damping_node = spring_node["damping"];
                    float damping = float.Parse(damping_node.InnerText, System.Globalization.NumberFormatInfo.InvariantInfo);

                    Joint joint_C = null;
                    foreach (Joint joint in m_joints.Values)
                        if (joint.ID == joint_id)
                        {
                            joint_C = joint;
                            break;
                        }

                    Limb limb_A = null;
                    Limb limb_B = null;
                    foreach (Limb limb in m_limbs.Values)
                    {
                        if (limb.ID == limb_A_id)
                            limb_A = limb;
                        else if (limb.ID == limb_B_id)
                            limb_B = limb;
                    }

                    if (joint_C != null & limb_A != null && limb_B != null)
                    {
                        AngularSpring spring = new AngularSpring(
                            joint_C, limb_A, limb_B, 
                            stiffness, damping);

                        spring.ID = id;
                        spring.MuscleGain = muscle_gain;
                        spring.IsMuscle = is_muscle;
                        m_angular_springs.Add(id, spring);
                    }
                }
            }

            m_brain.Build(genome.m_chromosome_neural_weights, genome.m_chromosome_neural_alphas);
        }

        public float OutputGain
        {
            set { m_output_gain = value; }
        }

        public void UpdateBrain(float dt, int num_sub_iters)
        {
            Vec2 p_mean = this.MeanPosition;
            Vec2 v_mean = this.MeanVelocity;

            float mean_angle = 0.0f;
            if (m_uses_mean_orientation || m_uses_mean_angvel)
            {
                mean_angle = CalcMeanOrientation(p_mean, MeanPositionOrig);
            }

            float mean_dangle_dt = 0.0f;
            if (m_uses_mean_angvel && !float.IsNaN(m_last_angle))
            {
                Complex z = new Complex();
                z.FromPolar(1.0f, mean_angle - m_last_angle);
                mean_dangle_dt = z.Arg() / dt;
            }

            m_last_angle = mean_angle;

            foreach (Input input in m_brain.Inputs)
            {
                switch (input.Name)
                {
                    case "centroid x position":
                        input.Signal = p_mean.X;
                        break;
                    case "centroid y position":
                        input.Signal = p_mean.Y;
                        break;
                    case "centroid x velocity":
                        input.Signal = v_mean.X;
                        break;
                    case "centroid y velocity":
                        input.Signal = v_mean.Y;
                        break;
                    case "mean angle":
                        input.Signal = mean_angle;
                        //Console.WriteLine("mean angle = {0}", mean_angle);
                        break;
                    case "mean ang.vel":
                        input.Signal = mean_dangle_dt;
                        //Console.WriteLine("dang / dt = {0}", mean_dangle_dt);
                        break;
                    case "joint contact":
                        input.Signal = m_joints[input.ID].ContactForce;
                        break;
                    case "linear muscle length":
                        input.Signal = m_limbs[input.ID].Length;
                        break;
                    case "linear muscle velocity":
                        input.Signal = m_limbs[input.ID].Velocity;
                        break;
                    case "angular muscle angle":
                        input.Signal = m_angular_springs[input.ID].CalcSpringAngle();
                        break;
                    case "angular muscle ang.vel.":
                        input.Signal = m_angular_springs[input.ID].CalcSpringAngularVelocity();
                        break;
                }
            }

            // Update brain states.
            for (int sub_iter = 0; sub_iter < num_sub_iters; sub_iter++)
                m_brain.Update();

            // Use brain outputs on actuators.
            const float c_sig_min = -5.0f; //-0.005f;
            const float c_sig_max = +5.0f; //+0.005f;
            const float c_threshold = 0.1f; //0.002f;
            int output_id = 0;
            foreach (Limb limb in m_limbs.Values)
            {
                if (limb.IsMuscle)
                    limb.SetLengthOffset(
                        m_output_gain * 
                        limb.MuscleGain * 
                        UtilsFloat.Clamp(m_brain.OutputNeurons[output_id++].OutputCurr, c_sig_min, c_sig_max), c_threshold);
            }
            foreach (AngularSpring spring in m_angular_springs.Values)
            {
                if (spring.IsMuscle)
                    spring.SetSpringAngleOffset(
                        m_output_gain * 
                        spring.MuscleGain * 
                        UtilsFloat.Clamp(m_brain.OutputNeurons[output_id++].OutputCurr, c_sig_min, c_sig_max), c_threshold);
            }

        }

        public void UpdateDynamics(float dt, Vec2 g, IntegrationMethod integration_method)
        {
            const float linear_damping = 0.01f;
            const float contact_stiffness = 200.0f;
            const float mu = 2.5f; // 0.5f;

            Integration.Method1D solver_1d = null;
            Integration.Method2D solver_2d = null;
            switch (integration_method)
            {
                case IntegrationMethod.ForwardEuler:
                    solver_1d = Integration.ForwardEuler;
                    solver_2d = Integration.ForwardEuler;
                    break;
                case IntegrationMethod.SemiImplicitEuler:
                    solver_1d = Integration.SemiImplicitEuler;
                    solver_2d = Integration.SemiImplicitEuler;
                    break;
                case IntegrationMethod.VelocityVerlet:
                    solver_1d = Integration.VelocityVerlet;
                    solver_2d = Integration.VelocityVerlet;
                    break;
            }

            //foreach (Joint joint in m_joints.Values)
            //    joint.CurrAcc.SetZero();
            foreach (Joint joint in m_joints.Values)
                joint.SetNetLinearSpringAcceleration();
            foreach (AngularSpring spring in m_angular_springs.Values)
                spring.UpdateNetAngularSpringAccelerations();
            foreach (Joint joint in m_joints.Values)
                joint.UpdateDynamics(solver_2d, dt, 
                    g, linear_damping, contact_stiffness, mu);
            foreach (Limb limb in m_limbs.Values)
                limb.UpdateSpring(solver_1d, dt);

            m_is_flying = true;
            foreach (Joint joint in m_joints.Values)
            {
                if (joint.IsHead)
                {
                    if (joint.InContact)
                        m_num_head_contacts++;
                    //if (joint.InContact && !m_head_was_in_contact)
                    //{
                    //    m_num_head_contacts++;
                    //    m_head_was_in_contact = true;
                    //}
                    //else if (!joint.InContact && m_head_was_in_contact)
                    //    m_head_was_in_contact = false;
                }
                if (joint.InContact)
                    m_is_flying = false;
            }
        }

        public float Mass
        {
            get
            {
                float mass = 0.0f;
                foreach (Joint joint in m_joints.Values)
                    mass += joint.Mass;
                return mass;
            }
        }

        public Vec2 MeanPosition
        {
            get
            {
                Vec2 p_mean = new Vec2();
                foreach (Joint joint in m_joints.Values)
                    p_mean += joint.CurrPos;
                p_mean /= m_joints.Count;
                return p_mean;
            }
        }

        private Vec2 MeanPositionOrig
        {
            get
            {
                Vec2 p_mean = new Vec2();
                foreach (Joint joint in m_joints.Values)
                    p_mean += joint.OrigPos;
                p_mean /= m_joints.Count;
                return p_mean;
            }
        }

        public Vec2 MeanVelocity
        {
            get
            {
                Vec2 v_mean = new Vec2();
                foreach (Joint joint in m_joints.Values)
                    v_mean += joint.CurrVel;
                v_mean /= m_joints.Count;
                return v_mean;
            }
        }

        public float CalcMeanOrientation(Vec2 mean_pos_curr, Vec2 mean_pos_orig)
        {
            Vec2 pc_curr = mean_pos_curr;
            Vec2 pc_orig = mean_pos_orig;
            Complex z_tot = 0.0f;
            for (int joint_idx = 0; joint_idx < m_joints.Count; joint_idx++)
            {
                Joint joint = m_joints.Values.ElementAt(joint_idx);
                Vec2 p_orig = joint.OrigPos - pc_orig;
                Vec2 p_curr = joint.CurrPos - pc_curr;
                Complex z_orig = new Complex(p_orig.X, p_orig.Y);
                Complex z_curr = new Complex(p_curr.X, p_curr.Y);
                z_tot += z_curr.GetNormalized() / z_orig.GetNormalized();
            }
            z_tot /= m_joints.Count;
            return z_tot.Arg();
        }

        public AABB BoundingBox
        {
            get
            {
                AABB aabb = new AABB();
                foreach (Joint joint in m_joints.Values)
                    aabb.AddPoint(joint.CurrPos);
                return aabb;
            }
        }

        public int NumHeadContacts
        {
            get { return m_num_head_contacts; }
        }

        public bool IsFlying
        {
            get { return m_is_flying; }
        }

        public Brain Brain
        {
            get { return m_brain; }
        }

        public void Draw(Graphics g, int alpha, 
            bool debug_draw_joints, bool debug_draw_angsprings)
        {
            float x_offs = 0.0f;
            float y_offs = Constants.m_canvas_height;
            Pen bone_pen = new Pen(Color.FromArgb(alpha, Color.Olive), 3.0f);
            Pen muscle_pen = new Pen(Color.FromArgb(alpha, Color.Salmon), 2.0f);
            foreach (Limb limb in m_limbs.Values)
            {
                float x0 = x_offs + limb.Joint_A.CurrPos.X;
                float y0 = y_offs - limb.Joint_A.CurrPos.Y;
                float x1 = x_offs + limb.Joint_B.CurrPos.X;
                float y1 = y_offs - limb.Joint_B.CurrPos.Y;

                g.DrawLine(limb.IsMuscle ? muscle_pen : bone_pen,
                    x0, y0,
                    x1, y1);
            }

            Brush joint_pen = new SolidBrush(Color.FromArgb(alpha, Color.CornflowerBlue));
            Brush joint_contact_pen = new SolidBrush(Color.FromArgb(alpha, Color.Red));
            float radius = 4.0f;
            float diam = 2.0f * radius;
            foreach (Joint joint in m_joints.Values)
            {
                if (joint.InContact)
                    g.FillEllipse(joint_contact_pen, joint.CurrPos.X - radius, y_offs - joint.CurrPos.Y - radius, diam, diam);
                else
                    g.FillEllipse(joint_pen, joint.CurrPos.X - radius, y_offs - joint.CurrPos.Y - radius, diam, diam);

                if (debug_draw_joints)
                    joint.DebugDraw(g, alpha);
            }

            if (debug_draw_angsprings)
                foreach (AngularSpring spring in m_angular_springs.Values)
                    spring.Draw(g);
        }

        public void ApplyForce(Point p)
        {
            Vec2 pv = new Vec2(p.X, Constants.m_canvas_height - p.Y);
            foreach (Limb limb in m_limbs.Values)
            {
                float t_limb = Vec2.Dot(
                    pv - limb.Joint_A.CurrPos,
                    limb.Joint_B.CurrPos - limb.Joint_A.CurrPos) / limb.LengthSquared;

                if (0.0f < t_limb && t_limb < 1.0f)
                {
                    Vec2 p_proj =
                        limb.Joint_A.CurrPos * (1.0f - t_limb) +
                        limb.Joint_B.CurrPos * t_limb;
                    float dist_sq = Vec2.DistanceSquared(pv, p_proj);
                    if (dist_sq < 36)
                    {
                        if (t_limb < 0.5f)
                            limb.AddTransientForce(+200.0f);
                        else
                            limb.AddTransientForce(-200.0f);
                    }
                }
            }
        }

        public void Clear()
        {
            m_joints.Clear();
            m_limbs.Clear();
            m_angular_springs.Clear();
            m_brain.Clear();
            m_last_angle = float.NaN;
            m_num_head_contacts = 0;
            m_is_flying = false;
        }

        public int NumJoints
        {
            get { return m_joints.Count; }
        }
    }
}
