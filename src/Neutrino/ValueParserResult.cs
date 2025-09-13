using Neutrino.Messages;

namespace Neutrino;

/// <summary>
/// Interface for parsing CLI option values and arguments.
/// A `ValueParser` is responsible for converting string input (typically from CLI
/// arguments or option values) into strongly-typed values of type {@link T}.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract record ValueParserResult<T>
{
    
    /// <summary>
    /// Indicates that the parsing operation was successful.
    /// </summary>
    public sealed record Success(T Value) : ValueParserResult<T>;

    /// <summary>
    /// Indicates that the parsing operation failed.
    /// </summary>
    /// <param name="Error">The error message describing why the parsing failed.</param>
    public sealed record Failure(Message Error) : ValueParserResult<T>;

    public static implicit operator ValueParserResult<T>(ValueParserResult.FailureType failure) => new Failure(failure.Error);
}

public static class ValueParserResult
{
    public record struct FailureType(Message Error);
    public static ValueParserResult<T> Success<T>(T value) => new ValueParserResult<T>.Success(value);
    public static FailureType Failure(Message error) => new FailureType(error);
}