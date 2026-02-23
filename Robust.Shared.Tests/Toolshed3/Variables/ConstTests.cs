using NUnit.Framework;
using Robust.Shared.Toolshed3;
using Robust.Shared.Toolshed3.Variables;

namespace Robust.Shared.Tests.Toolshed3.Variables;

public sealed class ConstTests
{
    [Test]
    public void VariableReadWrite()
    {
        var variable = new ConstVariable<int>(3);
        var place = "$variable";

        Assert.That(
            variable.TryRead(place, out var v1),
            Is.EqualTo(ReadResult.Success)
            );

        Assert.That(
            variable.TryWrite(place, 5),
            Is.EqualTo(WriteResult.NotMutable)
            );
    }
}
