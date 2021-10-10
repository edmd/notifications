using System;

namespace Notifications.DataAccess.Entities
{
    public class NotificationEntity
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string MessageContent { get; set; }
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string OrganisationName { get; set; }
        public string Reason { get; set; }
        public virtual NotificationTypeEntity NotificationType { get; set; }
        //public virtual UserEntity User { get; set; } // Assuming this is a standalone service then it's okay for denormalised data in this entity
    }
}