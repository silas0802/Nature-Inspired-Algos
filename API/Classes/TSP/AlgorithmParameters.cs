namespace API.Classes.TSP
{
    public class AlgorithmParameters
    {
        public int problemSize;
        public int iterations;
        public int algorithmI;
        public int ExpCount;
        public int ExpSteps;
        public float alpha;
        public float beta;
        public AlgorithmParameters(int problemSize, int iterations, int algorithmI, float alpha, float beta)
        {
            this.problemSize = problemSize;
            this.iterations = iterations;
            this.algorithmI = algorithmI;
            this.alpha = alpha;
            this.beta = beta;
        }
        public AlgorithmParameters(int problemSize, int iterations, int algorithmI, int expCount, int expSteps, float alpha, float beta)
            : this(problemSize, iterations, algorithmI, alpha, beta)
        {
            this.ExpCount = expCount;
            this.ExpSteps = expSteps;
        }
    }
}
