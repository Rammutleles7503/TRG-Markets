using Microsoft.EntityFrameworkCore;
using TRG_Markets.Persistence;
using TRG_Markets.Application;
using TRG_Markets.Application.Interfaces;
using TRG_Markets.Infrastructure.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<TRGMarketsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IMarketHolidayService, MarketHolidayService>();
builder.Services.AddScoped<ISystemAlertService, SystemAlertService>();
builder.Services.AddScoped<ITradingAccountService, TradingAccountService>();
builder.Services.AddScoped<ITradeService, TradeService>();
builder.Services.AddScoped<IProfitLightService, ProfitLightService>();
builder.Services.AddScoped<IEquityGuardianService, EquityGuardianService>();
builder.Services.AddScoped<IEntryAuthorityService, EntryAuthorityService>();
builder.Services.AddSingleton<IEmergencyControlService, EmergencyControlService>();
builder.Services.AddScoped<ISystemOrchestrationService, SystemOrchestrationService>();
builder.Services.AddScoped<IMt5BridgeService, Mt5BridgeService>();

builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();

app.Run();
