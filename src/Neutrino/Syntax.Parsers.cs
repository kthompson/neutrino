using System.Text.RegularExpressions;
using static Neutrino.MessageSyntax;

namespace Neutrino;

public static partial class Syntax
{
    
    public static Parser<T> Constant<T>(T value) => Parser.Pure(value);

    public static Result<TValue> Run<TValue>(Parser<TValue> parser, RunOptions<TValue>? options = null)
    {
        var runner = new Runner<TValue>(parser, options ?? new RunOptions<TValue>());
        return runner.Run();
    }

}