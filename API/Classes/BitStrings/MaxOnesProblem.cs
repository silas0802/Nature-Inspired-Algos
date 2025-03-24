using API.Classes;
using System;

public class MaxOnesProblem : BitProblem
{
    public override int EvaluateFitness(int[] bitstring)
    {
        return Utility.CountSetBits(bitstring);
    }
}
