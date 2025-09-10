namespace Neutrino;

/// <summary>
/// Represents the name of a command-line option.  There are four types of
/// option syntax:
///
/// - GNU-style long options (`--option`)
/// - POSIX-style short options (`-o`) or Java-style options (`-option`)
/// - MS-DOS-style options (`/o`, `/option`)
/// - Plus-prefixed options (`+o`)
/// </summary>
/// <param name="Name"></param>
public record OptionName
{
    public string Name { get; }

    private OptionName(string name)
    {
        Name = name;
    }

    static Option<OptionName> FromString(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return Option.None;
        }
        
        return name[0] is '-' or '/' or '+' ? Option.Some(new OptionName(name)) : Option.None;
    }

    
    
    public static implicit operator OptionName(string option) =>
        FromString(option) switch
        {
            Option<OptionName>.Some(var x) => x,
            
            // throwing here is a little gross, but it makes using the API a little nicer to 
            // be able to implicitly cast to a valid option name and users will know right away
            // if they made a mistake instead of having to check for None everywhere
            _ => throw new InvalidOperationException(
                $"Option name {option} is invalid. Option names must begin with `-`, `--`, or `/`"
            )
        };
}