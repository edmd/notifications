using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.DataAccess.Access;
using Notifications.Services;
using Notifications.Tests.Fixtures;
using System;
using System.Collections.Generic;

namespace Notifications.Tests.Integration
{
	public class TestWebApplicationFactory : WebApplicationFactory<Program>
	{
		protected override IHostBuilder CreateHostBuilder()
		{
			var builder = Host.CreateDefaultBuilder()
				.ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
					webBuilder.ConfigureServices(services =>
					{
						services.AddTransient<INotificationsAccess, NotificationsAccess>();
						services.AddTransient<INotificationsService, NotificationsService>();
						//SetupNotificationsService(services);
					});
				});

			return builder;
		}

		//private void SetupNotificationsService(IServiceCollection services)
		//{
		//	var mockNotificationService = new Mock<INotificationsService>();

		//	mockNotificationService.Setup(x =>
		//			x.AddNotificationEvent(It.Is<NotificationModel>(s => s.Id == NotificationsDataFixture.NotificationId)))
		//		.ReturnsAsync(true);

		//	mockNotificationService.Setup(x =>
		//			x.AddNotificationEvent(It.Is<NotificationModel>(s => s.Id != NotificationsDataFixture.NotificationId)))
		//		.ReturnsAsync(false);

		//	mockNotificationService.Setup(x =>
		//			x.GetNotifications(It.IsAny<Guid>(), Common.NotificationType.AppointmentCancelled))
		//		.ReturnsAsync(NotificationsDataFixture.NotificationListModels());

		//	mockNotificationService.Setup(x =>
		//			x.GetNotifications(It.IsAny<Guid>(), Common.NotificationType.AppointmentCreated))
		//		.ReturnsAsync(new List<NotificationModel>() { });

		//	mockNotificationService.Setup(x =>
		//			x.GetNotifications(It.IsAny<Guid>(), Common.NotificationType.AppointmentUpdated))
		//		.ReturnsAsync(new List<NotificationModel>() { });

		//	services.AddTransient<INotificationsService>(prop => mockNotificationService.Object);
		//}
	}
}