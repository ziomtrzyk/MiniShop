using Catalog.API.Apis;
using Catalog.API.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.Services.AddValidation();

builder.AddNpgsqlDbContext<CatalogContext>("catalogdb");

var app = builder.Build();

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CatalogContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    await context.Database.MigrateAsync();
    await CatalogContextSeed.SeedAsync(context, logger);
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapCatalogApi();

app.Run();