namespace RoomWroom.CommandHandling;

public abstract class Command(string commandText, string immediateResponse)
{
    private readonly string _commandText = commandText;

    internal string ImmediateResponse { get; } = immediateResponse;
    
    internal bool Matches(string text) => text.StartsWith(_commandText);

    internal abstract IEnumerable<Type> GetRequiredTypes();
}

public sealed class Command<T>(
    string commandText, Func<T, Task<string>> function, string immediateResponse)
    : Command(commandText, immediateResponse)
{
    private readonly Func<T, Task<string>> _function = function;

    internal Task<string> InvokeTask(T t) => _function.Invoke(t);
    internal override IEnumerable<Type> GetRequiredTypes() => [typeof(T)];
}

public sealed class Command<T1, T2>(
    string commandText, Func<T1, T2, Task<string>> function, string immediateResponse)
    : Command(commandText, immediateResponse)
{
    private readonly Func<T1, T2, Task<string>> _function = function;

    internal Task<string> InvokeTask(T1 t1, T2 t2) => _function.Invoke(t1, t2);
    internal override IEnumerable<Type> GetRequiredTypes() => [typeof(T1), typeof(T2)];
}

public sealed class Command<T1, T2, T3>(
    string commandText, Func<T1, T2, T3, Task<string>> function, string  immediateResponse)
    : Command(commandText,  immediateResponse)
{
    private readonly Func<T1, T2, T3, Task<string>> _function = function;

    internal Task<string> InvokeTask(T1 t1, T2 t2, T3 t3) => _function.Invoke(t1, t2, t3);
    internal override IEnumerable<Type> GetRequiredTypes() => [typeof(T1), typeof(T2), typeof(T3)];
}