namespace Neutrino;

using static MessageSyntax;

public static partial class Syntax
{
    public static Parser<Tuple<T1, T2>> Options<T1, T2>(Parser<T1> p1, Parser<T2> p2) =>
        p1.SelectMany(t1 => p2.Select(t2 => Tuple.Create(t1, t2)))
            .OrElse(p2.SelectMany(t2 => p1.Select(t1 => Tuple.Create(t1, t2))))
            .WithUsage(new Usage([new UsageTerm.Exclusive([p1.Usage, p2.Usage])]));
}