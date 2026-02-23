using System;
using Robust.Shared.Toolshed3.Attributes;

namespace Robust.Shared.Toolshed3.Variables;

internal sealed class GeneratorVariable<T>(Func<T> generator, RunRing ring) : Variable
{
    public override bool Mutable => false;

    public override Ts3Type ConcreteType => (Ts3Type)typeof(T);

    public override RunRing SafeRing { get; } = ring;

    private protected override ReadResult TryReadInner(string? place, out object? value)
    {
        value = generator();

        return ReadResult.Success;
    }

    private protected override WriteResult TryWriteInner(string? place, object? value)
    {
        return WriteResult.NotMutable;
    }
}
