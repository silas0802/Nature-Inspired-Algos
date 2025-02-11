namespace API
{
    public class BitStringSimulation
    {
        public const int MAX_N = 64; 
        public const int ALGORITHM_COUNT = 2;
        public const int PROBLEM_COUNT = 2;
        private int N;
        private int algorithmI;
        private int problemI;
        public ulong[][]? result { get; private set;}


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
        public void RunSimulation()
        {
            if (!ValidateInput())
            {
                Console.WriteLine("Invalid input");
                result = null;
                return;
            }

            result = GenerateRandomResult(N, algorithmI, 10);
            
        }

        /// <summary>
        /// Generates a random result based on the specified parameters.
        /// </summary>
        /// <param name="N">The length of the bit string.</param>
        /// <param name="algorithmI">The algorithm index.</param>
        /// <param name="iterations">The number of iterations.</param>
        /// <returns>A jagged array of ulong representing the generated bit strings.</returns>
        private ulong[][] GenerateRandomResult(int N, int algorithmI, int iterations)
        {
            int algorithmCount = Utility.CountSetBits(algorithmI);
            ulong[][] result = new ulong[algorithmCount][]; //For each algorithm, a list of bitstrings (bitstrings being a list of bits)
            for (int i = 0; i < algorithmCount; i++)//For each algorithm
            {
                List<ulong> resultList = new List<ulong>();

                for (int j = 0; j < iterations; j++)
                {
                    ulong bitstring = 0;
                    for (int k = 0; k < N; k++)
                    {
                        int val = Random.Shared.Next(2);
                        bitstring = bitstring | ((ulong)((uint)val) << k);
                    }
                    resultList.Add(bitstring);
                }
                result[i] = resultList.ToArray();
            }
            return result;
        }

        
    }
}
