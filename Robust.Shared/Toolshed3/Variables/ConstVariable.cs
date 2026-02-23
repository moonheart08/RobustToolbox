namespace Robust.Shared.Toolshed3.Variables;

/// <summary>
///     A variable with a known type and constant value.
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class ConstVariable<T>(T value) : Variable
    where T : notnull
{
    public override bool Mutable => false;

    public override Ts3Type ConcreteType => (Ts3Type)typeof(T);

    private protected override ReadResult TryReadInner(string? place, out object? value1)
    {
        value1 = value;

        return ReadResult.Success;
    }

    private protected override WriteResult TryWriteInner(string? place, object? value1)
    {
        return WriteResult.NotMutable;
    }
}
