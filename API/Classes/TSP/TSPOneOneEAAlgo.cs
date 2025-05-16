using API.Classes.Generic;
using System.Numerics;

namespace API.Classes.TSP
{
    public class TSPOneOneEAAlgo : TSPAlgorithm
    {
        static Random random = new Random();

        public override int[] Mutate(int[] original)
        {
            //Select 2-opt or 3-opt operator with probablity 0.5 respectively as mutation operator

            int[] mutated = random.Next(2) == 0 ? Utility.TSPOpt2(original) : Utility.BestTSPOfList(Utility.TSPOpt3(original),nodes);

            //If the mutated solution is at least as good as the original, return it, otherwise return the original
            return Utility.TSPCalculateDistance(nodes, mutated) <= Utility.TSPCalculateDistance(nodes, original) ? mutated : original;

        }


    }
}
