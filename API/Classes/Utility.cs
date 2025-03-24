using System;

namespace API.Classes
{
    /// <summary>
    /// A class containing public static helper functions.
    /// </summary>
    public class Utility
    {
        static Random random = new Random();

        /// <summary>
        /// Counts the number of set bits in an integer.
        /// </summary>
        /// <param name="n">The integer to count set bits in.</param>
        /// <returns>The number of set bits.</returns>
        public static int CountSetBits(ulong n)
        {
            ulong count = 0;
            while (n > 0)
            {
                count += n & 1;
                n >>= 1;
            }
            return (int)count;
        }
        public static int CountSetBits(int[] bitarray)
        {
            int count = 0;
            for (int i = 0; i < bitarray.Length; i++)
            {
                count += bitarray[i];
            }
            return count;
        }
        public static int[] InitializeRandomBinaryString(int length)
        {
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = random.Next(0, 2);
            }
            return result;
        }

        public static int[] CloneBitArrayPart(int[] array, int length)
        {
            int[] result = new int[length];
            for (int i = 0; i < length; i++)
            {
                result[i] = array[i];
            }
            return result;
        }

        public static string DisplayAnyList<T>(T[] list)
        {
            return DisplayAnyListRecursive(list, 0);
        }

        private static string DisplayAnyListRecursive<T>(T[] list, int depth)
        {
            string indent = new string(' ', depth * 2);
            string result = "";
            foreach (var item in list)
            {
                if (item is Array subArray)
                {
                    if (subArray is T[] subArrayT)
                    {
                        result += $"{indent}[\n{DisplayAnyListRecursive(subArrayT, depth + 1)}\n{indent}]\n";
                    }
                    else
                    {
                        result += $"{indent}[{DisplayAnyListRecursive(subArray.Cast<object>().ToArray(), depth + 1)}]\n";
                    }
                }
                else
                {
                    result += $"{item} ";
                }
            }
            return result;
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
