using System.Text.RegularExpressions;
using static Neutrino.MessageSyntax;

namespace Neutrino;

public static partial class Syntax
{
    public static ValueParser<Guid> Uuid() =>
        ValueParser.Create(input => Guid.TryParse(input, out var guid)
            ? ValueParserResult.Success(guid)
            : ValueParserResult.Failure(
                MessageFromTerms(
                    Text("Invalid GUID: "),
                    Value(input)
                )
            ));

    public static ValueParser<string> String(string? pattern = null, string? name = null) =>
        String(pattern == null ? null : new Regex(pattern), name);

    public static ValueParser<string> String(Regex? regex, string? name = null) =>
        ValueParser.Create(input => regex == null || regex.IsMatch(input)
            ? ValueParserResult.Success(input)
            : ValueParserResult.Failure(
                MessageFromTerms(
                    name == null ? Text("Input") : Metavar(name),
                    Text(" does not match required pattern: "),
                    Value(input),
                    Text($" (pattern: {regex})")
                )
            ));
        
    
    public static ValueParser<int> Int(int? min = null, int? max = null)
    {
        if (min.HasValue && max.HasValue && max.Value < min.Value)
            throw new ArgumentException("max must be greater than or equal to min");

        return ValueParser.Create(input =>
        {
            if (!int.TryParse(input, out var value))
            {
                return ValueParserResult.Failure(
                    MessageFromTerms(
                        Text("Input is not a valid integer: "),
                        Value(input)
                    )
                );
            }
            if (min.HasValue && value < min.Value)
            {
                return ValueParserResult.Failure(
                    MessageFromTerms(
                        Text($"Value must be at least {min.Value}: "),
                        Value(input)
                    )
                );
            }
            if (max.HasValue && value > max.Value)
            {
                return ValueParserResult.Failure(
                    MessageFromTerms(
                        Text($"Value must be at most {max.Value}: "),
                        Value(input)
                    )
                );
            }
            return ValueParserResult.Success(value);
        });
    }

    public static ValueParser<float> Float(float? min = null, float? max = null)
    {
        if (min.HasValue && max.HasValue && max.Value < min.Value)
            throw new ArgumentException("max must be greater than or equal to min");

        return ValueParser.Create(input =>
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

            if (min.HasValue && value < min.Value)
            {
                return ValueParserResult.Failure(
                    MessageFromTerms(
                        Text($"Value must be at least {min.Value}: "),
                        Value(input)
                    )
                );
            }

            if (max.HasValue && value > max.Value)
            {
                return ValueParserResult.Failure(
                    MessageFromTerms(
                        Text($"Value must be at most {max.Value}: "),
                        Value(input)
                    )
                );
            }

            return ValueParserResult.Success(value);
        });
    }
}