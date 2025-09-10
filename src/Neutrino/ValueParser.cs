namespace Neutrino;
/// <summary>
/// Interface for parsing CLI option values and arguments.
/// A `ValueParser` is responsible for converting string input (typically from
/// CLI arguments or option values) into strongly-typed values of type <see cref="TValue"/>.
/// </summary>
/// <typeparam name="TValue">The type of value this parser produces.</typeparam>
public class ValueParser<TValue>(Func<string, ValueParserResult<TValue>> f, string? name = null)
{
    public string? Name { get; } = name;
    
    /// <summary>
    /// Parses a string input into a value of type <see cref="TValue"/>.
    /// </summary>
    /// <param name="input">The string input to parse (e.g., the `value` part of `--option=value`)</param>
    /// <returns>A result object indicating success or failure with an error message.</returns>
    public ValueParserResult<TValue> Parse(string input) => f(input);
    
    public ValueParser<TResult> SelectMany<TResult>(Func<TValue, ValueParser<TResult>> f) =>
        new ValueParser<TResult>(
            input => this.Parse(input) switch
            {
                ValueParserResult<TValue>.Success(var value) =>
                    f(value).Parse(input),
                ValueParserResult< TValue>.Failure(var error) =>
                    ValueParserResult.Failure(error),
                _ => throw new ArgumentOutOfRangeException()
            }
        );
    
    public ValueParser<TResult> Select<TResult>(Func<TValue, TResult> f) =>
        this.SelectMany(value => ValueParser.Pure(f(value)));
    
    public ValueParser<TResult> SelectMany<TParser, TResult>(Func<TValue, ValueParser<TParser>> parserSelector,
        Func<TValue, TParser, TResult> resultSelector) =>
        this.SelectMany(value =>
            parserSelector(value)
                .SelectMany(tp => ValueParser.Pure(resultSelector(value, tp))));

    public ValueParser<TValue> OrElse(ValueParser<TValue> orElse) =>
        new ValueParser<TValue>(input => Parse(input) switch
        {
            ValueParserResult<TValue>.Failure(var error) =>
                orElse.Parse(input),
            ValueParserResult<TValue>.Success(var value) =>
                ValueParserResult.Success(value),
            _ => throw new ArgumentOutOfRangeException()
        });

    public ValueParser<TValue> Named(string name) =>
        new ValueParser<TValue>(f, name);
}

static class ValueParser
{
    public static ValueParser<TValue> Pure<TValue>(TValue value) =>
        new ValueParser<TValue>(input => ValueParserResult.Success(value));

    public static ValueParser<T> Create<T>(Func<string, ValueParserResult<T>> f) => 
        new ValueParser<T>(f);
}