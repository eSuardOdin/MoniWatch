using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Formatters;  // IOutputFormatters
using MoniWatch.DataContext;


var builder = WebApplication.CreateBuilder(args);




// Add services to the container.

builder.Services.AddControllers(options => 
{
    foreach (IOutputFormatter formatter in options.OutputFormatters)
    {
        OutputFormatter? mediaFormatter = formatter as OutputFormatter;
    }
})
.AddXmlDataContractSerializerFormatters()
.AddXmlSerializerFormatters();
builder.Services.AddMoniWatchContext();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
