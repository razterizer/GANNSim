using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GANNSim.morphology;
using System.Threading;
using System.Threading.Tasks;

namespace GANNSim.species
{
    class Population
    {
        float m_mutation_rate;            // Mutation rate
        List<Individual> m_population;
        int m_generations;                // Number of generations
        static Random m_random;
        bool m_use_additive_noise;
        float m_additive_noise_amp;

        static Population()
        {
            m_random = new Random((int)DateTime.Now.Ticks);
        }

        public Population(System.Xml.XmlDocument design_xml_doc, bool drop_on_head, 
                          float mutation_rate, bool use_additive_noise, float additive_noise_amp,
                          int init_population_size)
        {
            m_mutation_rate = mutation_rate;
            m_use_additive_noise = use_additive_noise;
            m_additive_noise_amp = additive_noise_amp;

            m_population = new List<Individual>();
            Individual individual;
            //individual = new Individual();
            //individual.Build();
            //Genome genome = individual.Genome;
            for (int individual_idx = 0; individual_idx < init_population_size; individual_idx++)
            {
                individual = new Individual(design_xml_doc, drop_on_head);
                //individual.Build(genome);
                individual.Build();
                m_population.Add(individual);
            }

            m_generations = 0;
        }

        public Population(float mutation_rate, bool use_additive_noise, float additive_noise_amp,
                          List<Individual> individuals, int generations)
        {
            m_mutation_rate = mutation_rate;
            m_use_additive_noise = use_additive_noise;
            m_additive_noise_amp = additive_noise_amp;
            m_population = individuals;
            m_generations = generations;
            //Console.WriteLine(m_population[0].Genome.m_chromosome_neural_weights.BasePair(0));
        }

        public void CalcFitness(float contact_penalty_factor)
        {
            foreach (Individual individual in m_population)
            {
                individual.CalcFitness(contact_penalty_factor);
            }

            // Sort individuals by fitness.
            m_population.Sort(); // Seems to sort in ascending order.
            // Make the most fitted end up on the top.
            m_population.Reverse();
        }

        public Stats CalcStats()
        {
            Stats stats = new Stats();
            stats.best_fitness = Best.Fitness;

            double fitness_sum = 0;
            foreach (Individual individual in m_population)
                fitness_sum += individual.Fitness;
            stats.pop_mu = fitness_sum / m_population.Count;

            double sum_diff_sq = 0;
            foreach (Individual individual in m_population)
            {
                double diff = individual.Fitness - stats.pop_mu;
                sum_diff_sq += diff * diff;
                if (individual.Genome.IsChild)
                    stats.best_child_fitness = Math.Max(stats.best_child_fitness, individual.Fitness);
                stats.pop_min = Math.Min(stats.pop_min, individual.Fitness);
                stats.pop_max = Math.Max(stats.pop_max, individual.Fitness);
                stats.pop_fitness.Add(individual.Fitness);
            }
            stats.pop_sigma = Math.Sqrt(sum_diff_sq / m_population.Count);
            if (double.IsInfinity(stats.best_child_fitness))
                stats.best_child_fitness = 0;

            return stats;
        }

        public void PerformNaturalSelection(int num_deaths)
        {
            // Better model: Remove randomly.
            int max_population_idx = m_population.Count - 1;
            for (int remove_idx = 0; remove_idx < num_deaths; remove_idx++)
            {
                int individual_idx = uniform_random_pick(max_population_idx--);
                Individual individual = m_population[individual_idx];
                individual.Clear();
                int pre_size = m_population.Count;
                m_population.Remove(individual);
                if (m_population.Count - pre_size > 1)
                    System.Windows.Forms.MessageBox.Show("Removing too many individuals at once");
            }
        }

        public void InsertBest(Individual best_individual, float replacement_probability)
        {
            if (m_random.NextDouble() < replacement_probability)
            {
                if (!(m_population.Count > 0 && m_population[0] == best_individual))
                {
                    m_population.Remove(m_population.Last());
                    m_population.Insert(0, best_individual);
                }
            }
        }

        public void Breed(int num_matings, int num_crossover_pts, int num_children_per_mating)
        {
            // At the end of the epoch everyone will become potential parents.
            foreach (Individual individual in m_population)
                individual.Genome.IsChild = false;

            // Normalize fitnesses
            double min_fitness = m_population.Last().Fitness;
            double tot_fitness = 0.0;
            foreach (Individual individual in m_population)
                tot_fitness += individual.Fitness - min_fitness;
            foreach (Individual indiviual in m_population)
                indiviual.NormalizeFitness(min_fitness, tot_fitness);

            // Fetch unique individuals that are allowed to mate by using roulette sampling.
            List<Individual> mating_population = new List<Individual>();
            for (int mating_idx = 0; mating_idx < num_matings; mating_idx++)
            {
                Individual partner = null;
                do
                {
                    partner = roulette_pick();
                } while (mating_population.Contains(partner));
                if (partner != null)
                    mating_population.Add(partner);
            }

            // Do not proceed with mating if there is less than 2 individuals in the mating population.
            if (mating_population.Count < 2)
                return;

            // Let everyone have sex with practically everyone from the mating population.
            // A full fledged orgy! yay!!!
            List<Individual> m_next_generation = new List<Individual>();
            int max_population_idx = mating_population.Count - 1;
            for (int mating_idx = 0; mating_idx < mating_population.Count; mating_idx++)
            {
                Individual mom = mating_population[mating_idx];

                Individual dad;
                int dad_idx;
                do // Don't want it to fuck itself.
                {
                    dad_idx = uniform_random_pick(max_population_idx);
                    dad = m_population[dad_idx];
                } while (dad == mom);

                Individual[] children = mom.Mate(dad, num_crossover_pts, num_children_per_mating);
                for (int child_idx = 0; child_idx < children.Length; child_idx++)
                    children[child_idx].Genome.Mutate(m_mutation_rate, m_use_additive_noise, m_additive_noise_amp);
                m_next_generation.AddRange(children);
            }
            // Add the new generation to the population.
            m_population.AddRange(m_next_generation);
            m_generations++;
        }

