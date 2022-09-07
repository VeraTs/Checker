using CheckerServer.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace CheckerServer.Models
{
    internal class KitchenManager2 : IHostedService, IDisposable
    {
        private IServiceProvider Services { get; }
        private IHubContext<KitchenHub> kitchenHubContext;
        private readonly int r_RestId;
        private static bool shouldRun = true;
        private Timer? _timer = null;

        public KitchenManager2(IServiceProvider serviceProvider, IHubContext<KitchenHub> kitchenHubContext, int id)
        {
            Services = serviceProvider;
            this.kitchenHubContext = kitchenHubContext;
            this.r_RestId = id;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));
            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            if (shouldRun)
            {
                shouldRun = false;
                await ManageKitchenAsync();
                shouldRun = true;
            }

        }

        private Task ManageKitchenAsync()
        {
            throw new NotImplementedException();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}