using API.Classes.Generic;

namespace API.Classes.TSP
{
    public class TSPOneOneEAAlgo : TSPAlgorithm
    {
        static Random random = new Random();

        public override int[] Mutate(int[] original)
        {
            //Select 2-opt or 3-opt operator with probablity 0.5 respectively as mutation operator

            int[] mutated = random.Next(2) == 0 ? Utility.TSPOpt2(original) : Utility.TSPOpt3(original);

            //If the mutated solution is better than the original, return it, otherwise return the original
            return Utility.TSPCompare(nodes, mutated, original) ? mutated : original;

        }

        
    }
}
