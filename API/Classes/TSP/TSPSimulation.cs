﻿using API.Classes.Generic;
using System.Diagnostics;
using System.Numerics;

namespace API.Classes.TSP
{
    public class TSPSimulation : Simulation
    {
        public new const int MAX_PROBLEM_SIZE = 1000;
        public new const int MAX_ITERATIONS = 50000;
        public int iterations;
        public Vector2[] nodes = null!;
        private AlgorithmParameters? algorithmParameters;
        public void SetParametersForDetailed(AlgorithmParameters algorithmParameters)
        {
            this.nodes = algorithmParameters.nodes;
            this.problemSize = nodes==null ? algorithmParameters.problemSize : nodes.Length;
            this.iterations = algorithmParameters.iterations;
            this.algorithmI = algorithmParameters.algorithmI;
            this.expCount = 1;
            this.expSteps = 1;
            this.algorithmParameters = algorithmParameters;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxProblemSize">The maximum length of the bit string.</param>
        /// <param name="expCount">The amount of experiment repetitions at each problemsize</param>
        /// <param name="expSteps">How many experiments with different problemsizes will be executed</param>
        /// <param name="algorithmI">The algorithm indexes, each bit corresponds to an algorithm.</param>
        /// <param name="problemI">The problem index.</param>
        public void SetParametersForMultiExperiment(AlgorithmParameters algorithmParameters)
        {
            SetParametersForDetailed(algorithmParameters);
            this.expCount = algorithmParameters.expCount;
            this.expSteps = algorithmParameters.expSteps;
        }
        

        public (float[][], int[][][], float[][]) RunDetailedExperiment()
        {
            
            int[][][] result = new int[Utility.CountSetBits((ulong)algorithmI)][][];
            Vector2[] selectedNodes = this.nodes == null ? Utility.GenerateRandomGraph(problemSize, 500, 500) : this.nodes;
            
            

            int currentAlgo = 0;

            int[] startValue = Utility.RandomTSPSolution(problemSize);
            for (int i = 0; i < ALGORITHM_COUNT; i++)
            {
                if ((algorithmI & 1 << i) != 0)
                {
                    result[currentAlgo] = RunDetailedSimulation(startValue, selectedNodes, GetAlgorithm(i));
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
                    distances[i][j] = Utility.TSPCalculateDistance(selectedNodes, result[i][j]);
                }
            }

            return (Utility.ConvertVectorsToFloatArray(selectedNodes), result, distances);

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
            //Vector2[][][] nodes = new Vector2[expSteps][][];
            //for (int i = 0; i < expSteps; i++)
            //{
            //    nodes[i] = new Vector2[expCount][];
            //    for (int j = 0; j < expCount; j++)
            //    {
            //        nodes[i][j] = Utility.GenerateRandomGraph(problemSize/expSteps*(i+1), 500, 500);
            //    }
            //}

            int currentAlgo = 0;

            int[] startValue = Utility.RandomTSPSolution(problemSize);
            for (int i = 0; i < ALGORITHM_COUNT; i++)
            {
                if ((algorithmI & 1 << i) != 0)
                {
                    //result[currentAlgo] = RunMultiSimulation(startValue,nodes,GetAlgorithm(i));
                    result[currentAlgo] = RunMultiSimulation(startValue, GetAlgorithm(i));
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
            if (algorithmParameters == null) throw new NullReferenceException("Algorithm parameters must be set before experiment");

            List<int[]> result = new List<int[]>();
            result.Add(startValue);
            algorithm.InitializeAlgorithm(nodes, algorithmParameters);
            for (int i = 0; i < iterations; i++)
            {
                int[] bestRes = result[result.Count - 1];
                int[] mutatedRes = algorithm.Mutate(bestRes);
                result.Add(mutatedRes);
                if (i % 500 == 0)
                {
                    Debug.WriteLine($"Running... {(int)((float)(i+1)/iterations*100)}%");
                }
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
        public float[] RunMultiSimulation(int[] startValue, TSPAlgorithm algorithm)
        {
            if (algorithmParameters == null) throw new NullReferenceException("Algorithm parameters must be set before experiment");
            
            float[] results = new float[expSteps + 1];

            for (int k = 0; k < expSteps; k++)
            {
                int stepProblemSize = problemSize / expSteps * (k + 1);
                float[] stepResults = new float[expCount];
                for (int i = 0; i < expCount; i++)
                {
                    Vector2[] nodes = Utility.GenerateRandomGraph(stepProblemSize, 500, 500);
                    algorithm.InitializeAlgorithm(nodes, algorithmParameters);

                    int[] bestRes = Utility.RandomTSPSolution(nodes.Length);
                    for (int j = 1; j < iterations; j++)
                    {
                        bestRes = algorithm.Mutate(bestRes);
                    }
                    stepResults[i] = Utility.TSPCalculateDistance(nodes, bestRes);
                }
                results[k+1] = (float)stepResults.Sum() / expCount;
                Debug.WriteLine($"For problemsize {stepProblemSize} Average: {results[k+1]} ");
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
