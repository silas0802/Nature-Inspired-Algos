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

    }
}
