using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Notifications.Common;
using Notifications.Common.Models;
using Notifications.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Notifications.DataAccess.Access
{
	public class NotificationsAccess : INotificationsAccess
    {
        private readonly NotificationsDbContext _dbContext;
        private readonly IMapper _mapper;

        public NotificationsAccess(NotificationsDbContext dbContext, IMapper mapper)
        {
            this._dbContext = dbContext;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<NotificationModel>> GetNotifications(Guid userId, NotificationType? type = null)
        {
            var notifications = _dbContext.Notifications.Include(c => c.NotificationType).Where(x => x.UserId == userId);
            if (type.HasValue)
            {
                notifications = notifications.Where(x => x.NotificationType.Type == type.Value.ToString());
            }

            return _mapper.Map<List<NotificationModel>>(notifications);
        }

        public async Task<Guid> AddNotification(NotificationEntity entity)
        {
            // Debating return of Guid.Empty on Exception
            _dbContext.Notifications.Update(entity);
            _dbContext.SaveChanges();

            return entity.Id;
        }

        public async Task<NotificationTypeEntity> GetNotificationType(NotificationType type)
        {
            return _dbContext.NotificationTypes.FirstOrDefault(x => x.Type == type.ToString());
        }
    }
}