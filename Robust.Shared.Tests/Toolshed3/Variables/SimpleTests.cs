using NUnit.Framework;
using Robust.Shared.Toolshed3;
using Robust.Shared.Toolshed3.Variables;

namespace Robust.Shared.Tests.Toolshed3.Variables;

public sealed class SimpleTests
{
    [Test]
    public void VariableReadWrite()
    {
        var variable = new SimpleVariable((int)3, (Ts3Type)typeof(int));
        var place = "$variable";

        Assert.That(
            variable.TryRead(place, out var v1),
            Is.EqualTo(ReadResult.Success)
            );

        Assert.That(v1, Is.EqualTo(3));

        Assert.That(
            variable.TryWrite(place, 5),
            Is.EqualTo(WriteResult.Success)
        );

        Assert.That(
            variable.TryRead(place, out var v2),
            Is.EqualTo(ReadResult.Success)
        );

        Assert.That(v2, Is.EqualTo(5));
    }

    [Test]
    public void VariableBadWrite()
    {
        var variable = new SimpleVariable((int)3, (Ts3Type)typeof(int));
        var place = "$variable";

        Assert.That(
            variable.TryWrite(place, "among us"),
            Is.EqualTo(WriteResult.NewValueOfWrongType)
            );

        Assert.That(variable.TryRead(place, out var value),
            Is.EqualTo(ReadResult.Success)
            );

        Assert.That(value, Is.EqualTo(3));
    }

    [Test]
    public void VariableReadWriteNull()
    {
        var variable = new SimpleVariable((int?)3, new Ts3Type(typeof(int?),true));
        var place = "$variable";

        Assert.That(
            variable.TryRead(place, out var v1),
            Is.EqualTo(ReadResult.Success)
        );

        Assert.That(v1, Is.EqualTo(3));

        Assert.That(
            variable.TryWrite(place, null),
            Is.EqualTo(WriteResult.Success)
        );

        Assert.That(
            variable.TryRead(place, out var v2),
            Is.EqualTo(ReadResult.Success)
        );

        Assert.That(v2, Is.EqualTo(null));
    }

    [Test]
    public void CannotConstructBadNullVariable()
    {
        Assert.Catch<Exception>(() =>
        {
            _ = new SimpleVariable(null, new Ts3Type(typeof(int), false));
        });
    }
}
