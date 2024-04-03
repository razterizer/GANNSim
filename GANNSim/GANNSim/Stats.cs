using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GANNSim
{
    public class Stats
    {
        public double best_fitness = 0;
        public double best_child_fitness = Double.NegativeInfinity;
        public double pop_mu = 0;
        public double pop_sigma = 0;
        public double pop_min = Double.PositiveInfinity;
        public double pop_max = Double.NegativeInfinity;
        public List<double> pop_fitness = new List<double>();
    };
}
