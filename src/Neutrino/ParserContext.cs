namespace Neutrino;

/// <summary>
/// The context of the parser, which includes the input buffer and the state.
/// </summary>
/// <typeparam name="TState">The type of the state used during parsing.</typeparam>
/// <param name="Buffer">
/// The array of input strings that the parser is currently processing.
/// </param>
/// <param name="State">
/// The current state of the parser, which is used to track
/// the progress of parsing and any accumulated data.
/// </param>
/// <param name="OptionsTerminated">
/// A flag indicating whether no more options should be parsed and instead
/// the remaining input should be treated as positional arguments.
/// This is typically set when the parser encounters a <c>--</c> in the input,
/// which is a common convention in command-line interfaces to indicate
/// that no further options should be processed.
/// </param>
public record ParserContext<TState>(
    ArraySegment<string> Buffer,
    TState State,
    bool OptionsTerminated
);