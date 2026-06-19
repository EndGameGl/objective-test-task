using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Objective.PriceTracker.Api;
using Objective.PriceTracker.Api.Extensions;
using Objective.PriceTracker.Api.Services;
using Objective.PriceTracker.Api.Services.Database;
using Objective.PriceTracker.Api.Services.Hosted;
using Objective.PriceTracker.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContextPool<PriceTrackerDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("Postgres"),
        dbCtxOptions =>
        {
            dbCtxOptions.MigrationsAssembly("Objective.PriceTracker.Api");
            dbCtxOptions.MigrationsHistoryTable("__EFMigrationsHistory", "price_tracking");
        }
    );
});

builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();

builder.Services.AddHostedService<BackgroundPriceUpdater>();
builder.Services.AddHostedServiceWithInterface<IMailSender, BackgroundMailSender>();
builder.Services.AddSingleton<IPrinzipSubscriptionHandler, PrinzipSubscriptionHandler>();
builder.Services.AddHttpClient<IPrinzipApi, PrinzipApiClient>(o =>
{
    var prinzipApiHost = builder.Configuration.GetValue<string>("Hosts:PrinzipApi");
    ArgumentException.ThrowIfNullOrWhiteSpace(prinzipApiHost);
    o.BaseAddress = new Uri(prinzipApiHost);
});

builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

await ApplyMigrationsAsync(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

async Task ApplyMigrationsAsync(WebApplication webApplication)
{
    using var scope = webApplication.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<PriceTrackerDbContext>();
    await dbContext.Database.MigrateAsync();
}
