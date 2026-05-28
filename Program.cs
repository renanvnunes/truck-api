using System.Text.Json;
using System.Text.Json.Serialization;
using Carter;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TruckApi.Features.Users.Interface;
using TruckApi.Features.Users.Repository;
using TruckApi.Features.Users.UseCases;
using TruckApi.Infrastructure.Database;
using TruckApi.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"))
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<CreateUserUseCase>();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddCarter();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment()) { }
app.MapOpenApi();
app.MapScalarApiReference();

app.MapGet("/", () => "TruckApi is running.");
app.MapCarter();

app.Run();
