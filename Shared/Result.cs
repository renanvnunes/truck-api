namespace TruckApi.Shared;

public abstract class Result<T>
{
    private Result() { }

    public sealed class Ok(T value) : Result<T>
    {
        public T Value { get; } = value;
    }

    public sealed class Fail(Error error) : Result<T>
    {
        public Error Error { get; } = error;
    }

    public static Result<T> Success(T value) => new Ok(value);
    public static Result<T> Failure(Error error) => new Fail(error);
}
