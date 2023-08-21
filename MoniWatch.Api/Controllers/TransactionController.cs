using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using MoniWatch.Entities;
using MoniWatch.DataContext;



namespace MoniWatch.Api.Controllers;

[ApiController]
[Route("transaction")]
public class TransactionController : ControllerBase
{
    private readonly ILogger<TransactionController> _logger;
    public TransactionController(ILogger<TransactionController> logger)
    {
        _logger = logger;
    }


    // o----------------------o
    // | GET ALL TRANSACTIONS |
    // o----------------------o
    /// <summary>
    /// Get all transactions in DB with URL: /root/transaction</br>
    /// Or all transactions from a specific user with URL: /root/transaction?AccountId={id}
    /// </summary>
    /// <param name="accountId">The account to filter transactions with</param>
    /// <returns></returns>
    [HttpGet(Name="GetAllTransactions")]
    public async Task<IEnumerable<Transaction>> GetAllTransactions(int? accountId)
    {
        using (MoniWatchDbContext db = new())
        {
            return !accountId.HasValue ? await db.Transactions.ToArrayAsync() : await db.Transactions.Where(t => t.AccountId == accountId).ToArrayAsync();
        }
    }


    // o------------------------o
    // | GET UNIQUE TRANSACTION |
    // o------------------------o
    /// <summary>
    /// Get a transaction with URL: /root/transaction /{id}
    /// </summary>
    /// <param name="id">The id of transaction to find</param>
    /// <returns>A status code</returns>
    [HttpGet("{id}", Name = "GetTransaction")]
    public async Task<ActionResult<Transaction>> GetTransaction(int id)
    {
        using(MoniWatchDbContext db = new())
        {
            Transaction transaction = await db.Transactions.FindAsync(id);
            if (transaction is null) 
            {
                return NotFound();
            }
            else
            {
                return Ok(transaction);
            }
        }
    }


    // o----------------------o
    // | POST NEW TRANSACTION |
    // o----------------------o 
    [HttpPost(Name = "PostTransaction")]
    public async Task<ActionResult<Transaction>> PostTransaction([FromBody]Transaction transaction)
    {
        using (MoniWatchDbContext db = new())
        {
            // Add transaction
            db.Transactions.Add(transaction);
            await db.SaveChangesAsync();

            // Get associated account
            Account account = await db.Accounts.FindAsync(transaction.AccountId);
            if (account is null)
            {
                return NotFound("Account not found");
            }
            account.AccountBalance = Math.Round(account.AccountBalance + transaction.TransactionAmount, 2);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new {id = transaction.TransactionId}, transaction);
        }
    }
}