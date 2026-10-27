using Basket.API.Apis;
using Basket.API.Grpc;
using Basket.API.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.AddServiceDefaults();

builder.AddRedisClient("redis");
builder.Services.AddSingleton<IBasketRepository, RedisBasketRepository>();

builder.Services.AddOpenApi();

builder.Services.AddValidation();

builder.Services.AddGrpc();

builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapDefaultEndpoints();

app.MapBasketApi();

app.MapGrpcService<BasketService>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.MapGrpcReflectionService();
}

app.UseHttpsRedirection();

app.Run();
