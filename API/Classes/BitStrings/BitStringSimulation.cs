using System.Diagnostics;

namespace API.Classes.BitStrings
{
    public class BitStringSimulation
    {
        public const int MAX_PROBLEM_SIZE = 1000;
        public const int MAX_EXPERIMENT_COUNT = 1000;
        public const int MAX_EXPERIMENT_STEPS = 30;
        public const int ALGORITHM_COUNT = 3;
        public const int PROBLEM_COUNT = 2;
        public const int MAX_ITERATIONS = 50000;
        private int _problemSize;
        private int _algorithmI;
        private int _problemI;
        private int _expCount;

        public int problemSize
        {
            get => _problemSize;
            private set
            {
                if (value > MAX_PROBLEM_SIZE || value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _problemSize = value;
            }
        }
        public int algorithmI
        {
            get => _algorithmI;
            private set
            {
                if (value > MathF.Pow(2, ALGORITHM_COUNT) - 1 || value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _algorithmI = value;
            }
        }
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
        public int expCount
        {
            get => _expCount;
            private set
            {
                if (value > MAX_EXPERIMENT_COUNT || value <= 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _expCount = value;
            }
        }


        /// <summary>
        /// Sets the parameters for the simulation.
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
        public void SetParametersForMultiExperiment(int problemSize, int expCount, int algorithmI, int problemI)
        {
            SetParametersForDetailed(problemSize, algorithmI, problemI);
            this.expCount = expCount;
        }
        

        /// <summary>
        /// Runs the experiment results for each algorithm.
        /// </summary>
        public T[] RunExperiment<T>(Func<int[],BitAlgorithm,BitProblem, T> simulation)
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
            Debug.WriteLine($"Experiment finished with start value: {{{string.Join(',',startValue)}}}");
            return result;

        }
        public int[][] RunDetailedSimulation(int[] startValue, BitAlgorithm algorithm, BitProblem problem)
        {
            List<int[]> result = new List<int[]>();
            result.Add(startValue);

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
        public float[] RunMultiSimulation(int[] startValue, BitAlgorithm algorithm, BitProblem problem)
        {
            float[] results = new float[MAX_EXPERIMENT_STEPS];
            for (int k = 0; k < MAX_EXPERIMENT_STEPS; k++)
            {
                int[] stepStartVal = Utility.CloneBitArrayPart(startValue, problemSize*(k+1)/MAX_EXPERIMENT_STEPS);
                int[] stepResults = new int[expCount];
                for (int i = 0; i < expCount; i++)
                {
                    int[] bestRes = stepStartVal;
                    for (int j = 1; j < MAX_ITERATIONS; j++)
                    {
                        int[] mutatedRes = algorithm.Mutate(bestRes);
                        if (problem.FitnessCompare(bestRes, mutatedRes))
                        {
                            bestRes = mutatedRes;
                        }


                        if (Utility.CountSetBits(bestRes) == problemSize)
                        {
                            stepResults[i] = j;
                            //Debug.WriteLine($"Run {i} finished after iteration: {j}");
                            break;
                        }
                        if (j == MAX_ITERATIONS - 1)
                        {
                            Debug.WriteLine($"Failed to find a solution in time");
                            stepResults[i] = MAX_ITERATIONS;
                        }
                    }
                    results[k] = (float)stepResults.Sum() / expCount;
                    Debug.WriteLine($"For problemsize {bestRes.Length} Average: {results[k]}");
                    algorithm.ResetAlgorithm();
                }
            }
            return results;
        }

        private BitAlgorithm GetAlgorithm(int index, BitProblem selectedProblem)
        {
            switch (index)
            {
                case 0:
                    return new OneOneEAAlgo();
                case 1:
                    return new RLSAlgo();
                case 2:
                    return new MMASAlgo(problemSize, selectedProblem);
                default:
                    throw new IndexOutOfRangeException($"No algorithm with index: {index}");
            }
        }
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

        /// <summary>
        /// !!!Deprecated!!! Generates a random result based on the specified parameters. Used for testing.
        /// </summary>
        /// <param name="N">The length of the bit string.</param>
        /// <param name="algorithmI">The algorithm index.</param>
        /// <param name="iterations">The number of iterations.</param>
        /// <returns>A jagged array of generated bit arrays.</returns>
        private int[][][] GenerateRandomResult(int N, int algorithmI, int iterations)
        {
            int algorithmCount = Utility.CountSetBits((ulong)algorithmI);
            int[][][] result = new int[algorithmCount][][]; //For each algorithm, a list of bitstrings (bitstrings being a list of bits)
            for (int i = 0; i < algorithmCount; i++)//For each algorithm
            {
                List<int[]> resultList = new List<int[]>();

                for (int j = 0; j < iterations; j++)
                {
                    int[] bitarray = new int[N];
                    for (int k = 0; k < N; k++)
                    {
                        bitarray[k] = Random.Shared.Next(2);
                    }
                    resultList.Add(bitarray);
                }
                result[i] = resultList.ToArray();
            }
            return result;
        }


    }
}