        private Individual roulette_pick()
        {
            double fitness_sum = 0.0;
            float rand_pick = (float)m_random.NextDouble();
            foreach (Individual individual in m_population)
            {
                fitness_sum += individual.NormFitness;
                if (fitness_sum >= rand_pick)
                    return individual;
            }
            return null;
        }

        private int uniform_random_pick(int max_population_idx)
        {
            return (int)Math.Round(m_random.NextDouble() * max_population_idx);
        }

        public Individual Best
        {
            get
            {
                if (m_population.Count == 0)
                    return null;
                return m_population[0];
            }
        }

        public bool Finished
        {
            get
            {
                Individual best_individual = this.Best;
                if (best_individual == null)
                    return false;

                bool finished = false;
                if (best_individual.Fitness >= 0.999999f)
                    finished = true;
                //if (m_population.Count >= m_final_population_size)
                //    finished = true;
                return finished;
            }
        }

        public int NumGenerations
        {
            get { return m_generations; }
        }

        public int Size
        {
            get { return m_population.Count; }
        }

        public void CurrPopulationStepSimulation(float fix_dt, Phenotype.IntegrationMethod integration_method,
            bool use_brain, 
            int num_main_subiters, int num_brain_subiters, int num_phys_subiters, 
            bool show_best, bool only_run_relevant, int thread_priority)
        {
            List<Individual> population_subset = null;

            if (show_best)
                Best.StepSimulation(fix_dt, integration_method, use_brain,
                    num_main_subiters, num_brain_subiters, num_phys_subiters);
            else if (only_run_relevant)
            {
                population_subset = new List<Individual>();
                foreach (Individual individual in m_population)
                    if (individual == Best || individual.Genome.IsChild || !individual.IsSimulated)
                        population_subset.Add(individual);
            }
            else
                population_subset = m_population;

            if (population_subset != null)
            {
                if (thread_priority == 0)
                    foreach (Individual individual in population_subset)
                    {
                        individual.StepSimulation(fix_dt, integration_method, use_brain,
                        num_main_subiters, num_brain_subiters, num_phys_subiters);
                    }
                else
                    Parallel.ForEach(population_subset, individual =>
                    {
                        Thread.CurrentThread.Priority = (ThreadPriority)(thread_priority - 1);
                        individual.StepSimulation(fix_dt, integration_method, use_brain,
                            num_main_subiters, num_brain_subiters, num_phys_subiters);
                    });
            }
        }

        public void Draw(System.Drawing.Graphics g, 
            bool debug_draw_joints, bool debug_draw_angsprings, 
            bool draw_dna, bool draw_name,
            bool show_best, bool only_run_relevant)
        {
            if (show_best)
                Best.Draw(g,
                    0, m_population.Count,
                    debug_draw_joints, debug_draw_angsprings,
                    draw_dna, draw_name);
            else if (only_run_relevant)
            {
                Best.Draw(g,
                    0, m_population.Count,
                    debug_draw_joints, debug_draw_angsprings,
                    draw_dna, draw_name);
                for (int individual_idx = 1; individual_idx < m_population.Count; individual_idx++)
                {
                    Individual individual = m_population[individual_idx];
                    if (individual.Genome.IsChild || !individual.IsSimulated)
                        individual.Draw(g,
                            individual_idx, m_population.Count,
                            debug_draw_joints, debug_draw_angsprings,
                            draw_dna, draw_name);
                }
            }
            else
                for (int individual_idx = 0; individual_idx < m_population.Count; individual_idx++)
                    m_population[individual_idx].Draw(g,
                        individual_idx, m_population.Count,
                        debug_draw_joints, debug_draw_angsprings,
                        draw_dna, draw_name);
        }

        public void ApplyForce(System.Drawing.Point p)
        {
            foreach (Individual individual in m_population)
                individual.Body.ApplyForce(p);
        }

        public float OutputGain
        {
            set
            {
                foreach (Individual individual in m_population)
                    individual.Body.OutputGain = value;
            }
        }

        public IEnumerable<Individual> Individuals
        {
            get { return m_population; }
        }

        public int NumIndividuals
        {
            get { return m_population.Count; }
        }

        public Individual FirstIndividual
        {
            get { return m_population[0]; }
        }

        public void Clear()
        {
            foreach (Individual individual in m_population)
                individual.Clear();
            m_population.Clear();
        }

        public void ResetAndRandomize()
        {
            foreach (Individual individual in m_population)
                individual.ResetAndRandomize();
        }

        public void Reset()
        {
            foreach (Individual individual in m_population)
                individual.Reset();
        }

        public void Add(Individual individual)
        {
            m_population.Add(individual);
        }
    }
}
