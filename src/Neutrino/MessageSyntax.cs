using Neutrino.Messages;

namespace Neutrino;

/// <summary>
/// Helper methods for creating and formatting messages.
/// </summary>
public static class MessageSyntax
{
    public static MessageTerm Text(string text) => new MessageTerm.Text(text);
    public static MessageTerm OptionName(string name) => new MessageTerm.OptionName(name);
    public static MessageTerm OptionNames(IReadOnlyList<string> names) => new MessageTerm.OptionNames(names);
    public static MessageTerm Metavar(string name) => new MessageTerm.Metavar(name);
    public static MessageTerm Value(string value) => new MessageTerm.Value(value);
    public static MessageTerm Values(IReadOnlyList<string> values) => new MessageTerm.Values(values);
    public static MessageTerm EnvVar(string name) => new MessageTerm.EnvVar(name);

    public static Message MessageFromTerms(params MessageTerm[] terms) => new Message(terms);
    public static Message MessageFromList(IReadOnlyList<MessageTerm> terms) => new Message(terms);

    // Formatting logic can be added here as needed, similar to formatMessage in TypeScript
}