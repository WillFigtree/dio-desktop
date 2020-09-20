using System.Threading;
using System.Threading.Tasks;

namespace DioCli
{
    interface IDevice
    {
        void Open();
        void Close();
        Task RunAsync(CancellationToken ct);
    }
}
