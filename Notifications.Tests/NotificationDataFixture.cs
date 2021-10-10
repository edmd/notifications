using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Notifications.Common;
using Notifications.DataAccess;
using Notifications.DataAccess.Entities;
using Notifications.ViewModels;
using System;
using System.Collections.Generic;
using Xunit;

namespace Notifications.Tests.Fixtures
{
	public sealed class NotificationsDataFixture : IDisposable
	{
		private readonly NotificationsDbContext _dbContext;
		public static Guid NotFoundId = Guid.NewGuid();
		public static Guid NotificationId = Guid.NewGuid();
		public static Guid NotificationTypeId = Guid.NewGuid();
		public static Guid UserId = Guid.NewGuid();

		public static DateTime AppointmentDateTime = DateTime.Now;
		public static string FirstName = "John";
		public static string MessageBody = "Message Body";
		public static string NotificationTypeName = "Appointment Cancelled";
		public static string OrganisationName = "Brockelbank Group Practice";
		public static string Reason = "Patient Deceased";
		public static string Template = "Id: {0}\nEventType: {1}\nBody: Hi {2}, your appointment with {3} at {4} has \nbeen - cancelled for the following reason: {5}.\nTitle: {6}";
		private static NotificationEntity notification = new NotificationEntity();
		private static NotificationTypeEntity notificationType = new NotificationTypeEntity();

		public NotificationsDbContext DbContext => _dbContext;

		public NotificationsDataFixture()
		{
			DbContextOptions<NotificationsDbContext> dbOptions = new DbContextOptionsBuilder<NotificationsDbContext>()
					.UseInMemoryDatabase("name=NotificationTests")
					.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
					.EnableSensitiveDataLogging()
					.Options;
			_dbContext = new NotificationsDbContext(dbOptions);
			_dbContext.Database.EnsureCreated();

			_dbContext.Notifications.Add(new NotificationEntity
			{
				AppointmentDateTime = AppointmentDateTime,
				Id = Guid.NewGuid(),
				MessageContent = MessageContent(),
				NotificationType = GetNotificationType()
			});
			_dbContext.Add(Notification());

			_dbContext.SaveChanges();
		}

		public void Dispose()
		{
			_dbContext.Database.EnsureDeleted();
			_dbContext.Dispose();
		}

		public static NotificationCancelledRequest NotificationCancelledRequest()
		{
			return new NotificationCancelledRequest()
			{
				AppointmentDateTime = AppointmentDateTime,
				FirstName = FirstName,
				OrganisationName = OrganisationName,
				Reason = Reason
			};
		}

		public static string MessageContent()
		{
			return String.Format(GetNotificationType().Template,
				NotificationId,
				GetNotificationType().Type,
				FirstName,
				OrganisationName,
				AppointmentDateTime.ToShortDateString() + " " + AppointmentDateTime.ToShortTimeString(),
				Reason,
				GetNotificationType().TypeName);
		}

		public static NotificationTypeEntity GetNotificationType()
		{
			notificationType.Id = NotificationTypeId;
			notificationType.Template = Template;
			notificationType.Type = NotificationType.AppointmentCancelled.ToString();
			notificationType.TypeName = NotificationTypeName;

			return notificationType;
		}

		public static NotificationEntity Notification()
		{
			notification.AppointmentDateTime = AppointmentDateTime;
			notification.Id = NotificationId;
			notification.NotificationType = GetNotificationType();
			notification.UserId = UserId;
			notification.FirstName = FirstName;
			notification.OrganisationName = OrganisationName;
			notification.MessageContent = MessageContent();

			return notification;
		}

		public static List<NotificationEntity> NotificationList()
		{
			return new List<NotificationEntity>()
			{
				new NotificationEntity {
					AppointmentDateTime = AppointmentDateTime,
					Id = Guid.NewGuid(),
					NotificationType = GetNotificationType(),
					UserId = UserId,
					FirstName = FirstName,
					OrganisationName = OrganisationName
				},
				new NotificationEntity {
					AppointmentDateTime = AppointmentDateTime,
					Id = Guid.NewGuid(),
					NotificationType = GetNotificationType(),
					UserId = UserId,
					FirstName = FirstName,
					OrganisationName = OrganisationName
				}
			};
		}
	}

	[CollectionDefinition("Notifications")]
	public class NotificationsSharedFixture : IClassFixture<NotificationsDataFixture>
	{

	}
}