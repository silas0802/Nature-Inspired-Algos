using System.Diagnostics;

namespace API.Classes.BitStrings
{
    public class BitStringSimulation
    {
        public const int MAX_PROBLEM_SIZE = 1000;
        public const int MAX_EXPERIMENT_COUNT = 100;
        public const int ALGORITHM_COUNT = 2;
        public const int PROBLEM_COUNT = 2;
        public const int MAX_ITERATIONS = 1000;
        public int problemSize 
        { 
            get => problemSize; 
            private set 
            {
                if (value > MAX_PROBLEM_SIZE || value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                problemSize = value;
            } 
        }
        public int algorithmI
        {
            get => algorithmI;
            private set
            {
                if (value > MathF.Pow(2, ALGORITHM_COUNT) - 1 || value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                algorithmI = value;
            }
        }
        public int problemI
        {
            get => problemI;
            private set
            {
                if (value > PROBLEM_COUNT || value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                problemI = value;
            }
        }
        public int expCount
        {
            get => expCount;
            private set
            {
                if (value > MAX_EXPERIMENT_COUNT || value <= 1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                expCount = value;
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
        /// Runs the bit string simulation. Result is stored in this.result
        /// </summary>
        public object[] HandleDetailedExperiment()
        {
            
            int bitstring = algorithmI;
            object[] result = new object[Utility.CountSetBits((ulong)algorithmI)];

            int currentAlgo = 0;
            BitProblem selectedProblem = GetProblem(problemI);
            
            int[] startValue = Utility.InitializeRandomBinaryString(problemSize);
            for (int i = 0; i < ALGORITHM_COUNT; i++)
            {
                if ((algorithmI & 1 << i) != 0)
                {
                    result[currentAlgo] = RunDetailedSimulationPreset(startValue, GetAlgorithm(i), selectedProblem);
                    currentAlgo++;
                }

            }
            return result;

        }
        private int[][] RunDetailedSimulationPreset(int[] startValue, BitAlgorithm algorithm, BitProblem problem)
        {
            List<int[]> result = new List<int[]>();
            result.Add(startValue);
            int maxIterations = 1000;

            for (int i = 0; i < maxIterations; i++)
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
                Debug.WriteLine($"Iteration {i}: {Utility.CountSetBits(result[result.Count - 1])}");
                if (Utility.CountSetBits(result[result.Count - 1]) == problemSize)
                {
                    break;
                }
            }

            return result.ToArray();
        }

        private BitAlgorithm GetAlgorithm(int index)
        {
            switch (index)
            {
                case 0:
                    return new OneOneEAAlgo();
                case 1:
                    return new RLSAlgo();
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
                    throw new ArgumentOutOfRangeException($"No algorithm with index: {index}");
            }
        }

        /// <summary>
        /// Generates a random result based on the specified parameters. Used for testing.
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
