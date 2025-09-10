using static Neutrino.Syntax;

namespace Neutrino.Tests;

public class BoolOptionTests
{
    [Fact]
    public void Option_BooleanFlag_SingleName()
    {
        var parser = Option("--debug");
        
        Helpers.AssertRun(true, parser, "--debug");
        Helpers.AssertRun(false, parser);
    }

    [Theory]
    [InlineData(true, "-d")]
    [InlineData(true, "/debug")]
    [InlineData(true, "--debug")]
    public void Option_BooleanFlag_MultipleNames(bool result, string command)
    {
        var parser = Option("--debug", "-d", "/debug");

        Helpers.AssertRun(true, parser, "-d");
        Helpers.AssertRun(true, parser, "/debug");
        Helpers.AssertRun(true, parser, "--debug");
    }
    
    [Fact]
    public void Option_BooleanFlag_MultipleNamesNoArgs()
    {
        var parser = Option("--debug", "-d", "/debug");

        Helpers.AssertRun(false, parser);
    }

    [Fact]
    public void Option_BooleanFlag_ShouldHandleEmptyBuffer()
    {
        var parser = Option("--debug");
        var result = parser.Parse(Reader.FromArray<string>([]));
        
        var failure = Assert.IsType<ParserResult<bool>.ParserFailure>(result);
        
        Assert.Equal("Expected an option, but got end of input.", failure.Error.ToString());
    }
}