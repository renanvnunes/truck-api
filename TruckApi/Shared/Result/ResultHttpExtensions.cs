using System.Diagnostics;
using Microsoft.AspNetCore.Http.HttpResults;

namespace TruckApi.Shared.Result;

public static class ResultHttpExtensions
{
    public static Results<Ok<T>, JsonHttpResult<ErrorResponse>> ToHttpResult<T>(
        this Result<T> result
    ) =>
        result switch
        {
            Result<T>.Ok { Value: var value } => TypedResults.Ok(value),
            Result<T>.Fail { Error: var error } => error.ToHttpResult(),
            _ => throw new UnreachableException(),
        };

    public static Results<Ok<TResponse>, JsonHttpResult<ErrorResponse>> ToHttpResult<T, TResponse>(
        this Result<T> result,
        Func<T, TResponse> map
    ) =>
        result switch
        {
            Result<T>.Ok { Value: var value } => TypedResults.Ok(map(value)),
            Result<T>.Fail { Error: var error } => error.ToHttpResult(),
            _ => throw new UnreachableException(),
        };

    public static Results<NoContent, JsonHttpResult<ErrorResponse>> ToNoContentResult<T>(
        this Result<T> result
    ) =>
        result switch
        {
            Result<T>.Ok => TypedResults.NoContent(),
            Result<T>.Fail { Error: var error } => error.ToHttpResult(),
            _ => throw new UnreachableException(),
        };

    public static Results<Created<TResponse>, JsonHttpResult<ErrorResponse>> ToCreatedResult<T, TResponse>(
        this Result<T> result,
        Func<T, (string Location, TResponse Response)> map
    ) =>
        result switch
        {
            Result<T>.Ok { Value: var value } => TypedResults.Created(map(value).Location, map(value).Response),
            Result<T>.Fail { Error: var error } => error.ToHttpResult(),
            _ => throw new UnreachableException(),
        };

    public static async Task<Results<Ok<T>, JsonHttpResult<ErrorResponse>>> ToHttpResultAsync<T>(
        this Task<Result<T>> task
    ) => (await task).ToHttpResult();

    public static async Task<Results<Ok<TResponse>, JsonHttpResult<ErrorResponse>>> ToHttpResultAsync<T, TResponse>(
        this Task<Result<T>> task,
        Func<T, TResponse> map
    ) => (await task).ToHttpResult(map);

    public static async Task<Results<NoContent, JsonHttpResult<ErrorResponse>>> ToNoContentAsync<T>(
        this Task<Result<T>> task
    ) => (await task).ToNoContentResult();

    public static async Task<Results<Created<TResponse>, JsonHttpResult<ErrorResponse>>> ToCreatedAsync<T, TResponse>(
        this Task<Result<T>> task,
        Func<T, (string Location, TResponse Response)> map
    ) => (await task).ToCreatedResult(map);
}
