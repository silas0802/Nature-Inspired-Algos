using API.Classes.Generic;
using System.Numerics;

namespace API.Classes.TSP
{
    public class AlgorithmParameters
    {
        public int problemSize;
        public int iterations;
        public int algorithmI;
        public int expCount;
        public int expSteps;
        public float alpha;
        public float beta;
        public Vector2[] nodes;

        public AlgorithmParameters(float[][] nodes, int iterations, int algorithmI, float alpha, float beta)
        {
            this.nodes = Utility.ConvertFloatArrayToVectors(nodes);
            this.iterations = iterations;
            this.algorithmI = algorithmI;
            this.alpha = alpha;
            this.beta = beta;
        }
        public AlgorithmParameters(int problemSize, int iterations, int algorithmI, float alpha, float beta)
        {
            this.problemSize = problemSize;
            this.iterations = iterations;
            this.algorithmI = algorithmI;
            this.alpha = alpha;
            this.beta = beta;
            this.nodes = null!;
        }
        public AlgorithmParameters(int problemSize, int iterations, int algorithmI, int expCount, int expSteps, float alpha, float beta)
            : this(problemSize, iterations, algorithmI, alpha, beta)
        {
            this.expCount = expCount;
            this.expSteps = expSteps;
        }
    }
}
