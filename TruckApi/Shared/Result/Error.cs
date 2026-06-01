namespace TruckApi.Shared.Result;

public record Error(string Code, string Message, int StatusCode = 400)
{
    public Microsoft.AspNetCore.Http.HttpResults.JsonHttpResult<ErrorResponse> ToHttpResult() =>
        Microsoft.AspNetCore.Http.TypedResults.Json(new ErrorResponse(Code, Message), statusCode: StatusCode);
}
