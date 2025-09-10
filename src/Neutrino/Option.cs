namespace Neutrino;

abstract record Option<T>
{
    public record Some(T Value) : Option<T>;

    public record None : Option<T>;
    
    public static implicit operator Option<T>(Option.NoneType _) => new None();
}

class Option
{
    public readonly struct NoneType;

    public static readonly NoneType None = new NoneType();
    
    public static Option<T> Some<T>(T value) => new Option<T>.Some(value); 
}