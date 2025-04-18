using API.Classes.Generic;
using System.Numerics;

namespace API.Classes.TSP
{
    public abstract class TSPAlgorithm : Algorithm
    {
        public Vector2[] nodes = [];
        public virtual void InitializeAlgorithm(Vector2[] nodes, AlgorithmParameters algorithmParameters)
        {
            this.nodes = nodes;
        }

    }
}
