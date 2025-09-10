namespace Neutrino;

using static MessageSyntax;

public static partial class Syntax
{
    public static Parser<T> Option<T>(IReadOnlyList<OptionName> optionNames, ValueParser<T> valueParser)
    {
        if (optionNames.Count == 0)
            throw new InvalidOperationException("At least one option name must be provided.");

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
            var currentArg = input.Head;
            if (currentArg == "--")
                return ParserResult.OptionsTerminated(input.Tail);

            // When the input is split by spaces, the first element is the option name
            // E.g., `--option value` or `/O value`

            if (options.Contains(currentArg))
            {
                // We found a matching option name, now we need to parse the value

                var rest = input.Tail;

                if (rest.IsEmpty)
                {
                    return ParserResult.Failure(
                        MessageFromTerms(
                            Text("Option "),
                            OptionName(options[0]),
                            Text(" requires a value, but none was provided.")
                        ),
                        rest
                    );
                }

                var valueArg = rest.Head;
                return valueParser.Parse(valueArg) switch
                {
                    ValueParserResult<T>.Success(var value) => ParserResult.Success(value, rest.Tail),
                    ValueParserResult<T>.Failure failure => ParserResult.Failure(
                        MessageFromList([
                            Text("Failed to parse value for option "), OptionNames(options), Text(": "),
                            .. failure.Error.Terms
                        ]), rest),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            var prefixes = options.Where(name => name.StartsWith("--") || name.StartsWith('/'))
                .Select(name => name.StartsWith('/') ? $"{name}:" : $"{name}=")
                .ToArray();

            foreach (var prefix in prefixes)
            {
                if (!currentArg.StartsWith(prefix)) continue;

                var valueString = currentArg[prefix.Length..];

                return valueParser.Parse(valueString) switch
                {
                    ValueParserResult<T>.Success(var value) => ParserResult.Success(value, input.Tail),
                    ValueParserResult<T>.Failure failure => ParserResult.Failure(
                        MessageFromList([
                            Text("Failed to parse value for option "), OptionNames(options), Text(": "),
                            .. failure.Error.Terms
                        ]), input),
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return ParserResult.Failure(
                MessageFromTerms(
                    Text("No matched option for "),
                    OptionName(currentArg),
                    Text(".")
                ),
                input
            );
        }, new Usage([
            new UsageTerm.Option(valueParser.Name, optionNames)
        ]));
    }


    public static Parser<T> Option<T>(OptionName name, ValueParser<T> value) => Option([name], value);

    public static Parser<T> Option<T>(OptionName name, OptionName name2, ValueParser<T> value) =>
        Option([name, name2], value);

    public static Parser<T> Option<T>(OptionName name, OptionName name2, OptionName name3,
        ValueParser<T> value) => Option([name, name2, name3], value);

    public static Parser<T> Option<T>(OptionName name, OptionName name2, OptionName name3,
        OptionName name4, ValueParser<T> value) => Option([name, name2, name3, name4], value);
}