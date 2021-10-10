using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Notifications.Common;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.ViewModels;
using System;
using System.Threading.Tasks;

namespace Notifications.Controllers
{
	/// <summary>
	/// Swap out userId for email, not good to expose out internal Ids on public interface.
	/// AppointmentEvent endpoint does not follow Restful practices, should be replaced 
	///		with specific verbs; see AppointmentCancelledEvent endpoint.
	/// Appointment Notification Requests should be renormalised, i.e. { AppointmentId, DateTime, Reason }.
	/// </summary>
	[Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly INotificationsService _notificationsService;
		private readonly IMapper _mapper;

        public NotificationsController(INotificationsService notificationsService, IMapper mapper)
        {
            this._notificationsService = notificationsService;
			this._mapper = mapper;
        }

		[Route("")]
		[HttpPost]
		public async Task<IActionResult> NotificationEvent(Guid userId, string type, [FromBody] object data)
		{
			if (data == null)
			{
				return BadRequest("Notification Event data cannot be null");
			}

			bool result;
			if (Enum.TryParse(type, out NotificationType notificationType))
			{
				// Keep object models separate
				var request = JsonConvert.DeserializeObject<NotificationCancelledRequest>(data.ToString());

				var notification = new NotificationModel // Candidate for mapping
				{
					AppointmentDateTime = request.AppointmentDateTime,
					FirstName = request.FirstName,
					OrganisationName = request.OrganisationName,
					Reason = request.Reason,
					UserId = userId,
					Type = notificationType
				};

				result = await _notificationsService.AddNotificationEvent(notification);
			} else
			{
				return BadRequest("Notification Type data cannot be null");
			}

			return Ok(result);
		}

		[Route("")]
		[HttpDelete]
		public async Task<IActionResult> NotificationCancelledEvent(Guid userId, [FromBody] NotificationCancelledRequest request)
		{
			if (request == null)
			{
				return BadRequest("Appointment Cancelled Request cannot be null");
			}

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var notification = new NotificationModel
			{
				AppointmentDateTime = request.AppointmentDateTime,
				FirstName = request.FirstName,
				OrganisationName = request.OrganisationName,
				UserId = userId,
				Type = NotificationType.AppointmentCancelled
			};

			var result = await _notificationsService.AddNotificationEvent(notification);
			return Ok(result);
		}

		[Route("")]
        [HttpGet]
        public async Task<IActionResult> GetNotifications(Guid userId, string type = null)
        {
			Enum.TryParse(type, out NotificationType notificationType);
			var notifications = await _notificationsService.GetNotifications(userId, notificationType);

			return Ok(notifications);
        }
    }
}