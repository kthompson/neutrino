using System.Text.RegularExpressions;
using static Neutrino.MessageSyntax;

namespace Neutrino.ValueParsers;

class StringValueParser : IValueParser<string>
{
    public string? Name { get; }
    private readonly Regex? _regex;
    private readonly string? _pattern;
    
    public StringValueParser(string? pattern = null, string? name = null)
    {
        Name = name;
        _pattern = pattern;
        _regex = pattern == null ? null : new Regex(pattern);
    }

    public StringValueParser(Regex regex, string? name = null)
    {
        Name = name;
        _regex = regex;
        _pattern = regex.ToString();
    }

    public ValueParserResult<string> Parse(string input)
    {
        if (_regex == null || _regex.IsMatch(input))
            return ValueParserResult.Success(input);

        return ValueParserResult.Failure(
            MessageFromTerms(
                this.Name == null ? Text("Input") : Metavar(this.Name),
                Text(" does not match required pattern: "),
                Value(input),
                Text($" (pattern: {_pattern})")
            )
        );
    }

    public string Format(string value)
    {
        return value;
    }
}
