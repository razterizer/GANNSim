using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GANNSim
{
    class Constants
    {
        public static float m_ground_y;
        public static float m_canvas_width;
        public static float m_canvas_height;
        public static float m_torsion_spring_limb_pos;

        static Constants()
        {
            m_ground_y = 50.0f;
            m_torsion_spring_limb_pos = 1.0f;
        }
    }
}
