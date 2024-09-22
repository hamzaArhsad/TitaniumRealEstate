using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System.Threading.Tasks;
using Application;
using Domain;
using System.Linq;
using CinemaxFinal.SignalRHubs;

namespace Titanium_MVC.Services
{
    public class PropertyCheckService : BackgroundService
    {
        private readonly ILogger<PropertyCheckService> _logger;
        private readonly IPropertyService<Property> _propertyService; // Service for database operations
        private readonly IHubContext<NotificationHub> _hubContext;
        private DateTime _lastCheckTime; // Last time properties were checked
        private DateTime _lastNotificationTime; // Last time a notification was sent
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Check every 1 minute
        private readonly TimeSpan _notificationInterval = TimeSpan.FromMinutes(1); // Notify every 1 minute

        public PropertyCheckService(
            ILogger<PropertyCheckService> logger,
            IPropertyService<Property> propertyService,
            IHubContext<NotificationHub> hubContext)
        {
            _logger = logger;
            _propertyService = propertyService;
            _hubContext = hubContext;
            _lastCheckTime = DateTime.UtcNow; // Initialize with the current time
            _lastNotificationTime = DateTime.UtcNow; // Initialize notification time
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Property Check Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTime.UtcNow;

                    // Check for the most recent property added in the last minute
                    var recentProperty = _propertyService.GetRecentProperties(now.AddMinutes(-1)).OrderByDescending(p => p.UploadDate).FirstOrDefault();

                    // Only send notifications if it's been 1 minute since the last notification
                    if (recentProperty != null && now - _lastNotificationTime >= _notificationInterval)
                    {
                        var message = $"Latest property added: Location: {recentProperty.Location}, Type: {recentProperty.Type}, Area: {recentProperty.Area}";
                        await _hubContext.Clients.All.SendAsync("ReceiveNotification", message);

                        _lastNotificationTime = now; // Update the last notification time
                    }

                    _lastCheckTime = now; // Update the last check time

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while checking for new properties.");
                }

                // Wait for the configured interval before checking again
                await Task.Delay(_checkInterval, stoppingToken);
            }

            _logger.LogInformation("Property Check Service is stopping.");
        }
    }
}
