using System;

namespace Notifications.DataAccess.Entities
{
    public class NotificationTypeEntity
    {
        public Guid Id { get; set; }

        public string Type { get; set; }

        public string TypeName { get; set; }

        public string Template { get; set; }
    }
}