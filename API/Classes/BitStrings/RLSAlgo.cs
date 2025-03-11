using System;

public class RLSAlgo : BitAlgorithm
{
    static Random random = new Random();

    public override int[] Mutate(int[] original)
    {
        int[] mutated = new int[original.Length];
        int flippedIndex = random.Next(original.Length);
        for (int i = 0; i < original.Length; i++)
        {
            mutated[i] = original[i];
            if (i == flippedIndex) mutated[i] = 1 - mutated[i];

        }

        return mutated;
    }

    public override void ResetAlgorithm()
    {
        return;
    }
}
