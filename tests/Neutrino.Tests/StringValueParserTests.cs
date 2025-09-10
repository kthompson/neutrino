using System.Text.RegularExpressions;
using static Neutrino.Syntax;
namespace Neutrino.Tests;

public class StringValueParserTests
{
    [Fact]
    public void Parse_NoPattern_AcceptsAnyString()
    {
        var parser = String();
        var result = parser.Parse("hello");
        Assert.IsType<ValueParserResult<string>.Success>(result);
        Assert.Equal("hello", ((ValueParserResult<string>.Success)result).Value);
    }

    [Fact]
    public void Parse_WithPattern_MatchesString()
    {
        var parser = String("^abc$");
        var result = parser.Parse("abc");
        Assert.IsType<ValueParserResult<string>.Success>(result);
        Assert.Equal("abc", ((ValueParserResult<string>.Success)result).Value);
    }

    [Fact]
    public void Parse_WithPattern_DoesNotMatchString()
    {
        var parser = String("^abc$");
        var result = parser.Parse("def");
        Assert.IsType<ValueParserResult<string>.Failure>(result);
    }

    [Fact]
    public void Parse_WithRegexObject_MatchesString()
    {
        var regex = new Regex("^[0-9]+$");
        var parser = String(regex);
        var result = parser.Parse("12345");
        Assert.IsType<ValueParserResult<string>.Success>(result);
        Assert.Equal("12345", ((ValueParserResult<string>.Success)result).Value);
    }

    [Fact]
    public void Parse_WithRegexObject_DoesNotMatchString()
    {
        var regex = new Regex("^[0-9]+$");
        var parser = String(regex);
        var result = parser.Parse("abc");
        Assert.IsType<ValueParserResult<string>.Failure>(result);
    }
}

