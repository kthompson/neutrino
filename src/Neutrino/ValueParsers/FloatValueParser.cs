using static Neutrino.MessageSyntax;

namespace Neutrino.ValueParsers;

class FloatValueParser : IValueParser<float>
{
    public string? Name { get; }
    
    private readonly float? _min;
    private readonly float? _max;

    public FloatValueParser(string? name = null, float? min = null, float? max = null)
    {
        if (min.HasValue && max.HasValue && max.Value < min.Value)
            throw new ArgumentException("max must be greater than or equal to min");
        Name = name;
        _min = min;
        _max = max;
    }

    public ValueParserResult<float> Parse(string input)
    {
        if (!float.TryParse(input, out var value))
        {
            return ValueParserResult.Failure(
                MessageFromTerms(
                    Text("Input is not a valid float: "),
                    Value(input)
                )
            );
        }
        if (_min.HasValue && value < _min.Value)
        {
            return ValueParserResult.Failure(
                MessageFromTerms(
                    Text($"Value must be at least {_min.Value}: "),
                    Value(input)
                )
            );
        }
        if (_max.HasValue && value > _max.Value)
        {
            return ValueParserResult.Failure(
                MessageFromTerms(
                    Text($"Value must be at most {_max.Value}: "),
                    Value(input)
                )
            );
        }
        return ValueParserResult.Success(value);
    }

    public string Format(float value)
    {
        return value.ToString();
    }
}
