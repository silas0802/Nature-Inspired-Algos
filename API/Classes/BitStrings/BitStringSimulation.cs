using System.Diagnostics;

namespace API.Classes.BitStrings
{
    public class BitStringSimulation
    {
        public const int MAX_N = 64;
        public const int ALGORITHM_COUNT = 2;
        public const int PROBLEM_COUNT = 2;
        public const int MAX_ITERATIONS = 1000;
        private int N;
        private int algorithmI;
        private int problemI;
        public int[][][]? result { get; private set; }


        /// <summary>
        /// Sets the parameters for the simulation.
        /// </summary>
        /// <param name="N">The length of the bit string.</param>
        /// <param name="algorithmI">The algorithm indexes, each bit corresponds to an algorithm.</param>
        /// <param name="problemI">The problem index.</param>
        public void SetParameters(int N, int algorithmI, int problemI)
        {
            this.N = N;
            this.algorithmI = algorithmI;
            this.problemI = problemI;
        }
        /// <summary>
        /// Validates the input parameters.
        /// </summary>
        /// <returns>True if the input parameters are valid, otherwise false.</returns>
        private bool ValidateInput()
        {
            if (N <= 0 || N > MAX_N)
            {
                return false;
            }
            if (algorithmI <= 0 || algorithmI > MathF.Pow(2, ALGORITHM_COUNT) - 1)
            {
                return false;
            }
            if (problemI < 0 || problemI >= PROBLEM_COUNT)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Runs the bit string simulation. Result is stored in this.result
        /// </summary>
        public void HandleSimulations()
        {
            if (!ValidateInput())
            {
                Console.WriteLine("Invalid input");
                result = null;
                return;
            }
            int bitstring = algorithmI;
            result = new int[Utility.CountSetBits((ulong)algorithmI)][][];

            int currentAlgo = 0;
            BitProblem selectedProblem;
            switch (problemI)
            {
                case 0:
                    selectedProblem = new MaxOnesProblem();
                    break;
                case 1:
                    selectedProblem = new LeadingOnesProblem();
                    break;
                default:
                    result = null;
                    Console.WriteLine("Failed to select problem");
                    return;
            }
            int[] startValue = Utility.InitializeRandomBinaryString(N);
            for (int i = 0; i < ALGORITHM_COUNT; i++)
            {
                if ((algorithmI & 1 << i) != 0)
                {
                    switch (i)
                    {
                        case 0:
                            result[currentAlgo] = RunSimulationPreset(startValue, new OneOneEAAlgo(), selectedProblem);
                            currentAlgo++;
                            break;
                        case 1:
                            result[currentAlgo] = RunSimulationPreset(startValue, new RLSAlgo(), selectedProblem);
                            currentAlgo++;
                            break;
                        default:
                            break;
                    }
                }

            }

        }
        private int[][] RunSimulationPreset(int[] startValue, BitAlgorithm algorithm, BitProblem problem)
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
                if (Utility.CountSetBits(result[result.Count - 1]) == N)
                {
                    break;
                }
            }

            return result.ToArray();
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
