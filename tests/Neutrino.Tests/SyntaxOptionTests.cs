using static Neutrino.Syntax;
using static Neutrino.Tests.Helpers;

namespace Neutrino.Tests;

public class SyntaxOptionTests
{
    [Fact]
    public void Option_SingleName_CreatesOptionParser()
    {
        var parser = Option("--verbose", String());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);

        AssertRun("true", parser, "--verbose", "true");
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
        
        AssertRun("result.txt", parser, "-o", "result.txt");
        AssertRun("data.log", parser, "/out", "data.log");
        AssertRun("data.log", parser, "/out:data.log");
        AssertRun("final.md", parser, "--output", "final.md");
        AssertRun("final.md", parser, "--output=final.md");
    }

    [Fact]
    public void Option_FourNames_CreatesOptionParser()
    {
        var parser = Option("--config", "-c", "/config", "+cfg", String());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
        
        AssertRun("settings.yaml", parser, "+cfg", "settings.yaml");
        AssertRun("app.json", parser, "/config", "app.json");
        AssertRun("app.json", parser, "/config:app.json");
        AssertRun("default.ini", parser, "-c", "default.ini");
        AssertRun("prod.env", parser, "--config", "prod.env");
        AssertRun("prod.env", parser, "--config=prod.env");
    }

    [Fact]
    public void Option_WithIntValueParser_CreatesTypedParser()
    {
        var parser = Option("--port", "-p", Int(min: 1, max: 65535));
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
        
        AssertRun(8080, parser, "-p", "8080");
        AssertRun(443, parser, "--port", "443");
        AssertRun(3000, parser, "--port=3000");
    }

    [Fact]
    public void Option_WithFloatValueParser_CreatesTypedParser()
    {
        var parser = Option("--threshold", "-t", Float(min: 0.0f, max: 1.0f));
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
        
        AssertRun(0.75f, parser, "-t", "0.75");
        AssertRun(0.5f, parser, "--threshold", "0.5");
        AssertRun(0.9f, parser, "--threshold=0.9");
    }

    [Fact]
    public void Option_WithGuidValueParser_CreatesTypedParser()
    {
        var parser = Option("--id", Uuid());
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
        
        var testGuid = Guid.NewGuid();

        AssertRun(testGuid, parser, "--id", testGuid.ToString());
        AssertRun(testGuid, parser, $"--id={testGuid}");
    }

    [Fact]
    public void Option_WithStringPattern_CreatesConstrainedParser()
    {
        var parser = Option("--log-level", String("^(debug|info|warn|error)$"));
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
        
        AssertRun("debug", parser, "--log-level", "debug");
        AssertRun("info", parser, "--log-level=info");
        AssertRun("error", parser, "--log-level", "error");
        
        // Invalid case
        // Assert.Throws<ArgumentException>(() => Run(parser, new RunOptions { Arguments = ["--log-level", "verbose"] }));
    }

    [Fact]
    public void Option_MultipleNamesWithComplexValueParser_CreatesParser()
    {
        var parser = Option("--timeout", "-t", "/timeout", Int(min: 1, max: 3600));
        
        Assert.NotNull(parser);
        Assert.NotNull(parser.Usage);
        AssertRun(120, parser, "-t", "120");
        AssertRun(300, parser, "/timeout", "300");
        AssertRun(600, parser, "/timeout:600");
        AssertRun(1800, parser, "--timeout", "1800");
        AssertRun(3600, parser, "--timeout=3600");
        
        // Invalid cases
        // Assert.Throws<ArgumentException>(() => Run(parser, new RunOptions { Arguments = ["-t", "0"] }));
        // Assert.Throws<ArgumentException>(() => Run(parser, new RunOptions { Arguments = ["/timeout", "4000"] }));
        // Assert.Throws<ArgumentException>(() => Run(parser, new RunOptions { Arguments = ["--timeout=-5"] }));
    }

    [Fact]
    public void Option_WithArrayOfNames_CreatesParser()
    {
        var names = new OptionName[] { "--file", "-f", "/file"};
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
        
        AssertRun("true", parser, optionName, "true");
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
        
        Assert.IsAssignableFrom<Parser<string>>(parser1);
        Assert.IsAssignableFrom<Parser<string>>(parser2);
        Assert.IsAssignableFrom<Parser<string>>(parser3);
        Assert.IsAssignableFrom<Parser<string>>(parser4);
    }

    [Fact]
    public void Option_DifferentValueParserTypes_MaintainTypeConstraints()
    {
        var stringParser = Option("--str", String());
        var intParser = Option("--num", Int());
        var floatParser = Option("--float", Float());
        var guidParser = Option("--guid", Uuid());
        
        Assert.IsAssignableFrom<Parser<string>>(stringParser);
        Assert.IsAssignableFrom<Parser<int>>(intParser);
        Assert.IsAssignableFrom<Parser<float>>(floatParser);
        Assert.IsAssignableFrom<Parser<Guid>>(guidParser);
    }
}
