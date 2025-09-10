namespace Neutrino.Parsers;

class ConstantParser<TState>(TState initialState) : IParser<TState, TState>
{
    public Usage Usage => Usage.Empty;

    public TState InitialState => initialState;

    public ParserResult<TState> Parse(ParserContext<TState> context) => 
        ParserResult.Success(context, []);

    public ValueParserResult<TState> Complete(TState state) => 
        ValueParserResult.Success(state);
}