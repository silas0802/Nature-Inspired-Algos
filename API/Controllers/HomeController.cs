using API.Classes.BitStrings;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        
        BitStringSimulation simulation = new BitStringSimulation();

        [HttpGet("BitstringRun")]
        public ActionResult<IEnumerable<int>> BitstringRun(int N, int algorithmI, int problemI)
        {
            if (N<=0 || N>BitStringSimulation.MAX_N) 
            {
                return BadRequest($"N must be between 1 and {BitStringSimulation.MAX_N}");
            }
            if (algorithmI <= 0 || algorithmI > MathF.Pow(2, BitStringSimulation.ALGORITHM_COUNT) - 1)
            {
                return BadRequest("Invalid algorithm selected");
            }
            if (problemI < 0 || problemI >= BitStringSimulation.PROBLEM_COUNT)
            {
                return BadRequest($"Algorithm index must be between 0 and {BitStringSimulation.PROBLEM_COUNT}");
            }
            simulation.SetParameters(N, algorithmI, problemI);
            simulation.HandleSimulations();
            if (simulation.result == null)
            {
                return BadRequest("Simulation failed");
            }
            return Ok(simulation.result);
        }
    }
}
