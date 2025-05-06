using API.Classes.Generic;
using API.Classes.TSP;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TSPController : ControllerBase
    {
        TSPSimulation simulation = new TSPSimulation();

        [HttpPost("TSPRun")]
        public ActionResult<TSPRunResult> TSPRun([FromBody] TSPRunParameters parameters)
        {
            if ((parameters.ProblemSize <= 0 || parameters.ProblemSize > TSPSimulation.MAX_PROBLEM_SIZE) && parameters.Nodes == null)
            {
                return BadRequest($"N must be between 1 and {TSPSimulation.MAX_PROBLEM_SIZE}");
            }
            if (parameters.AlgorithmI <= 0 || parameters.AlgorithmI > MathF.Pow(2, TSPSimulation.ALGORITHM_COUNT) - 1)
            {
                return BadRequest("Invalid algorithm(s) selected");
            }
            if (parameters.Iterations <= 0 || parameters.Iterations > TSPSimulation.MAX_ITERATIONS)
            {
                return BadRequest($"Iterations must be between 0 and {TSPSimulation.MAX_ITERATIONS}");
            }
            if (parameters.Nodes != null)
            {
                Debug.WriteLine($"Nodes: {Utility.DisplayAnyList(parameters.Nodes)}");
                simulation.SetParametersForDetailed(new AlgorithmParameters(parameters.Nodes, parameters.Iterations, parameters.AlgorithmI, parameters.Alpha, parameters.Beta, parameters.CoolingRate));
            }
            else
            {

                simulation.SetParametersForDetailed(new AlgorithmParameters( parameters.ProblemSize, parameters.Iterations, parameters.AlgorithmI, parameters.Alpha, parameters.Beta, parameters.CoolingRate));
            }
            (float[][], int[][][], float[][])? result = simulation.RunDetailedExperiment();
            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(new TSPRunResult(result.Value.Item1,result.Value.Item2, result.Value.Item3));
        }

        [HttpPost("TSPExp")]
        public ActionResult<TSPExpResult> TSPExp([FromBody] TSPExpParameters parameters)
        {
            if (parameters.MaxProblemSize <= 0 || parameters.MaxProblemSize > TSPSimulation.MAX_PROBLEM_SIZE)
            {
                return BadRequest($"N must be between 1 and {TSPSimulation.MAX_PROBLEM_SIZE}");
            }
            if (parameters.ExpCount <= 0 || parameters.ExpCount > TSPSimulation.MAX_EXPERIMENT_COUNT)
            {
                return BadRequest($"experiment count must be between 1 and {TSPSimulation.MAX_EXPERIMENT_COUNT} \nbut was {parameters.ExpCount}");
            }
            if (parameters.ExpSteps <= 0 || parameters.ExpSteps > TSPSimulation.MAX_EXPERIMENT_STEPS)
            {
                return BadRequest($"experiment steps must be between 1 and {TSPSimulation.MAX_EXPERIMENT_STEPS} \nbut was {parameters.ExpSteps}");
            }
            if (parameters.AlgorithmI <= 0 || parameters.AlgorithmI > MathF.Pow(2, TSPSimulation.ALGORITHM_COUNT) - 1)
            {
                return BadRequest("Invalid algorithm(s) selected");
            }
            
            simulation.SetParametersForMultiExperiment(new AlgorithmParameters(parameters.MaxProblemSize, parameters.Iterations, parameters.AlgorithmI, parameters.ExpCount, parameters.ExpSteps, parameters.Alpha, parameters.Beta, parameters.CoolingRate));
            float[][]? result = simulation.RunComparisonExperiment();

            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(new TSPExpResult(result));
        }
        public class TSPRunParameters
        {
            public int ProblemSize { get; set; }
            public int AlgorithmI { get; set; }
            public int Iterations { get; set; }
            public float[][]? Nodes { get; set; } = null;
            public float Alpha { get; set; }
            public float Beta { get; set; }
            public int CoolingRate { get; set; }
        }
        public class TSPExpParameters
        {
            public int MaxProblemSize { get; set; }
            public int Iterations { get; set; }

            public int AlgorithmI { get; set; }
            public int ExpCount { get; set; }
            public int ExpSteps { get; set; }
            public float Alpha { get; set; }
            public float Beta { get; set; }
            public int CoolingRate { get; set; }


        }
        public class TSPRunResult
        {
            public float[][] nodes { get; set; }
            public int[][][] solutions { get; set; }

            public float[][] results { get; set; }

            public TSPRunResult(float[][] nodes, int[][][] solutions, float[][] results)
            {
                this.nodes = nodes;
                this.solutions = solutions;
                this.results = results;
            }
        }

        public class TSPExpResult
        {
            public float[][] results { get; set; }
            public TSPExpResult(float[][] results)
            {
                this.results = results;
            }
        }

        
    }
}
