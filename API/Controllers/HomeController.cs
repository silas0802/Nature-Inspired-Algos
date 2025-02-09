using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        const int MAX_N = 64;
        [HttpGet("BitstringRun")]
        public ActionResult<IEnumerable<int>> BitstringRun(int N, int algorithmI, int problemI)
        {
            if (N<=0 || N>MAX_N)
            {
                return BadRequest($"N must be between 1 and {MAX_N}");
            }
            if (algorithmI <= 0)
            {
                return BadRequest("No algorithm selected");
            }

            int algorithmCount = CountSetBits(algorithmI);
            ulong[][] result = new ulong[algorithmCount][]; //For each algorithm, a list of bitstrings (bitstrings being a list of bits)
            for (int i = 0; i < algorithmCount; i++)//For each algorithm
            {
                List<ulong> resultList = new List<ulong>();
                
                for (int j = 0; j < 10; j++)
                {
                    ulong bitstring = 0;
                    for (int k = 0; k < N; k++)
                    {
                        int val = Random.Shared.Next(2);
                        bitstring = bitstring | ((ulong)((uint)val) << k);
                    }
                    resultList.Add(bitstring);
                }
                result[i] = resultList.ToArray();
            }

            return Ok(result);
        }

        private int CountSetBits(int n)
        {
            int count = 0;
            while (n > 0)
            {
                count += n & 1;
                n >>= 1;
            }
            return count;
        }
    }
}
