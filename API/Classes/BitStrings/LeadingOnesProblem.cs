using System;

public class LeadingOnesProblem : BitProblem
{
    public override bool FitnessCompare(int[] current, int[] mutated)
    {
        return CountLeadingOnes(mutated) > CountLeadingOnes(current);
    }

    private int CountLeadingOnes(int[] bitArray)
    {
        int count = 0;
        for (int i = 0; i < bitArray.Length; i++)
        {
            if (bitArray[i] == 1) count++;
            else break;
        }
        return count;
    }
}
