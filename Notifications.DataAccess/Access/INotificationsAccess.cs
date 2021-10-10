using Notifications.Common;
using Notifications.Common.Models;
using Notifications.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Notifications.DataAccess.Access
{
	public interface INotificationsAccess
    {
        Task<Guid> AddNotification(NotificationEntity model);
        Task<IEnumerable<NotificationModel>> GetNotifications(Guid userId, NotificationType? type = null);
        Task<NotificationTypeEntity> GetNotificationType(NotificationType type);
    }
}