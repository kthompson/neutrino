using System.Reflection;

namespace Neutrino;

public class RunOptions<T>
{
    public string? ProgramName { get; init; }
    public IReadOnlyList<string> Arguments { get; init; } = Environment.GetCommandLineArgs();
    public HelpOptions<T> Help { get; init; } = new HelpOptions<T>();
    public VersionOptions<T> Version { get; init; } = new VersionOptions<T>();
    
    public Action<string> StandardError { get; init; } = Console.Error.WriteLine;
    public Action<string> StandardOutput { get; init; } = Console.Out.WriteLine;
    
    public Func<int, T?> OnError { get; init; } = _ => default;
}

[Flags]
public enum Mode
{
    Command = 1 << 0,
    Option = 1 << 1,
    Both = Command | Option,
}

public class VersionOptions<T>
{
    public Mode Mode { get; init; } = Mode.Option;
    public Func<int, T?> OnShow { get; init; } = _ => default;

    public string Version { get; init; } = Assembly.GetExecutingAssembly()
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion ?? "0.0.0";
}

public class HelpOptions<T>
{
    public Mode Mode { get; init; } = Mode.Option;
    public Func<int, T?> OnShow { get; init; } = _ => default;
}