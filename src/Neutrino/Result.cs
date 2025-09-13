using Neutrino.Messages;

namespace Neutrino;

abstract record Result<T>
{
    public record Success(T Value) : Result<T>;
    public record Failure(Message Error) : Result<T>;

    
    public static implicit operator Result<T>(Result.FailureType failure) => new Failure(failure.Error);
}

class Result
{
    public record struct FailureType(Message Error);
    
    
    public static Result<T> Success<T>(T value) => new Result<T>.Success(value);
    public static FailureType Failure(Message error) => new FailureType(error);
}