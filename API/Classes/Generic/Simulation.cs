using System.Diagnostics;

namespace API.Classes.Generic
{
    public class Simulation
    {
        public const int MAX_PROBLEM_SIZE = 5000;
        public const int MAX_ITERATIONS = 100000;
        public const int MAX_EXPERIMENT_COUNT = 200;
        public const int MAX_EXPERIMENT_STEPS = 100;
        public const int ALGORITHM_COUNT = 3;
        public const int PROBLEM_COUNT = 2;
        private int _problemSize;
        private int _algorithmI;
        private int _expCount;
        private int _expSteps;

        public int problemSize
        {
            get => _problemSize;
            protected set
            {
                if (value > MAX_PROBLEM_SIZE || value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _problemSize = value;
            }
        }
        public int algorithmI
        {
            get => _algorithmI;
            protected set
            {
                if (value > MathF.Pow(2, ALGORITHM_COUNT) - 1 || value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _algorithmI = value;
            }
        }
        
        public int expCount
        {
            get => _expCount;
            protected set
            {
                if (value > MAX_EXPERIMENT_COUNT || value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _expCount = value;
            }
        }
        public int expSteps
        {
            get => _expSteps;
            protected set
            {
                if (value > MAX_EXPERIMENT_STEPS || value <= 0)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _expSteps = value;
            }
        }


        


        
    }
}
