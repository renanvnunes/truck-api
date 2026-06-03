using System.Text.Json;
using System.Text.Json.Serialization;
using Carter;
using FluentValidation;
using TruckApi.Features.Company.Interfaces;
using TruckApi.Features.Company.Repository;
using TruckApi.Features.Company.UseCases;
using TruckApi.Features.Machine.Interfaces;
using TruckApi.Features.Machine.Repository;
using TruckApi.Features.Machine.UseCases;
using TruckApi.Features.Users.Interfaces;
using TruckApi.Features.Users.Repository;
using TruckApi.Features.Users.UseCases;
using TruckApi.Shared.Utils;

namespace TruckApi.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddOpenApi("v1", options =>
        {
            options.AddDocumentTransformer((doc, _, _) =>
            {
                doc.Info.Title = "TruckApi";
                doc.Info.Version = "v1";
                return Task.CompletedTask;
            });
        });
        services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new TrimmingStringJsonConverter());
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

        services.AddScoped<IMachineRepository, MachineRepository>();
        services.AddScoped<CreateMachineUseCase>();
        services.AddScoped<GetAllMachinesUseCase>();
        services.AddScoped<GetMachineByIdUseCase>();
        services.AddScoped<UpdateMachineUseCase>();
        services.AddScoped<UpdateMachineStatusUseCase>();
        services.AddScoped<UpdateMachineHourmeterUseCase>();
        services.AddScoped<DeleteMachineUseCase>();

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
