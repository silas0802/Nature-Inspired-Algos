using API.Classes.Generic;
using System.Numerics;

namespace API.Classes.TSP
{
    public class TSPMMASAlgo : TSPAlgorithm
    {
        private Random random = new Random();
        public double[,] pheromone = new double[0, 0];
        private int problemSize = 0; // Amount of points
        public int numAnts = 1; // Number of ants
        public double alpha = 1;
        public double beta = 1;
        public double rho = 1f; // Pheromone evaporation rate
        public double minPheromone = 0; // Lowest pheromone level
        public double maxPheromone = 0; // Highest pheromone level 


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

        public int[] ConstructSolution()
        {
            List<int> unvisited = Enumerable.Range(0, problemSize).ToList();
            int[] solution = new int[problemSize];
            int currentNode = random.Next(problemSize);
            solution[0] = currentNode;
            unvisited.Remove(currentNode);

            for (int i = 1; i < problemSize; i++)
            {
                double[] probabilities = new double[unvisited.Count];
                double sumProb = 0;

                for (int j = 0; j < unvisited.Count; j++)
                {
                    int nextNode = unvisited[j];
                    probabilities[j] = Math.Pow(pheromone[currentNode, nextNode], alpha) * Math.Pow(1.0 / Vector2.Distance(nodes[currentNode], nodes[nextNode]), beta);
                    sumProb += probabilities[j];
                }

                double rand = random.NextDouble() * sumProb;
                double cumulativeProb = 0;
                for (int j = 0; j < unvisited.Count; j++)
                {
                    cumulativeProb += probabilities[j];
                    if (rand <= cumulativeProb)
                    {
                        currentNode = unvisited[j];
                        solution[i] = currentNode;
                        unvisited.RemoveAt(j);
                        break;
                    }
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

        public override void InitializeAlgorithm(Vector2[] nodes)
        {
            base.InitializeAlgorithm(nodes);
            problemSize = nodes.Length;
            minPheromone = 1.0 / (problemSize * problemSize);
            maxPheromone = 1.0 - 1 / problemSize;
            pheromone = InitializePheromone(problemSize);
        }

    }
}
