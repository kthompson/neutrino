namespace Neutrino;

/// <summary>
/// Represents a single term in a command-line usage description.
/// </summary>
public abstract record UsageTerm
{
    public record Argument(string Name) : UsageTerm;
    /// <summary>
    /// An option term, which represents a command-line option that can
    /// be specified by the user.
    /// </summary>
    /// <param name="Name">An optional metavariable name for the option, which is used
    /// to indicate what value the option expects.</param>
    /// <param name="OptionNames">The names of the option, which can include multiple
    /// short and long forms</param>
    public record Option(string? Name, IReadOnlyList<OptionName> OptionNames) : UsageTerm;
    /// <summary>
    /// A command term, which represents a subcommand in the command-line
    /// usage.
    /// </summary>
    /// <param name="Name">The name of the command, which is used to identify it
    /// in the command-line usage</param>
    public record Command(string Name): UsageTerm;
    
    /// <summary>
    /// An optional term, which represents an optional component
    /// in the command-line usage.
    /// </summary>
    /// <param name="Terms">The terms that are optional, which can be an argument, an option,
    /// a command, or another usage term.</param>
    public record Optional(Usage Terms): UsageTerm;
    
    /// <summary>
    /// A term of multiple occurrences, which allows a term to be specified
    /// multiple times in the command-line usage.
    /// </summary>
    /// <param name="Terms">
    /// The terms that can occur multiple times, which can be an argument,
    /// an option, a command, or another usage term.
    /// </param>
    /// <param name="Min">The minimum number of times the term must occur.</param>
    public record Multiple(Usage Terms, int Min): UsageTerm;    
    
    /// <summary>
    /// An exclusive term, which represents a group of terms that are mutually
    /// exclusive, meaning that only one of the terms in the group can be
    /// specified at a time.
    /// </summary>
    /// <param name="Terms">
    /// The terms that are mutually exclusive, which can include
    /// arguments, options, commands, or other usage terms
    /// </param>
    public record Exclusive(IReadOnlyList<Usage> Terms): UsageTerm;
}

/// <summary>
/// Represents a command-line usage description, which is a sequence of
/// <see cref="UsageTerm"/> objects. This type is used to describe how a
/// command-line parser expects its input to be structured, including
/// the required and optional components, as well as any exclusive
/// groups of terms.
/// </summary>
/// <param name="UsageTerms"></param>
public record Usage(IReadOnlyList<UsageTerm> UsageTerms)
{
    public static readonly Usage Empty = new Usage([]);
}

/// <summary>
/// Options for formatting usage descriptions.
/// </summary>
public record UsageFormatOptions(
    bool ExpandCommands = false,
    bool OnlyShortestOptions = false,
    bool Colors = false,
    int? MaxWidth = null
);
