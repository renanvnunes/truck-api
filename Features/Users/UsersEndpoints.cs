using TruckApi.Features.Users.CreateUser;
using TruckApi.Shared;

namespace TruckApi.Features.Users;

public static class UsersEndpoints
{
    public static IEndpointRouteBuilder MapUsersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/users").WithTags("Users");

        group
            .MapPost(
                "/",
                async (CreateUserRequest request, CreateUserUseCase useCase) =>
                {
                    try
                    {
                        var user = await useCase.ExecuteAsync(request);
                        var response = new CreateUserResponse(
                            user.Id,
                            user.FullName,
                            user.Whatsapp,
                            user.Role.ToString(),
                            user.IsActive,
                            user.CreatedAt
                        );
                        return Results.Created($"/users/{user.Id}", response);
                    }
                    catch (InvalidOperationException ex)
                    {
                        return Results.Conflict(new { error = ex.Message });
                    }
                }
            )
            .AddEndpointFilter<ValidationFilter<CreateUserRequest>>();

        return app;
    }
}
