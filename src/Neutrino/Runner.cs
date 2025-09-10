namespace Neutrino;

using static MessageSyntax;

class Runner<TValue>(Parser<TValue> parser, RunOptions<TValue> options)
{
    public Result<TValue> Run()
    {
        // TODO: add help command and other features
        
        var buffer = Reader.FromArray(options.Arguments);

        return parser.Parse(buffer) switch
        {
            ParserResult<TValue>.ParserFailure failure => Result.Failure(failure.Error),
            ParserResult<TValue>.ParserSuccess success => Result.Success(success.Value),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}
