using static Neutrino.MessageSyntax;

namespace Neutrino.ValueParsers;

class GuidValueParser(string? name = null) : IValueParser<Guid>
{
    public string? Name { get; } = name;

    public ValueParserResult<Guid> Parse(string input)
    {
        if (Guid.TryParse(input, out var guid))
        {
            return ValueParserResult.Success(guid);
        }

        return ValueParserResult.Failure<Guid>(
            MessageFromTerms(
                Text("Invalid GUID: "),
                Value(input)
            )
        );
    }

    public string Format(Guid value)
    {
        return value.ToString();
    }
}
