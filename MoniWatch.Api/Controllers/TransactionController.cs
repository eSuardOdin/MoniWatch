using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
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
    /// Get all transactions in DB with URL: /root/transaction/GetAllTransactions</br>
    /// Or all transactions from a specific user with URL: /root/transaction/GetAllTransactions?accountId={id}
    /// Or all transactions from a specific tag with URL: /root/transaction/GetAllTransactions?tagId={id}
    /// </summary>
    /// <param name="accountId">The account to filter transactions with</param>
    /// <param name="tagId">The tag to filter transactions with</param>
    /// <returns>An array of transactions depending on the needs</returns>
    [HttpGet]
    [Route("GetAllTransactions")]
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


    // o--------------------------------o
    // | GET ALL TRANSACTIONS WITH DATE |
    // o--------------------------------o

    [HttpGet]
    [Route("GetAllTransactionsWithDate")]
    public async Task<IEnumerable<Transaction>> GetAllTransactionsWithDate(int accountId, DateTime startDate, DateTime? endDate, bool isAsc = true)
    {
        using (MoniWatchDbContext db = new())
        {
            var all = await db.Transactions.Where(t => t.AccountId == accountId).ToArrayAsync();
            if (endDate is null)
            {
                return isAsc ? 
                    all.Where(t => t.TransactionDate >= startDate).OrderBy(t => t.TransactionDate) :
                    all.Where(t => t.TransactionDate >= startDate).OrderByDescending(t => t.TransactionDate);
            }
            return isAsc ? 
                all.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).OrderBy(t => t.TransactionDate) :
                all.Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate).OrderByDescending(t => t.TransactionDate);
        }
    }


    // o------------------------o
    // | GET UNIQUE TRANSACTION |
    // o------------------------o
    /// <summary>
    /// Get a transaction with URL: /root/transaction/GetTransaction?transactionId={id}
    /// </summary>
    /// <param name="transactionId">The id of transaction to find</param>
    /// <returns>A status code</returns>
    [HttpGet]
    [Route("GetTransaction")]
    public async Task<ActionResult<Transaction>> GetTransaction(int transactionId)
    {
        using(MoniWatchDbContext db = new())
        {
            Transaction transaction = await db.Transactions.FindAsync(transactionId);
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
    [HttpPost]
    [Route("PostTransaction")]
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

            return CreatedAtAction(nameof(GetTransaction), new {transactionId = transaction.TransactionId}, transaction);
        }
    }


    // o--------------------o
    // | DELETE TRANSACTION |
    // o--------------------o
    /// <summary>
    /// Deletes a transaction and updates linked account balance </br>
    /// with URL: /root/transaction/DeleteTransaction?transactionId={id}
    /// </summary>
    /// <param name="transactionId">Id of the transaction to delete</param>
    /// <returns>A status code</returns>
    [HttpDelete]
    [Route("DeleteTransaction")]
    public async Task<ActionResult> DeleteTransaction(int transactionId)
    {
        using (MoniWatchDbContext db = new())
        {
            Transaction transaction = await db.Transactions.FindAsync(transactionId);
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
    // o---------------------------o
    // | UPDATE TRANSACTION AMOUNT |
    // o---------------------------o
    /// <summary>
    /// Update transaction amount with an Http PATCH request</br>
    /// on URL: /root/transaction/UpdateTransactionAmount?transactionId={id}&newAmout={amount}</br>
    /// Updates account balance
    /// </summary>
    /// <param name="transactionId">Id of the transaction to patch</param>
    /// <param name="newAmount">Amount of the transaction</param>
    /// <returns>Status code</returns>
    [HttpPatch]
    [Route("UpdateTransactionAmount")]
    public async Task<ActionResult<Transaction>> UpdateTransactionAmount(int transactionId, double newAmount)
    {
        using (MoniWatchDbContext db = new())
        {
            Transaction transaction = await db.Transactions.FindAsync(transactionId);
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


    // o-------------------------o
    // | UPDATE TRANSACTION NAME |
    // o-------------------------o
    /// <summary>
    /// Update transaction name with an Http PATCH request</br>
    /// on URL: /root/transaction/UpdateTransactionAmount?transactionId={id}&newName={name}</br>
    /// </summary>
    /// <param name="transactionId">Id of transaction to change</param>
    /// <param name="newName">New value to assign</param>
    /// <returns>A status code</returns>
    [HttpPatch]
    [Route("UpdateTransactionName")]
    public async Task<ActionResult<Transaction>> UpdateTransactionName(int transactionId, string newName)
    {
        using (MoniWatchDbContext db = new())
        {
            Transaction transaction = await db.Transactions.FindAsync(transactionId);
            if(transaction is null)
            {
                return BadRequest("This transaction does not exists");
            }
            // Update
            transaction.TransactionName = newName;
            await db.SaveChangesAsync();

            return Ok($"Transaction '{transaction.TransactionName} modified'");
        }
    }


    // o------------------------o
    // | UPDATE TRANSACTION TAG |
    // o------------------------o
    /// <summary>
    /// Change the tag of a transaction</br>
    /// Ensure the tag provided is from the user provided and assign it to the transaction
    /// </summary>
    /// <param name="transactionId">Id of the transaction to</param>
    /// <param name="newTag"></param>
    /// <param name="moniId"></param>
    /// <returns></returns>
    [HttpPatch]
    [Route("UpdateTransactionTag")]
    public async Task<ActionResult<Transaction>> UpdateTransactionTag(int transactionId, int newTag, int moniId)
    {
        using (MoniWatchDbContext db = new())
        {
            Transaction transaction = await db.Transactions.FindAsync(transactionId);
            if(transaction is null)
            {
                return BadRequest("This transaction does not exists");
            }

            Tag tag = await db.Tags.FindAsync(newTag);
            // If tag do not exists or is not one owned by user
            if(tag is null || tag.MoniId != moniId)
            {
                return BadRequest("This tag does not exists");
            }
            // Update
            transaction.TagId = newTag;
            await db.SaveChangesAsync();

            return Ok($"Transaction '{transaction.TransactionName} modified'");
        }
    }

    // o-------------------------o
    // | UPDATE TRANSACTION DATE |
    // o-------------------------o

    [HttpPatch]
    [Route("UpdateTransactionDate")]
    public async Task<ActionResult<Transaction>> UpdateTransactionDate(DateTime transactionDate, int transactionId)
    {
        using (MoniWatchDbContext db = new())
        {
            Transaction transaction = await db.Transactions.FindAsync(transactionId);
            if (transaction is null)
            {
                return BadRequest ("Transaction not found");
            }
            transaction.TransactionDate = transactionDate;
            await db.SaveChangesAsync();

            return Ok($"Transaction date is now: {transaction.TransactionDate}");
        }
    }

}