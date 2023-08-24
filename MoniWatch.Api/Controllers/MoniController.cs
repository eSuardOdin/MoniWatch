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


    /// <summary>
    /// Returns a Moni with matching login and password</br>
    /// Route URL: /root/moni/GetMoni?moniLogin={login}&moniPwd={pwd}
    /// </summary>
    /// <param name="moniLogin">The Login to search in db</param>
    /// <param name="moniPwd">Password that will be tested against hashed password in db</param>
    /// <returns>Not found or moni object</returns>
    [HttpGet]
    [Route("GetMoni")]
    public async Task<ActionResult<Moni>> GetMoni(string moniLogin, string moniPwd)
    {
        using (MoniWatchDbContext db = new())
        {
            Moni moni = await db.Monies.Where(
                m => m.MoniLogin == moniLogin)
                .FirstOrDefaultAsync();

            if (moni is null || !BcryptNet.Verify(moniPwd, moni.MoniPwd))
            {
                return NotFound();
            }
            else
            {
                return Ok(moni);
            }
        }
    }

    /// <summary>
    /// Adds a Moni to database with an hashed password
    /// </summary>
    /// <param name="moni">Moni object added, will be passed in request as JSON</param>
    /// <returns>The Moni created</returns>
    [HttpPost(Name="PostMoni")]
    public async Task<ActionResult<Moni>> PostMoni([FromBody] Moni moni)
    {
        // Pwd encrypt
        moni.MoniPwd = BcryptNet.HashPassword(moni.MoniPwd);
        using (MoniWatchDbContext db = new())
        {
            if(moni is null)
            {
                return BadRequest("Bad data provided");
            }
            db.Monies.Add(moni);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMoni), new {moniLogin = moni.MoniLogin}, moni);

        } 
    }
}