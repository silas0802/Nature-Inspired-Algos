using System;

public abstract class BitProblem
{
	/// <summary>
	/// Compares the fitness of 2 solutions.
	/// </summary>
	/// <param name="current"></param>
	/// <param name="mutated"></param>
	/// <returns>True if mutated is better than current</returns>
	public abstract bool FitnessCompare(int[] current, int[] mutated);
}
