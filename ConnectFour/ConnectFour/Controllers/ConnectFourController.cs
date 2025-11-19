using ConnectFour.Models;
using ConnectFour.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ConnectFour.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConnectFourController(IConnectFourService connectFourService) : ControllerBase
{

    [HttpPost("drop")]
    public async Task<ActionResult<string>> DropPiece(DropedPiece request)
    {
        var result = await connectFourService.PlacePiece(request);
        return Ok(result);
    }
}
