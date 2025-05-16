namespace API.Classes.BitStrings
{
    public class BitMMASAlgo : BitAlgorithm
    {
        private Random random = new Random();
        private BitProblem selectedProblem;
        public double[,] pheromone = new double[0,0];
        private int problemSize = 0; // Length of the binary string
        public double rho = 1f; // Pheromone evaporation rate
        public double minPheromone = 0; // Lowest pheromone level
        public double maxPheromone = 0; // Highest pheromone level 

        public BitMMASAlgo(BitProblem selectedProblem)
        {
            this.selectedProblem = selectedProblem;
        }
        
        public override void InitializeAlgorithm(int problemSize)
        {
            this.problemSize = problemSize;
            this.minPheromone = 1 / (float)problemSize;
            this.maxPheromone = 1 - this.minPheromone;
            this.rho = MathF.Log(problemSize);
            pheromone = InitializePheromone(problemSize);
        }

        public double[,] InitializePheromone(int length)
        {
            double[,] pheromone = new double[length, 2];
            for (int i = 0; i < length; i++)
            {
                pheromone[i, 0] = 0.5f; // Pheromone for bit 0
                pheromone[i, 1] = 0.5f; // Pheromone for bit 1
            }
            return pheromone;
        }
        public override int[] Mutate(int[] original)
        {
            int[] bestSolution = original;
            int bestFitness = selectedProblem.EvaluateFitness(original);

            int[] solution = ConstructSolution();
            int fitness = selectedProblem.EvaluateFitness(solution);
            if (fitness > bestFitness)
            {
                bestSolution = solution;
                bestFitness = fitness;
            }

            UpdatePheromone(bestSolution);
            return bestSolution;
        }

        public int[] ConstructSolution()
        {
            int[] solution = new int[problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                double prob0 = pheromone[i, 0];
                double prob1 = pheromone[i, 1];
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

        public void UpdatePheromone(int[] bestSolution)
        {
            // Evaporate pheromone
            for (int i = 0; i < problemSize; i++)
            {
                pheromone[i, 0] *= 1-rho;
                pheromone[i, 1] *= 1-rho;
                pheromone[i, 0] = Math.Max(pheromone[i, 0], minPheromone);
                pheromone[i, 1] = Math.Max(pheromone[i, 1], minPheromone);
            }

            // Deposit pheromone based on best solution
            for (int i = 0; i < problemSize; i++)
            {
                if (bestSolution[i] == 1) // If best solution has a 1 at i then boost 1-pheromone 
                {
                    pheromone[i, 1] += rho;
                    pheromone[i, 1] = Math.Min(pheromone[i, 1], maxPheromone);
                }
                else // else boost 0-pheromone
                {
                    pheromone[i, 0] += rho;
                    pheromone[i, 0] = Math.Min(pheromone[i, 0], maxPheromone);
                }
            }
        }

    }
}
