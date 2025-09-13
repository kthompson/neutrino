using Neutrino.Messages;

namespace Neutrino;

public class ParserResult
{
    public record struct FailureType(int Consumed, Message Error);
    
    public static ParserResult<T> Success<T>(ParserContext<T> next, IReadOnlyList<string> consumed) =>
        new ParserResult<T>.ParserSuccess(next, consumed);
    
    public static FailureType Failure(int consumed, Message error) =>
        new FailureType(consumed, error);
}

/// <summary>
/// A discriminated union type representing the result of a parser operation.
/// It can either indicate a successful parse with the next state and context,
/// or a failure with an error message.
/// </summary>
/// <typeparam name="TState">
/// The type of the state after parsing. It should match with the <c>TState</c> type of the <see cref="Parser{TValue, TState}"/>.
/// </typeparam>
public abstract record ParserResult<TState>
{
    /// <summary>
    /// Indicates whether the parsing operation was successful.
    /// </summary>
    public abstract bool Success { get; }
    

    /// <summary>
    /// Indicates that the parsing operation was successful.
    /// </summary>
    /// <param name="Next">The next context after parsing, which includes the updated input buffer.</param>
    /// <param name="Consumed">The input elements consumed by the parser during this operation.</param>
    public sealed record ParserSuccess(ParserContext<TState> Next, IReadOnlyList<string> Consumed) : ParserResult<TState>
    {
        /// <inheritdoc/>
        public override bool Success => true;
    }

    /// <summary>
    /// Indicates that the parsing operation failed.
    /// </summary>
    /// <param name="Consumed">The number of the consumed input elements.</param>
    /// <param name="Error">The error message describing why the parsing failed.</param>
    public sealed record ParserFailure(int Consumed, Message Error) : ParserResult<TState>
    {
        /// <inheritdoc/>
        public override bool Success => false;
    }

    public static implicit operator ParserResult<TState>(ParserResult.FailureType failure) => new ParserFailure(failure.Consumed, failure.Error);
}