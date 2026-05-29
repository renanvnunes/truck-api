namespace TruckApi.Features.Users.Dtos.GetAllUsers;

/// <summary>Resposta paginada com cursor da listagem de usuários.</summary>
/// <param name="Data">Lista de usuários da página atual.</param>
/// <param name="NextCursor">Cursor para buscar a próxima página. Nulo quando não há mais registros.</param>
public record GetAllUsersResponse(GetAllUsersItem[] Data, string? NextCursor);
