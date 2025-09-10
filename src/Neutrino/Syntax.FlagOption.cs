namespace Neutrino;

using static MessageSyntax;
public static partial class Syntax
{
    static Parser<bool> FlagOption(IReadOnlyList<OptionName> optionNames, Func<Reader<string>, ParserResult<bool>> defaultFunc)
    {
        var options = optionNames.Select(n => n.Name).ToArray();
        return Parser.Create(input =>
        {
            if (input.IsEmpty)
            {
                return ParserResult.Failure(
                    MessageFromTerms(
                        Text("Expected an option, but got end of input.")
                    ),
                    input
                );
            }

            // When the input contains `--` it is a signal to stop parsing
            // options and treat the rest as positional arguments.
            var current = input.Head;
            if (current == "--")
            {
                return ParserResult.OptionsTerminated(input.Tail);
            }

            // When the input is split by spaces, the first element is the option name
            // E.g., `--option value` or `/O value`

            if (options.Contains(current))
            {
                return ParserResult.Success(true, input.Tail);
            }

            // When the input is not split by spaces, but joined by = or :
            // E.g., `--option=value` or `/O:value`
            var prefixes = options.Where(nam => nam.StartsWith("--") || nam.StartsWith('/'))
                .Select(nam => nam.StartsWith('/') ? $"{nam}:" : $"{nam}=")
                .ToArray();

            foreach (var prefix in prefixes)
            {
                if (!current.StartsWith(prefix)) continue;

                var value = current[prefix.Length..];

                return ParserResult.Failure(
                    MessageFromTerms(
                        Text("Option "),
                        OptionName(prefix),
                        Text(" is a bool flag but got a value: "),
                        Value(value),
                        Text(".")
                    ),
                    input
                );
            }

            return defaultFunc(input);
        }, new Usage([
            new UsageTerm.Optional(new Usage(
                [new UsageTerm.Option(null, optionNames)]
            ))
        ]));
    }
    
    public static Parser<bool> Flag(OptionName name, params OptionName[] optionNames) =>
        FlagOption([name, ..optionNames], input => ParserResult.Failure(MessageFromTerms(
                Text("No matched option for "),
                OptionName(input.Head),
                Text(".")
            ),
            input
        ));
    
    
    
    public static Parser<bool> Option(OptionName name, params OptionName[] optionNames) =>
        FlagOption([name, ..optionNames], input => ParserResult.Success(false, input));
}