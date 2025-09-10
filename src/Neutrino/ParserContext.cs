namespace Neutrino;

// /// <summary>
// /// The context of the parser, which includes the input buffer and the state.
// /// </summary>
// /// <typeparam name="TState">The type of the state used during parsing.</typeparam>
// /// <param name="Buffer">
// /// The array of input strings that the parser is currently processing.
// /// </param>
// /// <param name="State">
// /// The current state of the parser, which is used to track
// /// the progress of parsing and any accumulated data.
// /// </param>
// /// <param name="OptionsTerminated">
// /// A flag indicating whether no more options should be parsed and instead
// /// the remaining input should be treated as positional arguments.
// /// This is typically set when the parser encounters a <c>--</c> in the input,
// /// which is a common convention in command-line interfaces to indicate
// /// that no further options should be processed.
// /// </param>
// public record ParserContext<TState>
// {
//     /// <summary>
//     /// The array of input strings that the parser is currently processing.
//     /// </summary>
//     public ArraySegment<string> Buffer { get; init; }
//
//     /// <summary>
//     /// The current state of the parser, which is used to track
//     /// the progress of parsing and any accumulated data.
//     /// </summary>
//     public TState State { get; init; }
//
//     /// <summary>
//     /// A flag indicating whether no more options should be parsed and instead
//     /// the remaining input should be treated as positional arguments.
//     /// This is typically set when the parser encounters a <c>--</c> in the input,
//     /// which is a common convention in command-line interfaces to indicate
//     /// that no further options should be processed.
//     /// </summary>
//     public bool OptionsTerminated { get; init; }
// }

public class Reader
{
    public static Reader<T> FromArray<T>(IReadOnlyList<T> array) => new ArrayReader<T>(array);
}

class ArrayReader<T>(IReadOnlyList<T> array, int index = 0) : Reader<T>
{
    public override T Head => array[index];
    public override Reader<T> Tail => new ArrayReader<T>(array, index + 1);
    public override bool IsEmpty => array.Count <= index;
}