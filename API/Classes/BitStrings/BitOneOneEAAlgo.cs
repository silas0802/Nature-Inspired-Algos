﻿namespace API.Classes.BitStrings
{
    public class BitOneOneEAAlgo : BitAlgorithm
    {
        static Random random = new Random();
        public override int[] Mutate(int[] original)
        {
            int[] mutated = new int[original.Length];
            for (int i = 0; i < original.Length; i++)
            {
                if (random.NextDouble() < 1.0 / original.Length)
                {
                    mutated[i] = 1 - original[i];
                }
                else
                {
                    mutated[i] = original[i];
                }
            }

            return mutated;
        }
        public override void InitializeAlgorithm(int problemSize)
        {
            return;
        }
    }
}


