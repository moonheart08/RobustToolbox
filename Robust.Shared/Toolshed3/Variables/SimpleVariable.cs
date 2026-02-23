using Robust.Shared.Utility;

namespace Robust.Shared.Toolshed3.Variables;

/// <summary>
///     A simple in-memory variable with no fancy behavior.
/// </summary>
internal sealed class SimpleVariable : Variable
{
    private object? _value;

    public override Ts3Type ConcreteType { get; }

    private protected override ReadResult TryReadInner(string? place, out object? value)
    {
        value = _value;

        return ReadResult.Success;
    }

    private protected override WriteResult TryWriteInner(string? place, object? value)
    {
        _value = value;

        return WriteResult.Success;
    }

    /// <summary>
    ///     Construct a simple variable from its value and its type.
    /// </summary>
    /// <remarks>
    ///     No constructor exists that infers the type from the value, as this isn't
    ///     possible within C#/TS's type system due to nullability not being encoded within object.
    /// </remarks>
    public SimpleVariable(object? value, Ts3Type type)
    {
        ConcreteType = type;
        DebugTools.Assert(FitsConcreteType(value));
        _value = value;
    }
}
