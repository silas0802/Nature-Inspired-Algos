using API.Classes.Generic;

namespace API.Classes.TSP
{
    public class TSPOneOneEAAlgo : TSPAlgorithm
    {
        static Random random = new Random();

        public override void InitializeAlgorithm(int problemSize)
        {
            return;
        }

        public override int[] Mutate(int[] original)
        {
            //Select 2-opt or 3-opt operator with probablity 0.5 respectively as mutation operator
            return random.Next(2) == 0 ? Utility.TSPOpt2(original) : Utility.TSPOpt3(original);
            
        }

        
    }
}
