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
        public ActionResult<IEnumerable<int>> BitstringRun(int maxProblemSize, int algorithmI, int problemI)
        {
            if (maxProblemSize<=0 || maxProblemSize>BitStringSimulation.MAX_PROBLEM_SIZE) 
            {
                return BadRequest($"N must be between 1 and {BitStringSimulation.MAX_PROBLEM_SIZE}");
            }
            if (algorithmI <= 0 || algorithmI > MathF.Pow(2, BitStringSimulation.ALGORITHM_COUNT) - 1)
            {
                return BadRequest("Invalid algorithm selected");
            }
            if (problemI < 0 || problemI >= BitStringSimulation.PROBLEM_COUNT)
            {
                return BadRequest($"Algorithm index must be between 0 and {BitStringSimulation.PROBLEM_COUNT}");
            }
            simulation.SetParametersForDetailed(maxProblemSize, algorithmI, problemI);
            int[][][]? result = simulation.HandleDetailedExperiment();
            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(result);
        }

        [HttpGet("BitstringExp")]
        public ActionResult<IEnumerable<int>> BitstringExp(int maxProblemSize, int expCount, int algorithmI, int problemI)
        {
            if (maxProblemSize <= 0 || maxProblemSize > BitStringSimulation.MAX_PROBLEM_SIZE)
            {
                return BadRequest($"N must be between 1 and {BitStringSimulation.MAX_PROBLEM_SIZE}");
            }
            if (expCount <= 0 || expCount > BitStringSimulation.MAX_PROBLEM_SIZE)
            {
                return BadRequest($"N must be between 1 and {BitStringSimulation.MAX_PROBLEM_SIZE}");
            }
            if (algorithmI <= 0 || algorithmI > MathF.Pow(2, BitStringSimulation.ALGORITHM_COUNT) - 1)
            {
                return BadRequest("Invalid algorithm selected");
            }
            if (problemI < 0 || problemI >= BitStringSimulation.PROBLEM_COUNT)
            {
                return BadRequest($"Algorithm index must be between 0 and {BitStringSimulation.PROBLEM_COUNT}");
            }
            simulation.SetParametersForDetailed(maxProblemSize, algorithmI, problemI);
            object? result = simulation.HandleDetailedExperiment();
            if (result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(result);
        }
    }   
}
