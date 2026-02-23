namespace Robust.Shared.Toolshed3.CommandPrims;

/// <summary>
///     A type for toolshed command arguments, representing an optional argument specified by argument name,
///     for example <c>Optional&lt;int&gt; count</c> is syntactually <c>--count int</c>
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public struct Optional<T>
{
    public T? Value { get; }
    public bool HasValue { get; }

    internal Optional(T? value)
    {
        Value = value;
        HasValue = true;
    }

    public Optional()
    {
        Value = default;
        HasValue = false;
    }
}
