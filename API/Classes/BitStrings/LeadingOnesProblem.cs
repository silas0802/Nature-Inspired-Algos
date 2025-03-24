using API.Classes;
using System;

public class LeadingOnesProblem : BitProblem
{
    public override int EvaluateFitness(int[] bitstring)
    {
        return CountLeadingOnes(bitstring);
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
