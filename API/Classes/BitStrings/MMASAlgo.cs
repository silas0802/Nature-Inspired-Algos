namespace API.Classes.BitStrings
{
    public class MMASAlgo : BitAlgorithm
    {
        private Random random = new Random();
        private BitProblem selectedProblem;
        public double[,] pheromone = new double[0,0];
        private int problemSize = 0; // Length of the binary string
        public int numAnts = 20; // Number of ants
        public double alpha = 1.0; // Pheromone importance
        public double beta = 2.0; // Heuristic importance
        public double rho = 0.1; // Pheromone evaporation rate
        public double minPheromone = 0.1;
        public double maxPheromone = 10.0;

        public MMASAlgo(BitProblem selectedProblem)
        {
            this.selectedProblem = selectedProblem;
        }
        public override int[] Mutate(int[] original)
        {
            int[] bestSolution = new int[problemSize];
            int bestFitness = 0;

            int[][] solutions = new int[numAnts][];
            int[] fitnesses = new int[numAnts];

            // Evaluate solutions
            for (int k = 0; k < numAnts; k++)
            {
                solutions[k] = ConstructSolution();
                fitnesses[k] = Utility.CountSetBits(solutions[k]);
                if (fitnesses[k] > bestFitness)
                {
                    bestSolution = solutions[k];
                    bestFitness = fitnesses[k];
                }
            }

            UpdatePheromone(solutions, fitnesses, bestSolution, bestFitness);
            return bestSolution;
        }
        
        

        public double[,] InitializePheromone(int length)
        {
            double[,] pheromone = new double[length, 2];
            for (int i = 0; i < length; i++)
            {
                pheromone[i, 0] = maxPheromone; // Pheromone for bit 0
                pheromone[i, 1] = maxPheromone; // Pheromone for bit 1
            }
            return pheromone;
        }

        public int[] ConstructSolution()
        {
            int[] solution = new int[problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                double prob0 = Math.Pow(pheromone[i, 0], alpha) * Math.Pow(1.0, beta);
                double prob1 = Math.Pow(pheromone[i, 1], alpha) * Math.Pow(1.0, beta);
                double sumProb = prob0 + prob1;

                double rand = random.NextDouble();
                if (rand < prob1 / sumProb)
                {
                    solution[i] = 1;
                }
                else
                {
                    solution[i] = 0;
                }
            }
            return solution;
        }

        public void UpdatePheromone(int[][] solutions, int[] fitnesses, int[] bestSolution, int bestFitness)
        {
            // Evaporate pheromone
            for (int i = 0; i < problemSize; i++)
            {
                pheromone[i, 0] *= (1 - rho);
                pheromone[i, 1] *= (1 - rho);
                pheromone[i, 0] = Math.Max(pheromone[i, 0], minPheromone);
                pheromone[i, 1] = Math.Max(pheromone[i, 1], minPheromone);
            }

            // Deposit pheromone based on best solution
            for (int i = 0; i < problemSize; i++)
            {
                if (bestSolution[i] == 1)
                {
                    pheromone[i, 1] += bestFitness;
                    pheromone[i, 1] = Math.Min(pheromone[i, 1], maxPheromone);
                }
                else
                {
                    pheromone[i, 0] += bestFitness;
                    pheromone[i, 0] = Math.Min(pheromone[i, 0], maxPheromone);
                }
            }
        }

        public override void InitializeAlgorithm(int problemSize)
        {
            this.problemSize = problemSize;
            pheromone = InitializePheromone(problemSize);
        }
    }
}
