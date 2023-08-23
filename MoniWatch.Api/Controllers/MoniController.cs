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


    [HttpPost(Name="PostMoni")]
    //public void PostMoni([FromBody] Moni moni)
    public async Task<ActionResult<Moni>> PostMoni([FromBody] Moni moni)
    {
        // Pwd encrypt
        moni.MoniPwd = BcryptNet.HashPassword(moni.MoniPwd);
        /* Console.WriteLine($"Pass : {moni.MoniPwd}");
        Console.WriteLine($"Crypted : {hashed}");
        Console.WriteLine(BcryptNet.Verify(moni.MoniPwd, "$2a$11$d/eripQMuqiA5zkzA3ho7uIq2x6gWq4kykhKglmV3xZ6KJpGkesLC"));
        Console.WriteLine(BcryptNet.Verify(moni.MoniPwd, hashed));
        */
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