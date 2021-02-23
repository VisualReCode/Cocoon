using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MvcCocoon.Services
{
    public class ActivityDebug : BackgroundService
    {
        private readonly ILogger<ActivityDebug> _logger;
        private readonly ActivityListener _listener;

        public ActivityDebug(ILogger<ActivityDebug> logger)
        {
            _logger = logger;
            _listener = new ActivityListener
            {
                ShouldListenTo = source => source.Name == "ReCode.Cocoon.Proxy",
                Sample = (ref ActivityCreationOptions<ActivityContext> _) => ActivitySamplingResult.AllData,
                ActivityStarted = ActivityStarted,
                ActivityStopped = ActivityStopped
            };
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ActivitySource.AddActivityListener(_listener);
            
            var completion = new TaskCompletionSource();
            stoppingToken.Register(() => completion.SetResult());
            return completion.Task;
        }

        private void ActivityStopped(Activity activity)
        {
            _logger.LogInformation($"Activity {activity.OperationName}: {activity.Duration}");
        }

        private void ActivityStarted(Activity obj)
        {
        }

        public override void Dispose()
        {
            _listener.Dispose();
            base.Dispose();
        }
    }
}