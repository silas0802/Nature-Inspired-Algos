using API.Classes.TSP;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TSPController : ControllerBase
    {
        TSPSimulation simulation = new TSPSimulation();

        [HttpGet("TSPRun")]
        public ActionResult<TSPRunResult> TSPRun(int problemSize, int algorithmI)
        {
            if (problemSize <= 0 || problemSize > TSPSimulation.MAX_PROBLEM_SIZE)
            {
                return BadRequest($"N must be between 1 and {TSPSimulation.MAX_PROBLEM_SIZE}");
            }
            if (algorithmI <= 0 || algorithmI > MathF.Pow(2, TSPSimulation.ALGORITHM_COUNT) - 1)
            {
                return BadRequest("Invalid algorithm(s) selected");
            }
            
            simulation.SetParametersForDetailed(problemSize, algorithmI);
            (float[][], int[][][], float[][])? result = simulation.RunDetailedExperiment();
            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(new TSPRunResult(result.Value.Item1,result.Value.Item2, result.Value.Item3));
        }

        [HttpGet("TSPExp")]
        public ActionResult<TSPExpResult> TSPExp(int maxProblemSize, int expCount, int expSteps, int algorithmI)
        {
            if (maxProblemSize <= 0 || maxProblemSize > TSPSimulation.MAX_PROBLEM_SIZE)
            {
                return BadRequest($"N must be between 1 and {TSPSimulation.MAX_PROBLEM_SIZE}");
            }
            if (expCount <= 0 || expCount > TSPSimulation.MAX_EXPERIMENT_COUNT)
            {
                return BadRequest($"experiment count must be between 1 and {TSPSimulation.MAX_EXPERIMENT_COUNT} \nbut was {expCount}");
            }
            if (expSteps <= 0 || expSteps > TSPSimulation.MAX_EXPERIMENT_STEPS)
            {
                return BadRequest($"experiment steps must be between 1 and {TSPSimulation.MAX_EXPERIMENT_STEPS} \nbut was {expSteps}");
            }
            if (algorithmI <= 0 || algorithmI > MathF.Pow(2, TSPSimulation.ALGORITHM_COUNT) - 1)
            {
                return BadRequest("Invalid algorithm(s) selected");
            }
            
            simulation.SetParametersForMultiExperiment(maxProblemSize, expCount, expSteps, algorithmI);
            float[][]? result = simulation.RunComparisonExperiment();

            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(new TSPExpResult(result));
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
