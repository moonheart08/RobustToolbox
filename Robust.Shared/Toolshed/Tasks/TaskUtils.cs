using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Robust.Shared.Toolshed.Tasks;

public static class ToolshedTaskUtils
{
    // THREAD SAFETY: Even if multiple tasks increment this at once, and even if one of them snaps it back to 0, it will never exceed Bound + ThreadCount in value and threads will yield as expected.
    // There is additionally no consequence to tasks yielding early or late due to toolshed's low perf requirements
    private static int _iterationYieldCounter = 0;

    public const int IterationsToYield = 1024;

    public static async ValueTask YieldIterator()
    {
        if (Interlocked.Increment(ref _iterationYieldCounter) > IterationsToYield)
        {
            _iterationYieldCounter = 0; // don't care about atomicity now!
            await Task.Yield();
        }

        return;
    }
}
