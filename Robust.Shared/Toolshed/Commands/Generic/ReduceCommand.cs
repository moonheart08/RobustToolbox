using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Robust.Shared.Players;
using Robust.Shared.Toolshed.Errors;
using Robust.Shared.Toolshed.Syntax;
using Robust.Shared.Toolshed.Tasks;

namespace Robust.Shared.Toolshed.Commands.Generic;

[ToolshedCommand]
public sealed class ReduceCommand : ToolshedCommand
{
    [CommandImplementation, TakesPipedTypeAsGeneric]
    public async ValueTask<T> Reduce<T>(
        [CommandInvocationContext] IInvocationContext ctx,
        [PipedArgument] IEnumerable<T> input,
        [CommandArgument] Block<T, T> reducer
    )
    {
        using IEnumerator<T> enumerator = input.GetEnumerator();

        if (!enumerator.MoveNext())
            throw new InvalidOperationException("No elements in input");

        var reduced = enumerator.Current;

        while (enumerator.MoveNext())
        {
            reduced = await reducer.Invoke(reduced, new ReduceContext<T>(ctx, enumerator.Current));
            await ToolshedTaskUtils.YieldIterator();
        }

        return reduced!;
    }
}

internal record ReduceContext<T>(IInvocationContext Inner, T Value) : IInvocationContext
{
    public bool CheckInvokable(CommandSpec command, out IConError? error)
    {
        return Inner.CheckInvokable(command, out error);
    }

    public ICommonSession? Session => Inner.Session;
    public ToolshedManager Toolshed => Inner.Toolshed;
    public ToolshedEnvironment Environment => Inner.Environment;

    public void WriteLine(string line)
    {
        Inner.WriteLine(line);
    }

    public void ReportError(IConError err)
    {
        Inner.ReportError(err);
    }

    public IEnumerable<IConError> GetErrors()
    {
        return Inner.GetErrors();
    }

    public void ClearErrors()
    {
        Inner.ClearErrors();
    }

    public Dictionary<string, object?> Variables { get; } = new();

    public object? ReadVar(string name)
    {
        if (name == "value")
            return Value;

        return Inner.ReadVar(name);
    }
}
