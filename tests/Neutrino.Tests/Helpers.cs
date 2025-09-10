using static Neutrino.Syntax;

namespace Neutrino.Tests;

public class Helpers
{
    public static void AssertRun<TValue>(TValue expected, Parser<TValue> parser, params IReadOnlyList<string> arguments) => 
        AssertRun(expected, parser, new RunOptions<TValue> { Arguments = arguments });

    public static void AssertRun<TValue>(TValue expected, Parser<TValue> parser,
        RunOptions<TValue>? options = null)
    {
        var result = Run(parser, options);
        var success = Assert.IsAssignableFrom<Result<TValue>.Success>(result);
        Assert.Equal(expected, success.Value);
    } 
        
}