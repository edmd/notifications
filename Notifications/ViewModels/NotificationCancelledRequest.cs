using System;
using System.ComponentModel.DataAnnotations;

namespace Notifications.ViewModels
{
	public class NotificationCancelledRequest
	{
		[Required]
		public string FirstName { get; set; }

		[Required]
		public DateTime AppointmentDateTime { get; set; }

		[Required]
		public string OrganisationName { get; set; }

		public string Reason { get; set; }
	}
}