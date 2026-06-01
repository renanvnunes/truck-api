using Carter;
using TruckApi.Features.Users.Dtos.CreateUser;
using TruckApi.Features.Users.UseCases;
using TruckApi.Infrastructure.Database.Entities;

namespace TruckApi.Features.Users.Endpoints;

public class UserCreate : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGroup("/users")
            .WithTags("Users")
            .MapPost(
                "/",
                (CreateUserRequest request, CreateUserUseCase useCase) =>
                    useCase.ExecuteAsync(request).ToCreatedAsync(user => (
                        $"/users/{user.Id}",
                        new CreateUserResponse(
                            user.Id,
                            user.FullName,
                            user.Whatsapp,
                            user.Role.ToString(),
                            user.CompanyId,
                            user.IsActive,
                            user.CreatedAt
                        )
                    ))
            )
            .WithSummary("Criar usuário")
            .WithDescription(
                "Cria um novo usuário no sistema. O whatsapp deve estar no formato internacional com 13 dígitos (ex: 5511999999999)."
            )
            .ProducesValidationProblem()
            .AddEndpointFilter<ValidationFilter<CreateUserRequest>>()
            .RequireAuth(UserRole.Admin, UserRole.CompanyManager);
    }
}
