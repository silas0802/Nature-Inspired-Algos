using API.Classes.Generic;
using System.Numerics;

namespace API.Classes.TSP
{
    public class TSPSimAnnealAlgo : TSPAlgorithm
    {
        Random random = new Random();
        double temperature = 0f;
        int m = 0;
        float c = 0.0001f;
        float alpha = 0;
        public override void InitializeAlgorithm(Vector2[] nodes)
        {
            base.InitializeAlgorithm(nodes);
            m = nodes.Length*20;
            temperature = m*m*m;
            alpha = 1 - 1 / (c * m * m);
            
        }
        public override int[] Mutate(int[] original)
        {
            int[] mutated = Utility.TSPOpt2(original);
            if ( Utility.TSPCompare(nodes, mutated, original)) // If mutated is better, take that
            {
                temperature *= alpha;
                return mutated;
            }
            else
            {   // If mutated is worse, take it with a probability
                float distDiff = Utility.TSPCalculateDistance(nodes, original) - Utility.TSPCalculateDistance(nodes, mutated);
                float lossProb = MathF.Exp( distDiff / (float)temperature);
                temperature *= alpha;
                return random.NextDouble() < lossProb ? mutated : original;
            }
        }
    }
}
