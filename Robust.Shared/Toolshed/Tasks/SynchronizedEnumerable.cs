using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Robust.Shared.Toolshed.Tasks;

internal sealed class SynchronizedEnumerable<T> : IEnumerable<T>
{
    private List<T> Inner;

    // ToBlockingEnumerable is just not good enough/
    public static async Task<SynchronizedEnumerable<T>> Create(IAsyncEnumerable<T> input)
    {

    }

    internal SynchronizedEnumerable(List<T> inner)
    {
        Inner = inner;
    }

    public IEnumerator<T> GetEnumerator()
    {
        return Inner.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
