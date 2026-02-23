using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Robust.Shared.Utility;

namespace Robust.Shared.Toolshed3.Variables;

public sealed class VariableCollection
{
    /// <summary>
    ///     Actual written variables within our environment, including their
    ///     defined type.
    /// </summary>
    private readonly Dictionary<string, Variable> _variables;

    public VariableCollection(VariableCollection? parent = null)
    {
        if (parent is not null)
        {
            _variables = parent._variables.ShallowClone();
            // TODO: Maybe expose the outer context through a variable, too?
        }
        else
        {
            _variables = new();
        }
    }

    public bool TryGetVariable(string name, [NotNullWhen(true)] out Variable? var)
    {
        return _variables.TryGetValue(name, out var);
    }

    public WriteResult TryWriteVariable(
        string name,
        Ts3Type type,
        object? value,
        bool createIfNonexistent = false)
    {
        if (TryGetVariable(name, out var v))
        {
            return v.TryWrite(name, value);
        }
        else if (createIfNonexistent)
        {
            var newV = new SimpleVariable(value, type);
            _variables[name] = newV;
            return WriteResult.Success; // No need to write, we created it that way.
        }
        else
        {
            return WriteResult.NonExistent;
        }
    }

    public bool TryReadVariable(
        string name,
        out object? value,
        [NotNullWhen(true)] out Ts3Type? type,
        out ReadResult resultCode)
    {
        if (TryGetVariable(name, out var v))
        {
            resultCode = v.TryRead(name, out value);
            type = v.ConcreteType;

            if (resultCode == ReadResult.Success)
                return true;

            return false;
        }

        resultCode = ReadResult.Failure;
        type = null;
        value = null;
        return false;
    }

    public bool TryReadVariable<T>(
        string name,
        out T? value,
        [NotNullWhen(true)] out Ts3Type? type,
        out ReadResult resultCode)
    {
        if (!TryReadVariable(name, out var ov, out type, out resultCode))
        {
            value = default;
            return false;
        }

        if (ov is T v2)
        {
            value = v2;
        }
        else if (ov is null && type is { Nullable: true } && type.UnderlyingType.IsAssignableTo(typeof(T)))
        {
            value = (T?)ov;
        }
        else
        {
            value = default;
        }

        return true;
    }

    public void InsertVariable(string name, Variable v)
    {
        _variables[name] = v;
    }
}

/// <summary>
///     Provides read-only programmatic variables for use in commands like <c>emplace</c>.
/// </summary>
public interface IVariableProvider
{
    static abstract IReadOnlyDictionary<string, Ts3Type> Variables { get; }

    public bool TryGetValue(string var, out object? value, [NotNullWhen(true)] out Ts3Type? type);
}
