using System;

namespace Robust.Shared.Toolshed3.Attributes;

/// <summary>
///     Dictates what environment this command provider can run in.
/// </summary>
/// <seealso cref="RunRing"/>
public sealed class CommandRunContextAttribute(RunRing kind) : Attribute
{
    public RunRing Kind = kind;
}

/// <summary>
///     What contexts given commands can run in.
/// </summary>
public enum RunRing
{
    /// <summary>
    ///     Commands that need to be fully in the game context to work.
    /// </summary>
    Game = 0,
    /// <summary>
    ///     Commands that can be processed outside the "core game", but still need to be main-thread.
    ///     Client commands that work prior to connection are an example of this.
    /// </summary>
    OffGame = 1,
    /// <summary>
    ///     Commands that can process fully asynchronously from the main thread.
    /// </summary>
    Async = 2,
}
