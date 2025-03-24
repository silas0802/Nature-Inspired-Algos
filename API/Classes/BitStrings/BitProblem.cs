using System;

public abstract class BitProblem
{
	/// <summary>
	/// Compares the fitness of 2 solutions.
	/// </summary>
	/// <param name="current"></param>
	/// <param name="mutated"></param>
	/// <returns>True if mutated is better than current</returns>
	public bool FitnessCompare(int[] current, int[] mutated)
	{
        return EvaluateFitness(mutated) > EvaluateFitness(current);
    }

	public abstract int EvaluateFitness(int[] bitstring);
}
