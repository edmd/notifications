using System;

namespace Notifications.DataAccess.Entities
{
	/// <summary>
	/// Illustration of functionality only, refer to NotificationTypeEntity
	/// </summary>
	public class TemplateEntity
	{
		public Guid Id { get; set; }

		public virtual NotificationTypeEntity NotificationType { get; set; }

		public string CommunicationType { get; set; } // Email, SMS

		public string Template { get; set; }
	}
}