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
        public ActionResult<int[][][]> TSPRun(int problemSize, int algorithmI)
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
            int[][][]? result = simulation.RunExperiment(simulation.RunDetailedSimulation);
            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(result);
        }

        [HttpGet("TSPExp")]
        public ActionResult<float[][]> TSPExp(int maxProblemSize, int expCount, int expSteps, int algorithmI)
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
            float[][]? result = simulation.RunExperiment(simulation.RunMultiSimulation);

            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(result);
        }
    }
}
