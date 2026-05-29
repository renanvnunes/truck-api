using Carter;
using Microsoft.AspNetCore.HttpOverrides;
using Scalar.AspNetCore;
using TruckApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.UseForwardedHeaders(
    new ForwardedHeadersOptions
    {
        ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedHost,
    }
);
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseExceptionHandler();

app.MapOpenApi();
app.MapScalarApiReference("/docs", options => options.Title = "TruckApi");

app.MapGet("/", () => "TruckApi is running.");
app.MapCarter();

app.Run();
