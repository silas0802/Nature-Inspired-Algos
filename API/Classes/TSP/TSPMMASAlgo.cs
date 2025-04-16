using API.Classes.Generic;
using System.Numerics;

namespace API.Classes.TSP
{
    public class TSPMMASAlgo : TSPAlgorithm
    {
        private Random random = new Random();
        public double[,] pheromone = new double[0, 0];
        public int numAnts = 1; // Number of ants
        public double alpha = 1;
        public double beta = 1;
        public double rho = 1f; // Pheromone evaporation rate
        public double minPheromone = 0; // Lowest pheromone level
        public double maxPheromone = 0; // Highest pheromone level 

        private int problemSize = 0; // Amount of points
        private double[,] distancePowers = new double[0, 0];

        public override int[] Mutate(int[] original)
        {
            int[] bestSolution = original;
            double bestFitness = Utility.TSPCalculateDistance(nodes, original);

            int[][] solutions = new int[numAnts][];
            double[] fitnesses = new double[numAnts];

            // Evaluate solutions
            for (int k = 0; k < numAnts; k++)
            {
                solutions[k] = ConstructSolution();
                fitnesses[k] = Utility.TSPCalculateDistance(nodes, solutions[k]);
                if (fitnesses[k] < bestFitness)
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
            double[,] pheromone = new double[length, length];
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    pheromone[i, j] = 0.5f; // Initial pheromone level
                }
            }
            return pheromone;
        }

        public override void InitializeAlgorithm(Vector2[] nodes)
        {
            base.InitializeAlgorithm(nodes);
            problemSize = nodes.Length;
            minPheromone = 1.0 / (problemSize * problemSize);
            maxPheromone = 1.0 - 1 / problemSize;
            pheromone = InitializePheromone(problemSize);

            distancePowers = new double[problemSize, problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    if (i != j)
                    {
                        double inverseDistance = 1.0 / Vector2.Distance(nodes[i], nodes[j]);
                        distancePowers[i, j] = Math.Pow(inverseDistance, beta);
                    }
                }
            }
        }

        public int[] ConstructSolution()
        {
            List<int> unvisited = new List<int>(Enumerable.Range(0, problemSize));
            int[] solution = new int[problemSize];
            int currentNode = random.Next(problemSize);
            solution[0] = currentNode;
            unvisited.Remove(currentNode);

            double[,] pheromonePowers = new double[problemSize, problemSize];

            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    if (i != j)
                    {
                        pheromonePowers[i, j] = Math.Pow(pheromone[i, j], alpha);
                    }
                }
            }

            for (int i = 1; i < problemSize; i++)
            {
                double[] probabilities = new double[unvisited.Count];
                double sumProb = 0;
                int index = 0;

                foreach (int nextNode in unvisited)
                {
                    probabilities[index] = pheromonePowers[currentNode, nextNode] * distancePowers[currentNode, nextNode];
                    sumProb += probabilities[index];
                    index++;
                }

                double rand = random.NextDouble() * sumProb;
                double cumulativeProb = 0;
                index = 0;

                foreach (int nextNode in unvisited)
                {
                    cumulativeProb += probabilities[index];
                    if (rand <= cumulativeProb)
                    {
                        currentNode = nextNode;
                        solution[i] = currentNode;
                        unvisited.Remove(currentNode);
                        break;
                    }
                    index++;
                }
            }
            return solution;
        }

        public void UpdatePheromone(int[][] solutions, double[] fitnesses, int[] bestSolution, double bestFitness)
        {
            // Evaporate pheromone
            for (int i = 0; i < problemSize; i++)
            {
                for (int j = 0; j < problemSize; j++)
                {
                    pheromone[i, j] *= (1 - rho);
                    pheromone[i, j] = Math.Max(pheromone[i, j], minPheromone);
                }
            }

            // Deposit pheromone based on best solution
            for (int i = 0; i < bestSolution.Length - 1; i++)
            {
                int from = bestSolution[i];
                int to = bestSolution[i + 1];
                pheromone[from, to] += rho / bestFitness;
                pheromone[from, to] = Math.Min(pheromone[from, to], maxPheromone);
            }
            // Add pheromone for the return to the starting node
            int last = bestSolution[bestSolution.Length - 1];
            int first = bestSolution[0];
            pheromone[last, first] += rho / bestFitness;
            pheromone[last, first] = Math.Min(pheromone[last, first], maxPheromone);
        }
    }
}

