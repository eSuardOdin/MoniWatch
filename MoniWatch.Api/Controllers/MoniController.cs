using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MoniWatch.Entities;
using MoniWatch.DataContext;



namespace MoniWatch.Api.Controllers;

[ApiController]
[Route("moni")]
public class MoniController : ControllerBase
{
    private readonly ILogger<MoniController> _logger;
    public MoniController(ILogger<MoniController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name="GetMoni")]
    public async Task<ActionResult<Moni>> GetMoni(string moniLogin)
    {
        using (MoniWatchDbContext db = new())
        {
            Moni moni = await db.Monies.Where(m => m.MoniLogin == moniLogin).FirstOrDefaultAsync();
            if (moni is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(moni);
            }
        }
    }
}