using NUnit.Framework;
using Robust.Shared.Toolshed3;
using Robust.Shared.Toolshed3.Variables;

namespace Robust.Shared.Tests.Toolshed3.Variables;

public sealed class CollectionTests
{
    private VariableCollection MakeTestCollection()
    {
        var collection = new VariableCollection();

        Assert.That(
            collection.TryWriteVariable("foo", (Ts3Type)typeof(int), 42, createIfNonexistent: true),
            Is.EqualTo(WriteResult.Success)
            );

        Assert.That(
            collection.TryWriteVariable("bar", (Ts3Type)typeof(IEnumerable<int>), new List<int> {4, 5, 6}, createIfNonexistent: true),
            Is.EqualTo(WriteResult.Success)
        );

        collection.InsertVariable("baz", new ConstVariable<int>(36));

        return collection;
    }

    [Test]
    public void ReadCollection()
    {
        var vars = MakeTestCollection();

        object? value;

        using (Assert.EnterMultipleScope())
        {
            Assert.That(vars.TryReadVariable("foo", out value, out _, out var code));
            Assert.That(code, Is.EqualTo(ReadResult.Success));
        }

        Assert.That(value, Is.EqualTo(42));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(vars.TryReadVariable("bar", out value, out _, out var code));
            Assert.That(code, Is.EqualTo(ReadResult.Success));
        }

        Assert.That(value, Is.EquivalentTo([4, 5, 6]));

        using (Assert.EnterMultipleScope())
        {
            Assert.That(vars.TryReadVariable("baz", out value, out _, out var code));
            Assert.That(code, Is.EqualTo(ReadResult.Success));
        }

        Assert.That(value, Is.EqualTo(36));
    }
}
