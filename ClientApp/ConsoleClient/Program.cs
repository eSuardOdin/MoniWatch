using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using MoniWatch.Client.Entities;

HttpClient client = new HttpClient();

try
{
    // using HttpResponseMessage res = await client.GetAsync("http://localhost:5000/transaction/GetAllTransactions");
    var transactions = await client.GetFromJsonAsync<List<Transaction>>("http://localhost:5000/transaction/GetAllTransactions");
    foreach(Transaction tr in transactions)
    {
        Console.WriteLine($"{tr.TransactionName}: {tr.TransactionAmount}€");
    }
    // Console.WriteLine(result);
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}
Console.WriteLine("Client App end");
