using Microsoft.AspNetCore.Mvc;
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
    [HttpGet(Name = "GetAccounts")]
    public IEnumerable<Account> Get()
    {
        using (MoniWatchDbContext db = new())
        {
            Account[] accounts = db.Accounts.ToArray();
            foreach (var account in accounts)
            {
                var json = JsonSerializer.Serialize(account);
                // Console.WriteLine($"{account.AccountName} got.");
                Console.WriteLine($"{json}");
            }
            return accounts;
        }
    }

    
    [HttpPost(Name = "GetAccounts")]
    public IActionResult Post([FromBody] Account account)
    {
        using (MoniWatchDbContext db = new())
        {
            if(account is null)
            {
                return BadRequest("Invalid data");
            }
            else
            {
                db.Add(account);
                db.SaveChanges();
                return Ok("Data added in db");
            }
        }
    }
}