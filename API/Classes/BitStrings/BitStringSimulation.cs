using API.Classes.Generic;
using System.Diagnostics;

namespace API.Classes.BitStrings
{
    public class BitStringSimulation : Simulation
    {
        private int _problemI;
        public int problemI
        {
            get => _problemI;
            private set
            {
                if (value >= PROBLEM_COUNT || value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _problemI = value;
            }
        }

        /// <summary>
        /// Sets the parameters for the single/detailed simulation.
        /// </summary>
        /// <param name="problemSize">The length of the bit string.</param>
        /// <param name="algorithmI">The algorithm indexes, each bit corresponds to an algorithm.</param>
        /// <param name="problemI">The problem index.</param>
        public void SetParametersForDetailed(int problemSize, int algorithmI, int problemI)
        {
            this.problemSize = problemSize;
            this.algorithmI = algorithmI;
            this.problemI = problemI;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxProblemSize">The maximum length of the bit string.</param>
        /// <param name="expCount">The amount of experiment repetitions at each problemsize</param>
        /// <param name="expSteps">How many experiments with different problemsizes will be executed</param>
        /// <param name="algorithmI">The algorithm indexes, each bit corresponds to an algorithm.</param>
        /// <param name="problemI">The problem index.</param>
        public void SetParametersForMultiExperiment(int maxProblemSize, int expCount, int expSteps, int algorithmI, int problemI)
        {
            SetParametersForDetailed(maxProblemSize, algorithmI, problemI);
            this.expCount = expCount;
            this.expSteps = expSteps;
        }

        /// <summary>
        /// Runs the experiment results for each algorithm.
        /// </summary>
        /// <typeparam name="T">The result type for each algorithm.</typeparam>
        /// <param name="simulation">Which type of simulation to run, e.g: RunDetailedSimulation</param>
        /// <returns>An experiment result for each algorithm selected</returns>
        public T[] RunExperiment<T>(Func<int[], BitAlgorithm, BitProblem, T> simulation)
        {

            int bitstring = algorithmI;
            T[] result = new T[Utility.CountSetBits((ulong)algorithmI)];

            int currentAlgo = 0;
            BitProblem selectedProblem = GetProblem(problemI);

            int[] startValue = Utility.InitializeRandomBinaryString(problemSize);
            for (int i = 0; i < ALGORITHM_COUNT; i++)
            {
                if ((algorithmI & 1 << i) != 0)
                {
                    result[currentAlgo] = simulation(startValue, GetAlgorithm(i, selectedProblem), selectedProblem);
                    currentAlgo++;
                }

            }
            Debug.WriteLine($"Experiment finished with start value: {{{string.Join(',', startValue)}}}");
            Debug.WriteLine($"Result:\n{Utility.DisplayAnyList(result)}");
            return result;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startValue">The initial bitarray</param>
        /// <param name="algorithm">The selected algorithm</param>
        /// <param name="problem">The selected problem</param>
        /// <returns>A list of the best solutions at each iteration</returns>
        public int[][] RunDetailedSimulation(int[] startValue, BitAlgorithm algorithm, BitProblem problem)
        {
            List<int[]> result = new List<int[]>();
            result.Add(startValue);
            algorithm.InitializeAlgorithm(startValue.Length);
            for (int i = 0; i < MAX_ITERATIONS; i++)
            {
                int[] bestRes = result[result.Count - 1];
                int[] mutatedRes = algorithm.Mutate(bestRes);
                if (problem.FitnessCompare(bestRes, mutatedRes))
                {
                    result.Add(mutatedRes);
                }
                else
                {
                    result.Add(bestRes);
                }
                //Debug.WriteLine($"Iteration {i}: {Utility.CountSetBits(result[result.Count - 1])}");
                if (Utility.CountSetBits(result[result.Count - 1]) == problemSize)
                {
                    Debug.WriteLine($"Solution found after {i} iterations");
                    break;
                }
            }

            return result.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="startValue">The initial bitarray</param>
        /// <param name="algorithm">The selected algorithm</param>
        /// <param name="problem">The selected problem</param>
        /// <returns>A list of average number of iterations to solve the problem at each problemsize.</returns>
        public float[] RunMultiSimulation(int[] startValue, BitAlgorithm algorithm, BitProblem problem)
        {
            float[] results = new float[expSteps+1];
            results[0] = 0;
            bool fail = false;
            for (int k = 1; k < expSteps+1; k++)
            {
                int[] stepStartVal = Utility.CloneBitArrayPart(startValue, problemSize*(k)/expSteps);
                int[] stepResults = new int[expCount];
                for (int i = 0; i < expCount; i++)
                {
                algorithm.InitializeAlgorithm(stepStartVal.Length);
                    if (fail)
                    {
                        stepResults[i] = MAX_ITERATIONS;
                        continue;
                    }

                    int[] bestRes = stepStartVal;
                    for (int j = 1; j < MAX_ITERATIONS; j++)
                    {
                        int[] mutatedRes = algorithm.Mutate(bestRes);
                        if (problem.FitnessCompare(bestRes, mutatedRes))
                        {
                            bestRes = mutatedRes;
                        }


                        if (Utility.CountSetBits(bestRes) == bestRes.Length)
                        {
                            stepResults[i] = j;
                            //Debug.WriteLine($"Run {i} finished after iteration: {j}");
                            break;
                        }
                        if (j == MAX_ITERATIONS - 1)
                        {
                            Debug.WriteLine($"Failed to find a solution in time for problemsize {stepStartVal.Length}");
                            
                            stepResults[i] = MAX_ITERATIONS;
                            fail = true;
                            
                        }
                    }
                    
                    
                }
                results[k] = (float)stepResults.Sum() / expCount;
                Debug.WriteLine($"For problemsize {stepStartVal.Length} Average: {results[k]}");
            }
            return results;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="selectedProblem"></param>
        /// <returns>A new BitAlgorithm object of given index</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private BitAlgorithm GetAlgorithm(int index, BitProblem selectedProblem)
        {
            switch (index)
            {
                case 0:
                    return new BitOneOneEAAlgo();
                case 1:
                    return new BitRLSAlgo();
                case 2:
                    return new BitMMASAlgo(selectedProblem);
                default:
                    throw new IndexOutOfRangeException($"No algorithm with index: {index}");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns>A new BitProblem object of given index</returns>
        /// <exception cref="IndexOutOfRangeException"></exception>
        private BitProblem GetProblem(int index)
        {
            switch (index)
            {
                case 0:
                    return new MaxOnesProblem();
                case 1:
                    return new LeadingOnesProblem();
                default:
                    throw new IndexOutOfRangeException($"No algorithm with index: {index}");
            }
        }
    }
}
