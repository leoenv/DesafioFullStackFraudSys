using Amazon.DynamoDBv2;
using FraudSys.Application.Services;
using FraudSys.Domain.Interfaces;
using FraudSys.Infrastructure.DynamoDB;
using FraudSys.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IAmazonDynamoDB>(_ =>
    DynamoDbConfig.CreateClient(useLocal: true));

builder.Services.AddScoped<ILimitRepository, DynamoLimitRepository>();
builder.Services.AddScoped<LimitService>();
builder.Services.AddScoped<PixTransactionService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllers();

app.Run();