using System.Linq;

namespace GGroupp.Infra;

partial class DefaultSocketsHttpHandlerProvider
{
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        foreach (var handler in namedHandlers.Select(pair => pair.Value))
        {
            handler.Dispose();
        }
        disposed = true;
    }
}
