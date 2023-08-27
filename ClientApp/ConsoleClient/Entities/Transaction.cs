namespace MoniWatch.Client.Entities;


public class Transaction
{
    public int TransactionId { get; set; }

    public int AccountId { get; set; }

    public double TransactionAmount { get; set; }

    public string TransactionName { get; set; } = null!;

    public DateTime TransactionDate { get; set; } = default;

    public int TagId { get; set; }
}