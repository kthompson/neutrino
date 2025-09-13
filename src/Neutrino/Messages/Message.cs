namespace Neutrino.Messages;

/// <summary>
/// Represents a single term in a message, which can be a text, an option name, a list of option names, a metavariable, a value, a list of consecutive values, or an environment variable.
/// </summary>
public abstract record MessageTerm
{
    /// <summary>
    /// A plain text term in the message.
    /// </summary>
    public record Text(string Content) : MessageTerm
    {
        public override string ToString() => Content;
    }

    /// <summary>
    /// An option name term in the message, which can be a single option name or a subcommand.
    /// </summary>
    public record OptionName(string Name) : MessageTerm
    {
        public override string ToString() => $"'{Name}'";
    }

    /// <summary>
    /// A list of option names term in the message.
    /// </summary>
    public record OptionNames(IReadOnlyList<string> Names) : MessageTerm
    {
        public override string ToString() => string.Join(", ", Names.Select(n => $"'{n}'"));
    }

    /// <summary>
    /// A metavariable term in the message.
    /// </summary>
    public record Metavar(string Name) : MessageTerm
    {
        public override string ToString() => Name;
    }

    /// <summary>
    /// A value term in the message, which can be a single value.
    /// </summary>
    public record Value(string ValueAsString) : MessageTerm
    {
        public override string ToString() => ValueAsString;
    }

    /// <summary>
    /// A list of values term in the message, which can be a list of consecutive values.
    /// </summary>
    public record Values(IReadOnlyList<string> ValuesAsStrings) : MessageTerm
    {
        public override string ToString() => string.Join(" ", ValuesAsStrings);
    }

    /// <summary>
    /// An environment variable term in the message, which represents an environment variable name.
    /// </summary>
    public record EnvVar(string Name) : MessageTerm
    {
        public override string ToString() => Name;
    }
}

/// <summary>
/// Type representing a message that can include styled/colored values.
/// This type is used to create structured messages that can be displayed to the user with specific formatting.
/// </summary>
public record Message(IReadOnlyList<MessageTerm> Terms)
{
    public override string ToString()
    {
        return string.Join("", Terms);
    }
}

/// <summary>
/// Options for formatting messages.
/// </summary>
public record MessageFormatOptions(
    bool Colors = false,
    bool Quotes = true,
    int? MaxWidth = null,
    string? ResetSuffix = null
);