using Microsoft.AspNetCore.Mvc;

namespace Yagohf.Gympass.RaceAnalyser.UnitTests.Api.Infrastructure
{
    public class InfrastructureTestsController : Controller
    {
        [HttpGet]
        public IActionResult Get(int id)
        {
            return Ok(id);
        }      
    }
}
