using static Neutrino.MessageSyntax;

namespace Neutrino.ValueParsers;

class IntValueParser : IValueParser<int>
{
    public string? Name { get; }
    private readonly int? _min;
    private readonly int? _max;

    public IntValueParser(string? name = null, int? min = null, int? max = null)
    {
        if (min.HasValue && max.HasValue && max.Value < min.Value)
            throw new ArgumentException("max must be greater than or equal to min");
        Name = name;
        _min = min;
        _max = max;
    }

    public ValueParserResult<int> Parse(string input)
    {
        if (!int.TryParse(input, out var value))
        {
            return ValueParserResult.Failure<int>(
                MessageFromTerms(
                    Text("Input is not a valid integer: "),
                    Value(input)
                )
            );
        }
        if (_min.HasValue && value < _min.Value)
        {
            return ValueParserResult.Failure<int>(
                MessageFromTerms(
                    Text($"Value must be at least {_min.Value}: "),
                    Value(input)
                )
            );
        }
        if (_max.HasValue && value > _max.Value)
        {
            return ValueParserResult.Failure<int>(
                MessageFromTerms(
                    Text($"Value must be at most {_max.Value}: "),
                    Value(input)
                )
            );
        }
        return ValueParserResult.Success(value);
    }

    public string Format(int value)
    {
        return value.ToString();
    }
}
