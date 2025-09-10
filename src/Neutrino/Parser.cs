namespace Neutrino;

/// <summary>
/// IParser interface for command-line argument parsing.
/// </summary>
/// <typeparam name="TValue">The type of the value returned by the parser.</typeparam>
/// <typeparam name="TState"></typeparam>
public class Parser<TValue>(Func<Reader<string>, ParserResult<TValue>> func, Usage? usage = null)
{
    /// <summary>
    /// The usage information for this parser, which describes how to use it in command-line interfaces.
    /// </summary>
    public Usage Usage { get; } = usage ?? Usage.Empty;

    /// <summary>
    /// Parses the input context and returns a result indicating whether the parsing was successful or not.
    /// </summary>
    /// <param name="input">
    ///     The context of the parser, which includes the input buffer and the current state.
    /// </param>
    /// <returns>
    /// A result object indicating success or failure.
    /// </returns>
    public ParserResult<TValue> Parse(Reader<string> input) => 
        func(input);
    
    public Parser<TResult> SelectMany<TResult>(Func<TValue, Parser<TResult>> f) =>
        new Parser<TResult>(
            input => this.Parse(input) switch
            {
                ParserResult< TValue>.ParserSuccess(var value, var remainingInput) =>
                    f(value).Parse(remainingInput),
                ParserResult< TValue>.ParserFailure(var value, var remainingInput) =>
                    ParserResult.Failure(value, remainingInput),
                ParserResult< TValue>.ParserOptionsTerminated(var remainingInput) =>
                    ParserResult.OptionsTerminated(remainingInput),
                _ => throw new ArgumentOutOfRangeException()
            },
            this.Usage
        );
    
    public Parser<TResult> Select<TResult>(Func<TValue, TResult> f) =>
        this.SelectMany(value => Parser.Pure(f(value), Usage));
    
    public Parser<TResult> SelectMany<TParser, TResult>(Func<TValue, Parser<TParser>> parserSelector,
        Func<TValue, TParser, TResult> resultSelector) =>
        this.SelectMany(value =>
            parserSelector(value)
                .SelectMany(tp => Parser.Pure(resultSelector(value, tp), Usage)));
    
    public Parser<TValue> OrElse(Parser<TValue> orElse) =>
        new Parser<TValue>(input => Parse(input) switch
        {
            ParserResult<TValue>.ParserFailure(var error, var remainingInput) =>
                orElse.Parse(input),
            ParserResult<TValue>.ParserSuccess(var value, var remainingInput) =>
                ParserResult.Success(value, remainingInput),
            ParserResult<TValue>.ParserOptionsTerminated(var remainingInput) =>
                ParserResult.OptionsTerminated(remainingInput),
            _ => throw new ArgumentOutOfRangeException()
        }, Usage);
    
    public Parser<TValue> WithUsage(Usage usage) =>
        new Parser<TValue>(Parse, usage);
}

static class Parser
{
    public static Parser<TValue> Pure<TValue>(TValue value, Usage? usage = null) =>
        new Parser<TValue>(reader => ParserResult.Success(value, reader), usage);
    
    public static Parser<T> Create<T>(Func<Reader<string>, ParserResult<T>> func, Usage? usage = null) =>
        new Parser<T>(func, usage);
}