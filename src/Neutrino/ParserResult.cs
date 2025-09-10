using Neutrino.Messages;

namespace Neutrino;

public class ParserResult
{
    public record struct FailureType(Message Error, Reader<string> RemainingInput);
    
    public record struct ParsingTerminatedType(Reader<string> RemainingInput);

    
    public static ParserResult<TState> Success<TState>(TState value, Reader<string> remainingInput) =>
        new ParserResult<TState>.ParserSuccess(value, remainingInput);
    
    public static FailureType Failure(Message error, Reader<string> remainingInput) =>
        new FailureType(error, remainingInput);
    
    public static ParsingTerminatedType OptionsTerminated(Reader<string> remainingInput) =>
        new ParsingTerminatedType(remainingInput);
}

/// <summary>
/// A discriminated union type representing the result of a parser operation.
/// It can either indicate a successful parse with the next state and context,
/// or a failure with an error message.
/// </summary>
/// <typeparam name="TState">
/// </typeparam>
/// <typeparam name="TInput">
/// </typeparam>
public abstract record ParserResult<TState>(Reader<string> RemainingInput)
{
    /// <summary>
    /// Indicates that the parsing operation was successful.
    /// </summary>
    /// <param name="Next">The next context after parsing, which includes the updated input buffer.</param>
    /// <param name="Consumed">The input elements consumed by the parser during this operation.</param>
    public sealed record ParserSuccess(TState Value, Reader<string> RemainingInput) 
        : ParserResult<TState>(RemainingInput);
    
    /// <summary>
    /// Indicates that the parsing operation was successful, but options parsing has been terminated.
    /// </summary>
    public sealed record ParserOptionsTerminated(Reader<string> RemainingInput) 
        : ParserResult<TState>(RemainingInput);

    /// <summary>
    /// Indicates that the parsing operation failed.
    /// </summary>
    /// <param name="Error">The error message describing why the parsing failed.</param>
    public sealed record ParserFailure(Message Error, Reader<string> RemainingInput) 
        : ParserResult<TState>(RemainingInput);

    public static implicit operator ParserResult<TState>(ParserResult.FailureType failure) => new ParserFailure(failure.Error, failure.RemainingInput);
    
    public static implicit operator ParserResult<TState>(ParserResult.ParsingTerminatedType terminated) => new ParserOptionsTerminated(terminated.RemainingInput);
}