namespace API.Classes
{
    /// <summary>
    /// A class containing public static helper functions.
    /// </summary>
    public class Utility
    {

        /// <summary>
        /// Counts the number of set bits in an integer.
        /// </summary>
        /// <param name="n">The integer to count set bits in.</param>
        /// <returns>The number of set bits.</returns>
        public static int CountSetBits(int n)
        {
            int count = 0;
            while (n > 0)
            {
                count += n & 1;
                n >>= 1;
            }
            return count;
        }

    }
}
