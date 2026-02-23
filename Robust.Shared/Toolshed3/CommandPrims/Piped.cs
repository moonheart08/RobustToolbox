namespace Robust.Shared.Toolshed3.CommandPrims;

/// <summary>
///     A type for toolshed command arguments, representing an optionally piped value.
///     Commands implicitly support both piped and unpiped form.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public struct Piped<T>
{
    public T Value { get; }

    internal Piped(T value)
    {
        Value = value;
    }
}
