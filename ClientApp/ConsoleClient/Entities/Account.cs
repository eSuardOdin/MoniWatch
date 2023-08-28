namespace MoniWatch.Client.Entities;

public class Account
{
    public int AccountId { get; set; }

    public string AccountName { get; set; } = null!;

    // public int MoniId { get; set; }

    public double AccountBalance { get; set; }
}