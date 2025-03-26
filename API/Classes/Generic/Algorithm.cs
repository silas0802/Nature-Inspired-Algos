namespace API.Classes.Generic
{
    public abstract class Algorithm
    {
        public abstract void InitializeAlgorithm(int problemSize);
        public abstract int[] Mutate(int[] original);

    }
}
