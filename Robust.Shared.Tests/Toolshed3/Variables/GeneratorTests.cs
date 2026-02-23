using NUnit.Framework;
using Robust.Shared.Toolshed3;
using Robust.Shared.Toolshed3.Attributes;
using Robust.Shared.Toolshed3.Variables;

namespace Robust.Shared.Tests.Toolshed3.Variables;

public sealed class GeneratorTests
{
    [Test]
    public void GeneratorReadTests()
    {
        var count = 0;
        var variable = new GeneratorVariable<int>(() => count++, RunRing.Async);
        var place = "$variable";

        Assert.That(
            variable.TryRead(place, out var v1),
            Is.EqualTo(ReadResult.Success)
        );

        Assert.That(
            variable.TryRead(place, out var v2),
            Is.EqualTo(ReadResult.Success)
        );

        Assert.That(v1, Is.EqualTo(0));
        Assert.That(v2, Is.EqualTo(1));

        Assert.That(
            variable.TryWrite(place, 5),
            Is.EqualTo(WriteResult.NotMutable)
        );
    }
}
