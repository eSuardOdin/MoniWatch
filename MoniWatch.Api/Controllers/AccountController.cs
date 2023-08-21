using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MoniWatch.Entities;
using MoniWatch.DataContext;



namespace MoniWatch.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }


    /// <summary>
    /// Get all accounts in DB
    /// </summary>
    /// <returns>An array of accounts</returns>
    [HttpGet(Name = "GetAllAccounts")]
    public async Task<IEnumerable<Account>> GetAll()
    {
        using (MoniWatchDbContext db = new())
        {
            Account[] accounts = await db.Accounts.ToArrayAsync();
            return accounts;
        }
    }

    [HttpGet("{id}", Name = "GetAccount")]
    public async Task<ActionResult<Account>> Get(int id)
    {
        using (MoniWatchDbContext db = new())
        {
            Account acc = await db.Accounts.FindAsync(id);
            if (acc is null) 
            {
                return NotFound();
            }
            else
            {
                return Ok(acc);
            }
        }
    }

    

    /// <summary>
    /// Adds an account to the database
    /// </summary>
    /// <param name="account">The account to add (specified as JSON, translated by EF Core</param>
    /// <returns></returns>
    [HttpPost(Name = "PostAccount")]
    public async Task<ActionResult<Account>> Post([FromBody] Account account)
    {
        using (MoniWatchDbContext db = new())
        {
            db.Accounts.Add(account);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), null, account);
        }
    }
}



/*
    Old Sync

    // [HttpPost(Name = "GetAccounts")]
    // public IActionResult Post([FromBody] Account account)
    // {
    //     using (MoniWatchDbContext db = new())
    //     {
    //         if(account is null)
    //         {
    //             return BadRequest("Invalid data");
    //         }
    //         else
    //         {
    //             db.Add(account);
    //             db.SaveChanges();
    //             return Ok("Data added in db");
    //         }
    //     }
    // }
*/