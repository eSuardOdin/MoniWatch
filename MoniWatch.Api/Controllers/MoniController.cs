using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MoniWatch.Entities;
using MoniWatch.DataContext;



namespace MoniWatch.Api.Controllers

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
    public async Task<IActionResult> GetMoni(int id)
    {
        using (MoniDbContext db = new())
        {
            
        }
    }
}