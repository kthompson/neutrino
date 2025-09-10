namespace Neutrino;
/// <summary>
/// Interface for parsing CLI option values and arguments.
/// A `ValueParser` is responsible for converting string input (typically from
/// CLI arguments or option values) into strongly-typed values of type <see cref="T"/>.
/// </summary>
/// <typeparam name="T">The type of value this parser produces.</typeparam>
public interface IValueParser<T>
{
    /// <summary>
    /// The name for this parser, used in help messages to indicate what kind
    /// of value this parser expects. Usually a single word in uppercase,
    /// like `PORT` or `FILE`.
    /// </summary>
    public string? Name { get; }
    
    /// <summary>
    /// Parses a string input into a value of type <see cref="T"/>.
    /// </summary>
    /// <param name="input">The string input to parse (e.g., the `value` part of `--option=value`)</param>
    /// <returns>A result object indicating success or failure with an error message.</returns>
    ValueParserResult<T> Parse(string input);
    
    /// <summary>
    /// Formats a value of type <see cref="T"/> into a string representation.
    /// This is useful for displaying the value in help messages or documentation.
    /// </summary>
    /// <param name="value">The value to format.</param>
    /// <returns>A string representation of the value.</returns>
    string Format(T value);
}