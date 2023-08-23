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
    /// Or all transactions from a specific user with URL: /root/transaction?accountId={id}
    /// </summary>
    /// <param name="accountId">The account to filter transactions with</param>
    /// <returns>An array of transactions depending on the needs</returns>
    [HttpGet(Name="GetAllTransactions")]
    public async Task<IEnumerable<Transaction>> GetAllTransactions(int? accountId, int? tagId)
    {
        using (MoniWatchDbContext db = new())
        {
            // If we want specific tags for a specific user
            if(accountId.HasValue && tagId.HasValue)
            {
                return await db.Transactions.Where(t => t.AccountId == accountId && t.TagId == tagId).ToArrayAsync();
            }
            // If we want transactions for a specific user
            else if(accountId.HasValue)
            {
                return await db.Transactions.Where(t => t.AccountId == accountId).ToArrayAsync();
            }
            // If we want transactions with a tag but no specific user
            else if(tagId.HasValue)
            {
                return await db.Transactions.Where(t => t.TagId == tagId).ToArrayAsync();
            }
            // If we want ALL transactions
            return await db.Transactions.ToArrayAsync();
        }
    }


    // o------------------------o
    // | GET UNIQUE TRANSACTION |
    // o------------------------o
    /// <summary>
    /// Get a transaction with URL: /root/transaction/{id}
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
    /// <summary>
    /// Posts a new transaction to database</br>
    /// Update account balance accordingly
    /// </summary>
    /// <param name="transaction">The transaction object to add (as a JSON)</param>
    /// <returns>Created object</returns>
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
            // Update account balance
            account.AccountBalance = Math.Round(account.AccountBalance + transaction.TransactionAmount, 2);
            await db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTransaction), new {id = transaction.TransactionId}, transaction);
        }
    }


    // o--------------------o
    // | DELETE TRANSACTION |
    // o--------------------o

    /// <summary>
    /// Deletes a transaction and updates linked account balance </br>
    /// with URL: /root/transaction?id={id}
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete(Name="DeleteTransaction")]
    public async Task<ActionResult> DeleteTransaction(int id)
    {
        using (MoniWatchDbContext db = new())
        {
            Transaction transaction = await db.Transactions.FindAsync(id);
            if(transaction is null)
            {
                return BadRequest("This transaction does not exists");
            }
            // Get linked account
            Account account = db.Accounts.Where(a => a.AccountId == transaction.AccountId).FirstOrDefault();
            if(account is null)
            {
                return BadRequest("This transaction is not linked to any account");
            }
            // Change account balance
            account.AccountBalance = Math.Round(account.AccountBalance - transaction.TransactionAmount, 2);
            db.Transactions.Remove(transaction);
            await db.SaveChangesAsync();
            return Ok($"Deleted {transaction.TransactionName}");
        }
    }



    // DATA UPDATE

    [HttpPatch(Name="UpdateTransactionAmount")]
    public async Task<ActionResult<Transaction>> UpdateTransactionAmount(int id, double newAmount)
    {
        using (MoniWatchDbContext db = new())
        {
            Transaction transaction = await db.Transactions.FindAsync(id);
            if(transaction is null)
            {
                return BadRequest("This transaction does not exists");
            }
            // Get linked account
            Account account = db.Accounts.Where(a => a.AccountId == transaction.AccountId).FirstOrDefault();
            if(account is null)
            {
                return BadRequest("This transaction is not linked to any account");
            }
            // Update
            account.AccountBalance = Math.Round(account.AccountBalance - (transaction.TransactionAmount - newAmount), 2);
            transaction.TransactionAmount = newAmount;
            await db.SaveChangesAsync();

            return Ok($"Transaction '{transaction.TransactionName} modified'");
        }
    }

}