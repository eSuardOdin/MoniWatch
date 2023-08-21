using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MoniWatch.Entities;
using MoniWatch.DataContext;



namespace MoniWatch.Api.Controllers;

[ApiController]
[Route("account")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }


    // o------------------o
    // | GET ALL ACCOUNTS |
    // o------------------o
    /// <summary>
    /// Get all accounts in DB with URL: /root/account</br>
    /// Or all accounts from a specific user with URL: /root/account?MoniId={id}
    /// </summary>
    /// <param name="moniId">If specified, the id of account's owner</param>
    /// <returns>An array of accounts</returns>
    [HttpGet(Name = "GetAllAccounts")]
    public async Task<IEnumerable<Account>> GetAllAccounts(int? moniId)
    {
        using (MoniWatchDbContext db = new())
        {
            return !moniId.HasValue ? await db.Accounts.ToArrayAsync() : await db.Accounts.Where(a => a.MoniId == moniId).ToArrayAsync();
        }
    }


    // o--------------------o
    // | GET UNIQUE ACCOUNT |
    // o--------------------o
    /// <summary>
    /// Get an account with URL: /root/account/{id}
    /// </summary>
    /// <param name="id">The id of the account to find</param>
    /// <returns>A status code</returns>
    [HttpGet("{id}", Name = "GetAccount")]
    public async Task<ActionResult<Account>> GetAccount(int id)
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


    // o------------------o
    // | POST NEW ACCOUNT |
    // o------------------o    
    /// <summary>
    /// Adds an account to the database
    /// </summary>
    /// <param name="account">The account to add (specified as JSON, translated by EF Core</param>
    /// <returns>A status code</returns>
    [HttpPost(Name = "PostAccount")]
    public async Task<ActionResult<Account>> PostAccount([FromBody] Account account)
    {
        using (MoniWatchDbContext db = new())
        {
            db.Accounts.Add(account);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccount), new {id = account.AccountId}, account);
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