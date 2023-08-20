using Microsoft.AspNetCore.Mvc;
using MoniWatch.Entities;
namespace MoniWatch.Api.Controllers;
using MoniWatch.DataContext;


[ApiController]
[Route("/accounts")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetAccounts")]
    public IEnumerable<Account> Get()
    {
        using (MoniWatchDbContext db = new())
        {
            Account[] accounts = db.Accounts.ToArray();
            foreach (var account in accounts)
            {
                Console.WriteLine($"{account.AccountName} got.");
            }
            return accounts;
        }
    }
}