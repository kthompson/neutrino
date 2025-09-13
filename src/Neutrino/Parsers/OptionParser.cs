using Neutrino.Messages;

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

    public ValueParserResult<T> InitialState => ValueParserResult.Failure(
        MessageFromTerms(
            Text("Missing option "),
            OptionNames(_optionNames.Select(n => n.Name).ToArray()), 
            Text(".")
        ));
    
    public ParserResult<ValueParserResult<T>> Parse(ParserContext<ValueParserResult<T>> context)
    {
        if (context.OptionsTerminated)
            return ParserResult.Failure(
                0,
                MessageFromTerms(
                    Text("No more options can be parsed.")
                )
            );
        
        if(context.Buffer.Count == 0)
            return ParserResult.Failure(
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
                new ParserContext<ValueParserResult<T>> {
                    Buffer = context.Buffer[1..],
                    State = context.State,
                    OptionsTerminated = true
                },
                context.Buffer[..^1]
            );
        
        // When the input is split by spaces, the first element is the option name
        // E.g., `--option value` or `/O value`
        
        if(_options.Contains(currentArg))
        {
            if (context.State is ValueParserResult<T>.Success)
            {
                return ParserResult.Failure(
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
                return ParserResult.Failure(
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
                new ParserContext<ValueParserResult<T>> {
                    OptionsTerminated = context.OptionsTerminated,
                    Buffer = context.Buffer[2..],
                    State = valueResult,
                },
                context.Buffer[..2]
            );
        }
        
        var prefixes = _options.Where(name => name.StartsWith("--") || name.StartsWith('/'))
            .Select(name => name.StartsWith('/') ? $"{name}:" : $"{name}=")
            .ToArray();

        foreach (var prefix in prefixes)
        {
            if(!currentArg.StartsWith(prefix)) continue;
            
            if (context.State is ValueParserResult<T>.Success)
            {
                return ParserResult.Failure(
                    1,
                    MessageFromTerms(
                        Text("Option "),
                        OptionNames(_options),
                        Text(" was already provided.")
                    )
                );
            }
            
            var value = currentArg[prefix.Length..];
            
            var result = _valueParser.Parse(value);
            return ParserResult.Success(
                new ParserContext<ValueParserResult<T>> {
                    Buffer = context.Buffer[1..],
                    State = result,
                    OptionsTerminated = true
                },
                context.Buffer[..1]
            );
            
        }
        
        return ParserResult.Failure(
            0,
            MessageFromTerms(
                Text("No matched option for "),
                OptionName(currentArg),
                Text(".")
            )
        );
    }

    public ValueParserResult<T> Complete(ValueParserResult<T> state) =>
        state switch
        {
            ValueParserResult<T>.Success => state,
            ValueParserResult<T>.Failure failure =>
                ValueParserResult.Failure(
                    MessageFromList([OptionNames(_options), .. failure.Error.Terms])
                ),
            _ => throw new ArgumentOutOfRangeException(nameof(state), state, null)
        };
}


class OptionParser : IParser<bool, ValueParserResult<bool>>
{
    private readonly string[] _options;
    private readonly IReadOnlyList<OptionName> _optionNames;

    public OptionParser(IReadOnlyList<OptionName> optionNames)
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

    public ValueParserResult<bool> InitialState => ValueParserResult.Success(true);
    
    public ParserResult<ValueParserResult<bool>> Parse(ParserContext<ValueParserResult<bool>> context)
    {
        if (context.OptionsTerminated)
            return ParserResult.Failure(
                0,
                MessageFromTerms(
                    Text("No more options can be parsed.")
                )
            );
        
        if(context.Buffer.Count == 0)
            return ParserResult.Failure(
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
                new ParserContext<ValueParserResult<bool>> {
                    Buffer = context.Buffer[1..],
                    State = context.State,
                    OptionsTerminated = true
                },
                context.Buffer[..^1]
            );
        
        // When the input is split by spaces, the first element is the option name
        // E.g., `--option value` or `/O value`
        
        if(_options.Contains(currentArg))
        {
            if (context.State is ValueParserResult<bool>.Success { Value: true })
            {
                return ParserResult.Failure(
                    1,
                    MessageFromTerms(
                        Text("Option "),
                        OptionNames(_options),
                        Text(" was already provided.")
                    )
                );
            }

            return ParserResult.Success(
                new ParserContext<ValueParserResult<bool>>
                {
                    OptionsTerminated = context.OptionsTerminated,
                    Buffer = context.Buffer[1..],
                    State = ValueParserResult.Success(true),
                },
                context.Buffer[..1]
            );
        }
        
        // When the input is not split by spaces, but joined by = or :
        // E.g., `--option=value` or `/O:value`
        var prefixes = _options.Where(name => name.StartsWith("--") || name.StartsWith('/'))
            .Select(name => name.StartsWith('/') ? $"{name}:" : $"{name}=")
            .ToArray();

        foreach (var prefix in prefixes)
        {
            if(!currentArg.StartsWith(prefix)) continue;
            
            if (context.State is ValueParserResult<bool>.Success { Value: true })
            {
                return ParserResult.Failure(
                    1,
                    MessageFromTerms(
                        Text("Option "),
                        OptionNames(_options),
                        Text(" was already provided.")
                    )
                );
            }
            
            var value = currentArg[prefix.Length..];
            
            return ParserResult.Failure(
                1,
                MessageFromTerms(
                    Text("Option "),
                    OptionName(prefix),
                    Text(" is a bool flag but got a value: "),
                    Value(value),
                    Text(".")
                )
            );
        }
        
        return ParserResult.Failure(
            0,
            MessageFromTerms(
                Text("No matched option for "),
                OptionName(currentArg),
                Text(".")
            )
        );
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