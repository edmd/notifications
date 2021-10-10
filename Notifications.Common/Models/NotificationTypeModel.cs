using System;

namespace Notifications.Common.Models
{
    public class NotificationTypeModel
    {
        public Guid Id { get; set; }
        public string Template { get; set; }
        public string Type { get; set; }
        public string TypeName { get; set; }
    }
}