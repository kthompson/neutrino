namespace Neutrino.Tests;

using static Neutrino.Syntax;
using static Neutrino.Tests.Helpers;

public class OptionsTests
{
    [Fact]
    public void Options_TwoParsers_AcceptsBothInFirstOrder()
    {
        var flagParser = Flag("--verbose");
        var optionParser = Option("--output", String());
        var parser = Options(flagParser, optionParser);
        
        AssertRun(Tuple.Create(true, "result.txt"), parser, "--verbose", "--output", "result.txt");
    }

    [Fact]
    public void Options_TwoParsers_AcceptsBothInSecondOrder()
    {
        var flagParser = Flag("--verbose");
        var optionParser = Option("--output", String());
        var parser = Options(flagParser, optionParser);
        
        AssertRun(Tuple.Create(true, "result.txt"), parser, "--output", "result.txt", "--verbose");
    }

    [Fact]
    public void Options_TwoOptions_WorksWithDifferentTypes()
    {
        var stringOption = Option("--name", String());
        var intOption = Option("--count", Int());
        var parser = Options(stringOption, intOption);
        
        AssertRun(Tuple.Create("test", 42), parser, "--name", "test", "--count", "42");
        AssertRun(Tuple.Create("test", 42), parser, "--count", "42", "--name", "test");
    }

    [Fact]
    public void Options_TwoFlags_WorksWithBooleans()
    {
        var verboseFlag = Flag("--verbose");
        var debugFlag = Flag("--debug");
        var parser = Options(verboseFlag, debugFlag);
        
        AssertRun(Tuple.Create(true, true), parser, "--verbose", "--debug");
        AssertRun(Tuple.Create(true, true), parser, "--debug", "--verbose");
    }

    [Fact]
    public void Options_HasCorrectUsage()
    {
        var flagParser = Flag("--verbose");
        var optionParser = Option("--output", String());
        var parser = Options(flagParser, optionParser);
        
        Assert.NotNull(parser.Usage);
        // The usage should contain an exclusive term with both parser usages
        var term = Assert.Single(parser.Usage.UsageTerms);
        var exclusiveTerm = Assert.IsAssignableFrom<UsageTerm.Exclusive>(term);
        Assert.Equal(2, exclusiveTerm.Terms.Count);
    }

    [Fact]
    public void Options_FailsWhenOnlyFirstParserMatches()
    {
        var flagParser = Flag("--verbose");
        var optionParser = Option("--output", String());
        var parser = Options(flagParser, optionParser);
        
        var result = Run(parser, new RunOptions<Tuple<bool, string>> { Arguments = ["--verbose"] });
        Assert.IsAssignableFrom<Result<Tuple<bool, string>>.Failure>(result);
    }

    [Fact]
    public void Options_FailsWhenOnlySecondParserMatches()
    {
        var flagParser = Flag("--verbose");
        var optionParser = Option("--output", String());
        var parser = Options(flagParser, optionParser);
        
        var result = Run(parser, new RunOptions<Tuple<bool, string>> { Arguments = ["--output", "test.txt"] });
        Assert.IsAssignableFrom<Result<Tuple<bool, string>>.Failure>(result);
    }

    [Fact]
    public void Options_FailsWhenNeitherParserMatches()
    {
        var flagParser = Flag("--verbose");
        var optionParser = Option("--output", String());
        var parser = Options(flagParser, optionParser);
        
        var result = Run(parser, new RunOptions<Tuple<bool, string>> { Arguments = ["--unknown"] });
        Assert.IsAssignableFrom<Result<Tuple<bool, string>>.Failure>(result);
    }

    [Fact]
    public void Options_WorksWithComplexParsers()
    {
        var multiOptionParser = Option("--config", "-c", String());
        var multiFlagParser = Flag("--force", "-f");
        var parser = Options(multiOptionParser, multiFlagParser);
        
        AssertRun(Tuple.Create("config.json", true), parser, "-c", "config.json", "--force");
        AssertRun(Tuple.Create("config.json", true), parser, "-f", "--config", "config.json");
    }

    [Fact]
    public void Options_WorksWithSameOptionNames()
    {
        // Test that Options can handle parsers with different value types but same option names
        var stringVersionOption = Option("--version", String());
        var intBuildOption = Option("--build", Int());
        var parser = Options(stringVersionOption, intBuildOption);
        
        AssertRun(Tuple.Create("1.0.0", 123), parser, "--version", "1.0.0", "--build", "123");
        AssertRun(Tuple.Create("2.1.4", 456), parser, "--build", "456", "--version", "2.1.4");
    }

    [Fact]
    public void Options_WorksWithMultipleArgumentOptions()
    {
        var nameOption = Option("--name", String());
        var pathOption = Option("--path", String());
        var parser = Options(nameOption, pathOption);
        
        AssertRun(Tuple.Create("MyApp", "/usr/local/bin"), parser, "--name", "MyApp", "--path", "/usr/local/bin");
    }
}