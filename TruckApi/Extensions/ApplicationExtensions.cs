using System.Text.Json;
using System.Text.Json.Serialization;
using Carter;
using FluentValidation;
using TruckApi.Features.Company.Interfaces;
using TruckApi.Features.Company.Repository;
using TruckApi.Features.Company.UseCases;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Features.Users.Repository;
using TruckApi.Features.Users.UseCases;

namespace TruckApi.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddOpenApi();
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
            options.SerializerOptions.UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow;
        });

        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<CreateCompanyUseCase>();
        services.AddScoped<GetAllCompaniesUseCase>();
        services.AddScoped<GetCompanyByIdUseCase>();
        services.AddScoped<UpdateCompanyUseCase>();
        services.AddScoped<DeleteCompanyUseCase>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<GetAllUsersUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<UpdateUserUseCase>();
        services.AddScoped<DeleteUserUseCase>();

        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddCarter();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }
}
