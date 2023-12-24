namespace RoomWroom.CommandHandling;

internal abstract class Command(string commandText, Response immediateResponse)
{
    private readonly string _commandText = commandText;

    internal Response ImmediateResponse { get; } = immediateResponse;
    
    internal bool Matches(string text) => text.StartsWith(_commandText);

    internal abstract IEnumerable<Type> GetRequiredTypes();
}

internal sealed class Command<T>(
    string commandText, Func<T, Task<Response>> function, Response immediateResponse)
    : Command(commandText, immediateResponse)
{
    private readonly Func<T, Task<Response>> _function = function;

    internal Task<Response> InvokeTask(T t) => _function.Invoke(t);
    internal override IEnumerable<Type> GetRequiredTypes() => [typeof(T)];
}

internal sealed class Command<T1, T2>(
    string commandText, Func<T1, T2, Task<Response>> function, Response immediateResponse)
    : Command(commandText, immediateResponse)
{
    private readonly Func<T1, T2, Task<Response>> _function = function;

    internal Task<Response> InvokeTask(T1 t1, T2 t2) => _function.Invoke(t1, t2);
    internal override IEnumerable<Type> GetRequiredTypes() => [typeof(T1), typeof(T2)];
}

internal sealed class Command<T1, T2, T3>(
    string commandText, Func<T1, T2, T3, Task<Response>> function, Response  immediateResponse)
    : Command(commandText,  immediateResponse)
{
    private readonly Func<T1, T2, T3, Task<Response>> _function = function;

    internal Task<Response> InvokeTask(T1 t1, T2 t2, T3 t3) => _function.Invoke(t1, t2, t3);
    internal override IEnumerable<Type> GetRequiredTypes() => [typeof(T1), typeof(T2), typeof(T3)];
}