namespace Neutrino.Parsers;

using static MessageSyntax;

public class FlagParser: IParser<bool, ValueParserResult<bool>>
{
    
    private readonly string[] _options;
    private readonly IReadOnlyList<OptionName> _optionNames;

    public FlagParser(IReadOnlyList<OptionName> optionNames)
    {
        if (optionNames.Count == 0)
            throw new InvalidOperationException("At least one option name must be provided.");
                
        _optionNames = optionNames;
        _options = optionNames.Select(n => n.Name).ToArray();
    }

    public Usage Usage => new Usage([
        new UsageTerm.Optional(new Usage(
            [new UsageTerm.Option(null, _optionNames)]
        ))
    ]);

    public ValueParserResult<bool> InitialState => ValueParserResult.Failure(
        MessageFromTerms(
            Text("Missing option "),
            OptionNames(_optionNames.Select(n => n.Name).ToArray()), 
            Text(".")
        ));
    
    public ParserResult<ValueParserResult<bool>> Parse(ParserContext<ValueParserResult<bool>> context)
    {
        if (context.OptionsTerminated)
        {
            return ParserResult.Failure(0, MessageFromTerms(Text("No more options can be parsed.")));
        }

        var buffer = context.Buffer;
        if (buffer.Count == 0)
        {
            return ParserResult.Failure(0, MessageFromTerms(Text("Expected an option, but for end of input.")));
        }
        
        var current = buffer[0];
        
        // When the input contains `--` it is a signal to stop parsing
        // options and treat the rest as positional arguments.
        if (current == "--")
        {
            // End of options marker
            return ParserResult.Success(
                new ParserContext<ValueParserResult<bool>>
                {
                    OptionsTerminated = true,
                    Buffer = buffer[1..],
                    State = context.State
                },
                buffer[..1]
            );
        }

        // When the input is split by spaces, the first element is the option name
        
        if (_options.Contains(current))
        {
            if(context.State is ValueParserResult<bool>.Success)
            {
                // Flag already set
                return ParserResult.Failure(1, MessageFromTerms(
                    OptionName(current),
                    Text(" cannot be used multiple times.")
                ));
            }
            
            return ParserResult.Success(
                new ParserContext<ValueParserResult<bool>>
                {
                    OptionsTerminated = context.OptionsTerminated,
                    Buffer = buffer[1..],
                    State = ValueParserResult.Success(true)
                },
                buffer[..1]
            );
        }
        
        // Check for joined format (e.g., --flag=value) which should fail for flags
        var prefixes = _options.Where(name => name.StartsWith("--") || name.StartsWith('/'))
            .Select(name => name.StartsWith('/') ? $"{name}:" : $"{name}=")
            .ToArray();


        foreach (var prefix in prefixes)
        {
            if (!current.StartsWith(prefix)) continue;
            
            var value = current[prefix.Length..];
            return ParserResult.Failure(1, MessageFromTerms(
                Text("Flag "),
                OptionName(prefix[..^1]),
                Text(" does not accept a value, but got: "),
                Value(value),
                Text(".")
            ));
        }
        
        return ParserResult.Failure(0, MessageFromTerms(
            Text("No matched option for "),
            OptionName(current),
            Text(".")
        ));
    }

    public ValueParserResult<bool> Complete(ValueParserResult<bool> state) =>
        state switch
        {
            ValueParserResult<bool>.Success => state,
            ValueParserResult<bool>.Failure failure =>
                ValueParserResult.Failure(
                    MessageFromList([OptionNames(_options), .. failure.Error.Terms])
                ),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
}