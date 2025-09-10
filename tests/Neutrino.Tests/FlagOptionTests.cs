using static Neutrino.Syntax;

namespace Neutrino.Tests;

public class FlagOptionTests
{
    [Fact]
    public void Flag_BooleanFlag_SingleName()
    {
        var parser = Flag("--verbose");
        Helpers.AssertRun(true, parser, "--verbose");
    }

    [Fact]
    public void Flag_BooleanFlag_MultipleNames()
    {
        var parser = Flag("--verbose", "-v", "/verbose");
        Helpers.AssertRun(true, parser, "-v");
        Helpers.AssertRun(true, parser, "/verbose");
        Helpers.AssertRun(true, parser, "--verbose");
    }
}