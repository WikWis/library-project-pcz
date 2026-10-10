using Microsoft.AspNetCore.Mvc;

namespace main.Controllers;

[ApiController]
[Route("/")]
public class TestController : ControllerBase
{
    [HttpGet]
    public Dictionary<string, string> Get()
    {
        return new Dictionary<string, string>
        {
            { "Edyta Jarząbek", "Sher" },
            { "Jakub Siwek", "polpop" },
            { "Wiktor Wiśniewski", "WikWis" }
        };
    }
}