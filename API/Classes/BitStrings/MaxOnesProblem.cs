using API.Classes;
using System;

public class MaxOnesProblem : BitProblem
{
    public override bool FitnessCompare(int[] current, int[] mutated)
    {
        return Utility.CountSetBits(mutated) > Utility.CountSetBits(current);
    }
}
