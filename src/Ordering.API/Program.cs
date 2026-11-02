using Microsoft.EntityFrameworkCore;
using Ordering.Domain.OrderAggregate;
using Ordering.Domain.SeedWork;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.AddServiceDefaults();

builder.Services.AddOpenApi();

builder.AddNpgsqlDbContext<OrderingContext>("orderingdb");

builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

app.MapDefaultEndpoints();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OrderingContext>();

    await context.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
