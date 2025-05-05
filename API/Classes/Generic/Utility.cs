using System;
using System.Diagnostics;
using System.Numerics;

namespace API.Classes.Generic
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
        public static float[][] ConvertVectorsToFloatArray(Vector2[] vector)
        {
            float[][] result = new float[vector.Length][];
            for (int i = 0; i < vector.Length; i++)
            {
                result[i] = new float[2];
                result[i][0] = vector[i].X;
                result[i][1] = vector[i].Y;
            }
            return result;
        }
        public static Vector2[] ConvertFloatArrayToVectors(float[][] array)
        {
            Vector2[] result = new Vector2[array.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new Vector2(array[i][0], array[i][1]);
            }
            return result;
        }
        public static int[] RandomTSPSolution(int problemSize)
        {
            List<int> options = new List<int>();
            options.AddRange(Enumerable.Range(0, problemSize));
            int[] result = new int[problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                int index = random.Next(options.Count);
                result[i] = options[index];
                options.RemoveAt(index);
            }
            return result;
        }
        public static Vector2[] GenerateRandomGraph(int problemSize, int maxWidth, int maxHeight)
        {
            Vector2[] result = new Vector2[problemSize];
            for (int i = 0; i < problemSize; i++)
            {
                result[i] = new Vector2(random.Next(maxWidth), random.Next(maxHeight));
            }
            return result;
        }

        public static float TSPCalculateDistance(Vector2[] nodes, int[] solution)
        {
            float sum = 0;
            for (int i = 0; i < solution.Length; i++)
            {
                Vector2 node1 = nodes[solution[i]];
                Vector2 node2 = nodes[solution[(i + 1) % solution.Length]];
                sum += Vector2.Distance(node1, node2);
            }
            return sum;
        }
        public static bool TSPCompare(Vector2[] nodes, int[] mutated, int[] original)
        {
            return TSPCalculateDistance(nodes, mutated) < TSPCalculateDistance(nodes, original);
        }

        public static int[] TSPOpt2(int[] original)
        {
            List<int> path = original.ToList();
            int length = original.Length;
            int i1 = random.Next(0, length);
            int i2 = random.Next(0, length);
            int subStart = Math.Min(i1, i2);
            int subEnd = Math.Max(i1, i2);
            List<int> substring = path.GetRange(subStart, subEnd - subStart + 1); //Get substring
            substring.Reverse(); //Reverse substring
            path.RemoveRange(subStart, subEnd - subStart + 1);
            path.InsertRange(subStart, substring); //Insert reversed substring

            return path.ToArray();

        }
        public static int[] TSPOpt3(int[] original)
        {
            List<int> path = original.ToList();
            int length = original.Length;
            int i1 = random.Next(0, length);
            int i2 = random.Next(0, length);
            int subStart = Math.Min(i1, i2);
            int subEnd = Math.Max(i1, i2);
            List<int> substring = path.GetRange(subStart, subEnd - subStart + 1); //Get substring
            path.RemoveRange(subStart, subEnd - subStart + 1);//Pick out substring
            path.InsertRange(random.Next(0, path.Count), substring); //Insert substring somewhere randomly

            return path.ToArray();
        }

        public static bool ValidateTSPSolution(int[] solution)
        {
            int problemSize = solution.Length;
            for (int i = 0; i < problemSize; i++)
            {
                if (!solution.Contains(i))
                {
                    return false;
                }
            }
            return true;
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
            int algorithmCount = CountSetBits((ulong)algorithmI);
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
