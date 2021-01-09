using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FiltersTest.Services
{
    public interface INotificationService
    {
        Task Send(Exception ex);
    }

    public class NotificationService : INotificationService
    {
        private readonly ILogger<NotificationService> logger;

        public NotificationService(ILogger<NotificationService> logger) => this.logger = logger;
        public Task Send(Exception ex)
        {
            logger.LogInformation("Sending notification.");
            logger.LogInformation("Notification sent.");
            return Task.CompletedTask;
        }
    }
}
