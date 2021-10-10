using AutoMapper;
using Notifications.Common;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.DataAccess.Access;
using Notifications.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationsAccess _notificationsAccess;
        private readonly IMapper _mapper;

        public NotificationsService(INotificationsAccess notificationsAccess, IMapper mapper)
        {
            this._notificationsAccess = notificationsAccess;
            this._mapper = mapper;
        }

		public async Task<bool> AddNotificationEvent(NotificationModel notificationEvent)
		{
            // Fetch template, Persist notification, Update notification MessageBody
            var success = false;
            try
			{
                var notificationType = await this._notificationsAccess.GetNotificationType(
                    notificationEvent.Type);

                if (notificationType != null)
                {
                    var notificationEntity = _mapper.Map<NotificationEntity>(notificationEvent);
                    notificationEntity.NotificationType = notificationType;

                    var Id = await this._notificationsAccess.AddNotification(notificationEntity);
                    var messageContent = String.Format(notificationType.Template,
                        Id,
                        notificationType.Type,
                        notificationEvent.FirstName,
                        notificationEvent.OrganisationName,
                        notificationEvent.AppointmentDateTime.ToShortDateString() + " " + notificationEvent.AppointmentDateTime.ToShortTimeString(),
                        notificationEvent.Reason,
                        notificationType.TypeName);
                    notificationEntity.MessageContent = messageContent;

                    await this._notificationsAccess.AddNotification(notificationEntity);
                    success = true;
                }
            }
            catch (Exception ex)
			{
                //_logger.LogError(ex);
                return success;
			}

            return success;
        }

		public async Task<IReadOnlyCollection<NotificationModel>> GetNotifications(Guid userId, NotificationType? type = null)
        {
            var notifications = await this._notificationsAccess.GetNotifications(userId, type);

            return notifications.ToList();
        }
	}
}