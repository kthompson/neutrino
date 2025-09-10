namespace Neutrino.Parsers;

using static MessageSyntax;

class OptionParser<T> : IParser<T, ValueParserResult<T>>
{
    private readonly string[] _options;
    private readonly IReadOnlyList<OptionName> _optionNames;
    private readonly IValueParser<T> _valueParser;

    public OptionParser(IReadOnlyList<OptionName> optionNames, IValueParser<T> valueParser)
    {
        if (optionNames.Count == 0)
            throw new InvalidOperationException("At least one option name must be provided.");
                
        _optionNames = optionNames;
        _valueParser = valueParser;
        _options = optionNames.Select(n => n.Name).ToArray();
    }

    public Usage Usage => new Usage([
        new UsageTerm.Option(_valueParser.Name,  _optionNames)
    ]);

    public ValueParserResult<T> InitialState => ValueParserResult.Failure<T>(
        MessageFromTerms(
            Text("Missing option "),
            OptionNames(_optionNames.Select(n => n.Name).ToArray()), 
            Text(".")
        ));
    
    public ParserResult<ValueParserResult<T>> Parse(ParserContext<ValueParserResult<T>> context)
    {
        if (context.OptionsTerminated)
            return ParserResult.Failure<ValueParserResult<T>>(
                0,
                MessageFromTerms(
                    Text("No more options can be parsed.")
                )
            );
        
        if(context.Buffer.Count == 0)
            return ParserResult.Failure<ValueParserResult<T>>(
                0,
                MessageFromTerms(
                    Text("Expected an option, but got end of input.")
                )
            );

        // When the input contains `--` it is a signal to stop parsing
        // options and treat the rest as positional arguments.
        var currentArg = context.Buffer[0];
        if (currentArg == "--")
            return ParserResult.Success(
                new ParserContext<ValueParserResult<T>>(
                    context.Buffer[1..],
                    context.State,
                    true
                ),
                context.Buffer[..^1]
            );
        
        // When the input is split by spaces, the first element is the option name
        // E.g., `--option value` or `/O value`
        
        if(_options.Contains(currentArg))
        {
            if (context.State is ValueParserResult<T>.Success)
            {
                return ParserResult.Failure<ValueParserResult<T>>(
                    1,
                    MessageFromTerms(
                        Text("Option "),
                        OptionNames(_options),
                        Text(" was already provided.")
                    )
                );
            }
            
            // We found a matching option name, now we need to parse the value
            if(context.Buffer.Count < 2)
            {
                return ParserResult.Failure<ValueParserResult<T>>(
                    1,
                    MessageFromTerms(
                        Text("Option "),
                        OptionName(_options[0]),
                        Text(" requires a value, but none was provided.")
                    )
                );
            }
            
            var valueArg = context.Buffer[1];
            var valueResult = _valueParser.Parse(valueArg);

            return ParserResult.Success(
                new ParserContext<ValueParserResult<T>>(
                    context.Buffer[2..],
                    valueResult,
                    true
                ),
                context.Buffer[..2]
            );
        }
        
        // https://github.com/dahlia/optique/blob/main/packages/core/src/parser.ts#L395
        
        throw new NotImplementedException();
    }

    public ValueParserResult<T> Complete(ValueParserResult<T> state)
    {
        throw new NotImplementedException();
    }
}