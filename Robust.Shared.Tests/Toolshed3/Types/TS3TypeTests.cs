using NUnit.Framework;
using Robust.Shared.Toolshed3;

namespace Robust.Shared.Tests.Toolshed3.Types;

public sealed class Ts3TypeTests
{
    public static readonly Type[] NonNullTypes = [typeof(int), typeof(bool), typeof(long)];
    public static readonly Type[] NullTypes = [typeof(int?), typeof(bool?)];
    public static readonly Type[] ObjectTypes = [typeof(string), typeof(object)];

    [Test]
    [TestCaseSource(nameof(NonNullTypes))]
    public void ConstructNonNullTypes(Type t)
    {
        _ = new Ts3Type(t, false);
    }

    [Test]
    [TestCaseSource(nameof(NullTypes))]
    public void ConstructNullTypes(Type t)
    {
        _ = new Ts3Type(t, true);
    }

    [Test]
    [TestCaseSource(nameof(ObjectTypes))]
    public void ConstructObjectTypesIndifferently(Type t)
    {
        _ = new Ts3Type(t, false);
        _ = new Ts3Type(t, true);
        _ = new Ts3Type(t);
    }

    [Test]
    [TestCaseSource(nameof(NullTypes))]
    public void FailToConstructNullTypes(Type t)
    {
        Assert.Catch<Ts3TypeNullabilityMismatchException>(() =>
        {
            _ = new Ts3Type(t, false);
        });
    }

    [Test]
    [TestCaseSource(nameof(NonNullTypes))]
    public void FailToConstructNonNullTypes(Type t)
    {
        Assert.Catch<Ts3TypeNullabilityMismatchException>(() =>
        {
            _ = new Ts3Type(t, true);
        });
    }
}
