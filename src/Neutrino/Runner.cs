namespace Neutrino;

using static MessageSyntax;

class Runner<TValue, TState>(IParser<TValue, TState> parser, RunOptions<TValue> options)
{

    private Result<TValue> Parse()
    {
        var buffer = new ArraySegment<string>(options.Arguments.ToArray());
        var context = new ParserContext<TState>
        {
            Buffer = buffer,
            State = parser.InitialState,
            OptionsTerminated = false
        };
        
        while (true)
        {
            var result = parser.Parse(context);
            switch (result)
            {
                case ParserResult<TState>.ParserFailure failure:
                    return Result.Failure(failure.Error);
                case ParserResult<TState>.ParserSuccess success:
                    var previousBuffer = context.Buffer;
                    context = success.Next;

                    if (context.Buffer.Count > 0 && context.Buffer.Count == previousBuffer.Count &&
                        context.Buffer[0] == previousBuffer[0])
                    {
                        return Result.Failure(MessageFromTerms(
                            Text("Unexpected option or argument: "),
                            Value(context.Buffer[0]),
                            Text(".")
                        ));
                    }
                    
                    if (context.Buffer.Count > 0)
                        continue;
                    
                    var endResult = parser.Complete(context.State);

                    return endResult switch
                    {
                        ValueParserResult<TValue>.Success value => Result.Success(value.Value),
                        ValueParserResult<TValue>.Failure failure => Result.Failure(failure.Error),
                        _ => throw new ArgumentOutOfRangeException()
                    };
            }
        }
    }
    
    public TValue? Run()
    {
        // TODO: support help and version options
        // https://github.com/dahlia/optique/blob/main/packages/core/src/facade.ts#L607
        switch (Parse())
        {
            case Result<TValue>.Success success:
                return success.Value;
            
            case Result<TValue>.Failure failure:
                throw new NotImplementedException();        
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
