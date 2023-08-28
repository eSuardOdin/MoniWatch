using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using MoniWatch.Client.Entities;

HttpClient client = new HttpClient();

try
{
    // using HttpResponseMessage res = await client.GetAsync("http://localhost:5000/transaction/GetAllTransactions");
    // var transactions = await client.GetFromJsonAsync<List<Transaction>>("http://192.168.30.10:5000/transaction/GetAllTransactions");
    var accounts = await client.GetFromJsonAsync<List<Account>>("http://192.168.30.10:5000/account/GetAllAccounts");
    // await client.DeleteAsync("http://192.168.30.10:5000/transaction/DeleteTransaction?transactionId=2");
    foreach(Account acc in accounts)
    {
        // Console.WriteLine($"ID: {tr.TransactionId} -> {tr.TransactionName}: {tr.TransactionAmount}€");
        Console.WriteLine($"ID: {acc.AccountId} -> {acc.AccountName}: {acc.AccountBalance}€");
    }
    // Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
Console.WriteLine("Client App end");
