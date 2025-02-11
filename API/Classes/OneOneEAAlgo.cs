using System;

public class OneOneEAAlgo : BitAlgorithm
{
    static Random random = new Random();

    static void Main()
    {
        int N = 10; // Length of the binary string
        int[] S = InitializeRandomBinaryString(N);
        int maxIterations = 1000;

        for (int iteration = 0; iteration < maxIterations; iteration++)
        {
            int[] SPrime = Mutate(S);
            if (CountOnes(SPrime) > CountOnes(S))
            {
                S = SPrime;
            }
        }

        Console.WriteLine("Final solution: " + string.Join("", S));
    }

    static int[] InitializeRandomBinaryString(int length)
    {
        int[] binaryString = new int[length];
        for (int i = 0; i < length; i++)
        {
            binaryString[i] = random.Next(2); // Randomly 0 or 1
        }
        return binaryString;
    }

    static int[] Mutate(int[] S)
    {
        int[] SPrime = (int[])S.Clone();
        for (int i = 0; i < S.Length; i++)
        {
            if (random.NextDouble() < 1.0 / S.Length)
            {
                SPrime[i] = 1 - SPrime[i]; // Flip the bit
            }
        }
        return SPrime;
    }

    static int CountOnes(int[] binaryString)
    {
        int count = 0;
        foreach (int bit in binaryString)
        {
            if (bit == 1)
            {
                count++;
            }
        }
        return count;
    }
}

