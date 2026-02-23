using System;
using Robust.Shared.Toolshed;
using Robust.Shared.Utility;

namespace Robust.Shared.Toolshed3;

public sealed class Ts3Type
{
    public Type UnderlyingType { get; }
    public bool Nullable { get; }

    public Ts3Type(Type underlyingType, bool nullable)
    {
        UnderlyingType = underlyingType;
        Nullable = nullable;

        var canBeNull = underlyingType.CanBeNull();

        if (!(nullable == false || canBeNull) || (underlyingType.IsValueType && (nullable != canBeNull)))
        {
            throw new Ts3TypeNullabilityMismatchException(underlyingType, nullable);
        }
    }

    /// <summary>
    ///     Creates a new non-null TS3 type.
    /// </summary>
    /// <param name="underlyingType">The underlying type to use.</param>
    public Ts3Type(Type underlyingType)
    {
        UnderlyingType = underlyingType;
        Nullable = false;

        DebugTools.Assert(!underlyingType.CanBeNull() || !underlyingType.IsValueType);
    }

    public static implicit operator Type(Ts3Type ty) => ty.UnderlyingType;
    public static explicit operator Ts3Type(Type ty) => new(ty);
}

public sealed class Ts3TypeNullabilityMismatchException(Type ty, bool nullable) : Exception
{
    public override string Message { get; } = $"Type {ty} doesn't match nullability {nullable}";
}
