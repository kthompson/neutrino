using static Neutrino.Syntax;

namespace Neutrino.Tests;

public class Helpers
{
    
    public static void AssertRun<TValue, TState>(TValue expected, IParser<TValue, TState> parser, params IReadOnlyList<string> arguments) => 
        AssertRun(expected, parser, new RunOptions<TValue> { Arguments = arguments });
    
    public static void AssertRun<TValue, TState>(TValue expected, IParser<TValue, TState> parser, RunOptions<TValue>? options = null) => 
            Assert.Equal(expected, Run(parser, options));
}