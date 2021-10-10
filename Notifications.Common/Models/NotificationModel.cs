using System;

namespace Notifications.Common.Models
{
    public class NotificationModel
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public NotificationType Type { get; set; }
        public string TypeName { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string OrganisationName { get; set; }
        public string Reason { get; set; }
        public string MessageContent { get; set; }

    }
}