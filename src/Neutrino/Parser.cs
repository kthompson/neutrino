namespace Neutrino;

/// <summary>
/// IParser interface for command-line argument parsing.
/// </summary>
/// <typeparam name="TValue">The type of the value returned by the parser.</typeparam>
/// <typeparam name="TState">The type of the state used during parsing.</typeparam>
public interface IParser<TValue, TState>
{
    /// <summary>
    /// The usage information for this parser, which describes how to use it in command-line interfaces.
    /// </summary>
    Usage Usage { get; }

    /// <summary>
    /// The initial state for this parser. This is used to initialize the state when parsing starts.
    /// </summary>
    TState InitialState { get; }

    /// <summary>
    /// Parses the input context and returns a result indicating whether the parsing was successful or not.
    /// </summary>
    /// <param name="context">
    /// The context of the parser, which includes the input buffer and the current state.
    /// </param>
    /// <returns>
    /// A result object indicating success or failure.
    /// </returns>
    ParserResult<TState> Parse(ParserContext<TState> context);

    /// <summary>
    /// Transforms a <see cref="TState"/> into a <see cref="TValue"/>, if applicable.
    /// If the transformation is not applicable, it should return
    /// a <see cref="ValueParserResult{TValue}"/> with `success: false` and an
    /// appropriate error message.
    /// </summary>
    /// <param name="state">
    /// The current state of the parser, which may contain accumulated
    /// data or context needed to produce the final value.
    /// </param>
    /// <returns>
    /// A result object indicating success or failure of the transformation.
    /// If successful, it should contain the parsed value of type <see cref="TValue"/>.
    /// If not applicable, it should return an error message.
    /// </returns>
    ValueParserResult<TValue> Complete(TState state);
}