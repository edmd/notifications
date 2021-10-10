using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Notifications.Common;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Controllers;
using Notifications.Mapping;
using Notifications.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Notifications.Tests
{
	public class NotificationControllerTests
    {
        private readonly NotificationsController _sut;
        private readonly Mock<INotificationsService> _mockNotificationService = new Mock<INotificationsService>();
        private readonly IMapper _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<NotificationMapper>(); }).CreateMapper();

        public NotificationControllerTests()
        {
            _sut = new NotificationsController(_mockNotificationService.Object, _mapper);
        }

		[Fact]
		public async Task AppointmentEvent_should_pass_request_to_service()
		{
			//Arrange
			_mockNotificationService
				.Setup(x => x.AddNotificationEvent(It.IsAny<NotificationModel>()))
				.ReturnsAsync(true)
				.Verifiable();

			//Act
			IActionResult result = await _sut.NotificationEvent(
				NotificationsDataFixture.UserId,
				NotificationType.AppointmentCancelled.ToString(),
				JsonConvert.SerializeObject(NotificationsDataFixture.NotificationCancelledRequest()));

			//Assert
			_mockNotificationService.Verify();
			result.Should().BeAssignableTo<OkObjectResult>();
			var okResult = result as OkObjectResult;
			okResult.Value.Should().BeAssignableTo<bool>();
			bool value = bool.Parse(okResult.Value.ToString());
			value.Should().BeTrue();
		}

		[Fact]
		public async Task AppointmentEvent_should_return_badrequest_if_model_null()
		{
			//Act
			IActionResult result = await _sut.NotificationEvent(
				NotificationsDataFixture.UserId,
				NotificationType.AppointmentCancelled.ToString(), 
				null);

			//Assert
			result.Should().BeAssignableTo<BadRequestObjectResult>();
		}

		[Fact]
		public async Task GetNotifications_should_pass_request_to_service()
		{
			//Arrange
			var list = _mapper.Map<List<NotificationModel>>(NotificationsDataFixture.NotificationList());

			_mockNotificationService
				.Setup(x => x.GetNotifications(
					NotificationsDataFixture.UserId, 
					NotificationType.AppointmentCancelled))
				.ReturnsAsync(list)
				.Verifiable();

			//Act
			IActionResult result = await _sut.GetNotifications(
				NotificationsDataFixture.UserId, NotificationType.AppointmentCancelled.ToString());

			//Assert
			_mockNotificationService.Verify();
			result.Should().BeAssignableTo<OkObjectResult>();
			var okResult = result as OkObjectResult;
			okResult.Value.Should().BeAssignableTo<List<NotificationModel>>();
			var value = (List<NotificationModel>)okResult.Value;
			value.Count.Should().BeGreaterThan(0);
		}

		[Fact]
		public async Task GetNotifications_should_return_empty_list()
		{
			//Arrange
			_mockNotificationService
				.Setup(x => x.GetNotifications(
					It.IsAny<Guid>(), It.IsAny<NotificationType>()))
				.ReturnsAsync(new List<NotificationModel>())
				.Verifiable();

			//Act
			IActionResult result = await _sut.GetNotifications(
				NotificationsDataFixture.UserId, NotificationType.AppointmentCancelled.ToString());

			//Assert
			_mockNotificationService.Verify();
			result.Should().BeAssignableTo<OkObjectResult>();
			var okResult = result as OkObjectResult;
			okResult.Value.Should().BeAssignableTo<List<NotificationModel>>();
			var value = (List<NotificationModel>)okResult.Value;
			value.Count.Should().Be(0);
		}
	}
}