using Notifications.Common.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.Common.Interfaces
{
	public interface INotificationsService
    {
        Task<IReadOnlyCollection<NotificationModel>> GetNotifications(Guid userId, NotificationType? type = null);
        Task<bool> AddNotificationEvent(NotificationModel notificationEvent);
    }
}