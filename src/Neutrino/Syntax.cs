using System.Text.RegularExpressions;
using Neutrino.Parsers;
using Neutrino.ValueParsers;

namespace Neutrino;

public static class Syntax
{
    public static IValueParser<Guid> Uuid() => new GuidValueParser();

    public static IValueParser<string> String(string? pattern = null, string? name = null) =>
        new StringValueParser(pattern, name);
    public static IValueParser<string> String(Regex regex, string? name = null) =>
        new StringValueParser(regex, name);

    
    public static IValueParser<int> Int(int? min = null, int? max = null, string? name = null) => new IntValueParser(name, min, max);
    
    public static IValueParser<float> Float(float? min = null, float? max = null, string? name = null) => new FloatValueParser(name, min, max);
    
    
    public static IParser<T, T> Constant<T>(T value) => new ConstantParser<T>(value);

    public static IParser<bool, ValueParserResult<bool>> Option(OptionName name, params OptionName[] otherNames) =>
        throw new NotImplementedException();

    public static IParser<T, ValueParserResult<T>> Option<T>(IReadOnlyList<OptionName> names, IValueParser<T> value) =>
        new OptionParser<T>(names, value);

    public static IParser<T, ValueParserResult<T>> Option<T>(OptionName name, IValueParser<T> value) =>
        Option([name], value);

    public static IParser<T, ValueParserResult<T>>
        Option<T>(OptionName name, OptionName name2, IValueParser<T> value) => Option([name, name2], value);

    public static IParser<T, ValueParserResult<T>> Option<T>(OptionName name, OptionName name2, OptionName name3,
        IValueParser<T> value) => Option([name, name2, name3], value);

    public static IParser<T, ValueParserResult<T>> Option<T>(OptionName name, OptionName name2, OptionName name3,
        OptionName name4, IValueParser<T> value) => Option([name, name2, name3, name4], value);



}


