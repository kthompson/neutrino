using static Neutrino.Syntax;

namespace Neutrino.Tests;

public class SyntaxOptionTests
{
    [Fact]
    public void Option_SingleName_CreatesOptionParser()
    {
        var parser = Option("--verbose", String());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_TwoNames_CreatesOptionParser()
    {
        var parser = Option("--help", "-h", String());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_ThreeNames_CreatesOptionParser()
    {
        var parser = Option("--output", "-o", "/out", String());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_FourNames_CreatesOptionParser()
    {
        var parser = Option("--config", "-c", "/config", "+cfg", String());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_WithIntValueParser_CreatesTypedParser()
    {
        var parser = Option("--port", "-p", Int(min: 1, max: 65535));
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_WithFloatValueParser_CreatesTypedParser()
    {
        var parser = Option("--threshold", "-t", Float(min: 0.0f, max: 1.0f));
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_WithGuidValueParser_CreatesTypedParser()
    {
        var parser = Option("--id", Uuid());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_WithStringPattern_CreatesConstrainedParser()
    {
        var parser = Option("--log-level", String("^(debug|info|warn|error)$"));
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_MultipleNamesWithComplexValueParser_CreatesParser()
    {
        var parser = Option("--timeout", "-t", "/timeout", Int(min: 1, max: 3600, name: "seconds"));
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_WithArrayOfNames_CreatesParser()
    {
        OptionName[] names = ["--file", "-f", "/file"];
        var parser = Option(names, String());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Theory]
    [InlineData("--verbose")]
    [InlineData("-v")]
    [InlineData("/verbose")]
    [InlineData("+v")]
    public void Option_ValidOptionNameFormats_AcceptsAllFormats(string optionName)
    {
        var parser = Option(optionName, String());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
    }

    [Fact]
    public void Option_InvalidOptionName_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => Option("verbose", String()));
    }

    [Fact]
    public void Option_EmptyOptionName_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => Option("", String()));
    }

    [Fact]
    public void Option_NullOptionName_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() => Option((string)null!, String()));
    }

    [Fact]
    public void Option_MultipleOverloads_AllReturnSameType()
    {
        var parser1 = Option("--test", String());
        var parser2 = Option("--test", "-t", String());
        var parser3 = Option("--test", "-t", "/test", String());
        var parser4 = Option("--test", "-t", "/test", "+t", String());
        
        Assert.IsAssignableFrom<IParser<string, ValueParserResult<string>>>(parser1);
        Assert.IsAssignableFrom<IParser<string, ValueParserResult<string>>>(parser2);
        Assert.IsAssignableFrom<IParser<string, ValueParserResult<string>>>(parser3);
        Assert.IsAssignableFrom<IParser<string, ValueParserResult<string>>>(parser4);
    }

    [Fact]
    public void Option_DifferentValueParserTypes_MaintainTypeConstraints()
    {
        var stringParser = Option("--str", String());
        var intParser = Option("--num", Int());
        var floatParser = Option("--float", Float());
        var guidParser = Option("--guid", Uuid());
        
        Assert.IsAssignableFrom<IParser<string, ValueParserResult<string>>>(stringParser);
        Assert.IsAssignableFrom<IParser<int, ValueParserResult<int>>>(intParser);
        Assert.IsAssignableFrom<IParser<float, ValueParserResult<float>>>(floatParser);
        Assert.IsAssignableFrom<IParser<Guid, ValueParserResult<Guid>>>(guidParser);
    }
}
