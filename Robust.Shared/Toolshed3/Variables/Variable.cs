using Robust.Shared.Toolshed3.Attributes;

namespace Robust.Shared.Toolshed3.Variables;

/// <summary>
///     The actual memory representation of a variable as a <b>place</b>.
/// </summary>
public abstract class Variable
{
    public abstract Ts3Type ConcreteType { get; }

    public virtual bool Mutable => true;

    public virtual RunRing SafeRing => RunRing.Async;

    public ReadResult TryRead(string? place, out object? value)
    {
        var success = TryReadInner(place, out value);

        if (!FitsConcreteType(value))
            return ReadResult.MysteryTypeChange;

        return success;
    }

    private protected abstract ReadResult TryReadInner(string? place, out object? value);

    public WriteResult TryWrite(string? place, object? value)
    {
        if (!FitsConcreteType(value))
            return WriteResult.NewValueOfWrongType; // Nuh uh.

        return TryWriteInner(place, value);
    }

    private protected abstract WriteResult TryWriteInner(string? place, object? value);

    /// <summary>
    ///     A helper for checking if an assignment is valid.
    ///     The underlying value of a variable should never fail this check.
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    protected bool FitsConcreteType(object? value)
    {
        if (value == null)
            return ConcreteType.Nullable;

        var ty = value.GetType();

        if (ty == ConcreteType.UnderlyingType)
            return true;

        if (ty.IsAssignableTo(ConcreteType.UnderlyingType))
            return true;

        return false;
    }
}

public enum ReadResult
{
    /// <summary>
    ///     The underlying type of the variable changed.
    ///     Debug only.
    /// </summary>
    MysteryTypeChange = -3,
    /// <summary>
    ///     The underlying place this variable existed in ceased to be.
    ///     For example, an entity got deleted.
    /// </summary>
    NonExistent = -2,
    /// <summary>
    ///     Arbitrary failure.
    /// </summary>
    Failure = -1,
    Success = 1,
}

public enum WriteResult
{
    NonExistent = -4,
    NotMutable = -3,
    NewValueOfWrongType = -2,
    Failure = -1,
    Success = 1,
}
