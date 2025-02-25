using API.Classes;
using System;
using System.Diagnostics;

public class OneOneEAAlgo : BitAlgorithm
{
    static Random random = new Random();
    public static ulong[] Run(int N)
    {
        List<ulong> result = new List<ulong>();
        result.Add(Utility.InitializeRandomBinaryString(N));
        int maxIterations = 1000;

        for (int i = 0; i < maxIterations; i++)
        {
            ulong bestRes = result[result.Count - 1];
            ulong mutatedRes = Mutate(bestRes, N);
            if (Utility.CountSetBits(mutatedRes) > Utility.CountSetBits(bestRes))
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

    

    static ulong Mutate(ulong original, int length)
    {
        ulong mutated = original;
        for (int i = 0; i < length; i++)
        {
            if (random.NextDouble() < 1.0 / length)
            {
                mutated = mutated ^ (1UL << i);
            }
        }
        ulong mask = (1UL << length) - 1;
        mutated &= mask;
        return mutated;
    }

    
}

