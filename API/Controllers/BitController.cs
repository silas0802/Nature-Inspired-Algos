using API.Classes.BitStrings;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BitController : ControllerBase
    {
        
        BitStringSimulation simulation = new BitStringSimulation();

        [HttpGet("BitstringRun")]
        public ActionResult<int[][][]> BitstringRun(int problemSize, int algorithmI, int problemI)
        {
            if (problemSize<=0 || problemSize>BitStringSimulation.MAX_PROBLEM_SIZE) 
            {
                return BadRequest($"N must be between 1 and {BitStringSimulation.MAX_PROBLEM_SIZE}");
            }
            if (algorithmI <= 0 || algorithmI > MathF.Pow(2, BitStringSimulation.ALGORITHM_COUNT) - 1)
            {
                return BadRequest("Invalid algorithm(s) selected");
            }
            if (problemI < 0 || problemI >= BitStringSimulation.PROBLEM_COUNT)
            {
                return BadRequest($"Problem index must be between 0 and {BitStringSimulation.PROBLEM_COUNT}");
            }
            simulation.SetParametersForDetailed(problemSize, algorithmI, problemI);
            int[][][]? result = simulation.RunExperiment(simulation.RunDetailedSimulation);
            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(result);
        }

        [HttpGet("BitstringExp")]
        public ActionResult<float[][]> BitstringExp(int maxProblemSize, int expCount, int expSteps,int algorithmI, int problemI)
        {
            if (maxProblemSize <= 0 || maxProblemSize > BitStringSimulation.MAX_PROBLEM_SIZE)
            {
                return BadRequest($"N must be between 1 and {BitStringSimulation.MAX_PROBLEM_SIZE}");
            }
            if (expCount <= 0 || expCount > BitStringSimulation.MAX_EXPERIMENT_COUNT)
            {
                return BadRequest($"experiment count must be between 1 and {BitStringSimulation.MAX_EXPERIMENT_COUNT} \nbut was {expCount}");
            }
            if (expSteps <= 0 || expSteps > BitStringSimulation.MAX_EXPERIMENT_STEPS)
            {
                return BadRequest($"experiment steps must be between 1 and {BitStringSimulation.MAX_EXPERIMENT_STEPS} \nbut was {expSteps}");
            }
            if (algorithmI <= 0 || algorithmI > MathF.Pow(2, BitStringSimulation.ALGORITHM_COUNT) - 1)
            {
                return BadRequest("Invalid algorithm(s) selected");
            }
            if (problemI < 0 || problemI >= BitStringSimulation.PROBLEM_COUNT)
            {
                return BadRequest($"Problem index must be between 0 and {BitStringSimulation.PROBLEM_COUNT}");
            }
            simulation.SetParametersForMultiExperiment(maxProblemSize, expCount, expSteps, algorithmI, problemI);
            float[][]? result = simulation.RunExperiment(simulation.RunMultiSimulation);

            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(result);
        }
    }   
}
