using API.Classes.BitStrings;
using API.Classes.Generic;
using System.Diagnostics;
using System.Numerics;

namespace API.Classes.TSP
{
    public class TSPSimulation : Simulation
    {
        public new const int MAX_PROBLEM_SIZE = 100;
        public new const int MAX_ITERATIONS = 200;
        public void SetParametersForDetailed(int problemSize, int algorithmI)
        {
            this.problemSize = problemSize;
            this.algorithmI = algorithmI;
            this.expCount = 1;
            this.expSteps = 1;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxProblemSize">The maximum length of the bit string.</param>
        /// <param name="expCount">The amount of experiment repetitions at each problemsize</param>
        /// <param name="expSteps">How many experiments with different problemsizes will be executed</param>
        /// <param name="algorithmI">The algorithm indexes, each bit corresponds to an algorithm.</param>
        /// <param name="problemI">The problem index.</param>
        public void SetParametersForMultiExperiment(int maxProblemSize, int expCount, int expSteps, int algorithmI)
        {
            SetParametersForDetailed(maxProblemSize, algorithmI);
            this.expCount = expCount;
            this.expSteps = expSteps;
        }
        
        public (float[][], int[][][], float[][]) RunDetailedExperiment()
        {

            int bitstring = algorithmI;
            int[][][] result = new int[Utility.CountSetBits((ulong)algorithmI)][][];
            Vector2[] nodes = Utility.GenerateRandomGraph(problemSize, 500, 500);
            

            int currentAlgo = 0;

            int[] startValue = Utility.RandomTSPSolution(problemSize);
            for (int i = 0; i < ALGORITHM_COUNT; i++)
            {
                if ((algorithmI & 1 << i) != 0)
                {
                    result[currentAlgo] = RunDetailedSimulation(startValue, nodes, GetAlgorithm(i));
                    currentAlgo++;
                }

            }
            Debug.WriteLine($"Experiment finished with start value: {Utility.DisplayAnyList(startValue)}");
            //Debug.WriteLine($"Result:\n{Utility.DisplayAnyList(result)}");

            //Calculate distances for each iteration for all algorithms
            float[][] distances = new float[result.Length][];
            for (int i = 0; i < distances.Length; i++) // For each algorithm
            {
                distances[i] = new float[result[i].Length];
                for (int j = 0; j < distances[i].Length; j++) // For each iteration
                {
                    distances[i][j] = Utility.TSPCalculateDistance(nodes, result[i][j]);
                }
            }

            return (Utility.ConvertVectorsToFloatArray(nodes), result, distances);

        }

        /// <summary>
        /// Runs the experiment results for each algorithm.
        /// </summary>
        /// <typeparam name="T">The result type for each algorithm.</typeparam>
        /// <param name="simulation">Which type of simulation to run, e.g: RunDetailedSimulation</param>
        /// <returns>An experiment result for each algorithm selected</returns>
        public float[][] RunComparisonExperiment()
        {

            int bitstring = algorithmI;
            float[][] result = new float[Utility.CountSetBits((ulong)algorithmI)][];
            Vector2[][] nodes = new Vector2[expSteps][];
            for (int i = 0; i < expSteps; i++)
            {
                nodes[i] = Utility.GenerateRandomGraph(problemSize/expSteps*(i+1), 500, 500);
            }

            int currentAlgo = 0;

            int[] startValue = Utility.RandomTSPSolution(problemSize);
            for (int i = 0; i < ALGORITHM_COUNT; i++)
            {
                if ((algorithmI & 1 << i) != 0)
                {
                    result[currentAlgo] = RunMultiSimulation(startValue,nodes,GetAlgorithm(i));
                    currentAlgo++;
                }

            }
            
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startValue">The initial path</param>
        /// <param name="algorithm">The selected algorithm</param>
        /// <returns>A list of the best solution at each iteration</returns>
        public int[][] RunDetailedSimulation(int[] startValue, Vector2[] nodes, TSPAlgorithm algorithm)
        {
            List<int[]> result = new List<int[]>();
            result.Add(startValue);
            algorithm.InitializeAlgorithm(nodes);
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                int[] bestRes = result[result.Count - 1];
                int[] mutatedRes = algorithm.Mutate(bestRes);
                result.Add(mutatedRes);
                
                //Debug.WriteLine($"Iteration {i}: {Utility.CountSetBits(result[result.Count - 1])}");

            }
            Debug.WriteLine($"Solution has a distance of: {Utility.TSPCalculateDistance(nodes, result[result.Count - 1])}, initially: {Utility.TSPCalculateDistance(nodes, result[0])}");
            return result.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startValue">The initial path</param>
        /// <param name="algorithm">The selected algorithm</param>
        /// <returns>A list of average number of iterations to solve the problem at each problemsize.</returns>
        public float[] RunMultiSimulation(int[] startValue, Vector2[][] nodes, TSPAlgorithm algorithm)
        {
            float[] results = new float[expSteps + 1];

            for (int k = 0; k < expSteps; k++)
            {
                int[] stepStartVal = Utility.RandomTSPSolution(nodes[k].Length);
                float[] stepResults = new float[expCount];
                for (int i = 0; i < expCount; i++)
                {
                    algorithm.InitializeAlgorithm(nodes[k]);

                    int[] bestRes = stepStartVal;
                    for (int j = 1; j < MAX_ITERATIONS; j++)
                    {
                        int[] mutatedRes = algorithm.Mutate(bestRes);
                        if (Utility.TSPCompare(nodes[k], bestRes, mutatedRes))
                        {
                            bestRes = mutatedRes;
                        }


                        
                        if (j == MAX_ITERATIONS - 1)
                        {
                            stepResults[i] = Utility.TSPCalculateDistance(nodes[k],bestRes);
                        }
                    }
                }
                results[k+1] = (float)stepResults.Sum() / expCount;
                Debug.WriteLine($"For problemsize {stepStartVal.Length} Average: {results[k+1]} Initially: {Utility.TSPCalculateDistance(nodes[k], stepStartVal)}");
            }
            return results;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>A new BitAlgorithm object of given index</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private TSPAlgorithm GetAlgorithm(int index)
        {
            switch (index)
            {
                case 0:
                    return new TSPOneOneEAAlgo();
                case 1:
                    return new TSPSimAnnealAlgo();
                case 2:
                    return new TSPMMASAlgo();
                default:
                    throw new IndexOutOfRangeException($"No algorithm with index: {index}");
            }
        }
    }
}
