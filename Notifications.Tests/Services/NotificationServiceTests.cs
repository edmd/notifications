using AutoMapper;
using FluentAssertions;
using Moq;
using Notifications.Common;
using Notifications.Common.Models;
using Notifications.DataAccess.Access;
using Notifications.DataAccess.Entities;
using Notifications.Mapping;
using Notifications.Services;
using Notifications.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Notifications.Tests.Services
{
	public class NotificationServiceTests
	{
        private readonly NotificationsService _sut;
        private readonly Mock<INotificationsAccess> _mockRepository = new Mock<INotificationsAccess>();
        private readonly IMapper _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<NotificationMapper>(); }).CreateMapper();

        public NotificationServiceTests()
        {
            _sut = new NotificationsService(_mockRepository.Object, _mapper);
        }

        [Fact]
        public async Task Should_return_success_if_notification_added()
        {
            //Arrange
            var model = _mapper.Map<NotificationModel>(NotificationsDataFixture.Notification());
            model.Id = Guid.Empty;
            _mockRepository.Setup(x => x.GetNotificationType(It.IsAny<NotificationType>())).ReturnsAsync(NotificationsDataFixture.GetNotificationType()).Verifiable();
            _mockRepository.Setup(x => x.AddNotification(It.IsAny<NotificationEntity>())).ReturnsAsync(NotificationsDataFixture.NotificationId).Verifiable();

            //Act
            var result = await _sut.AddNotificationEvent(model);

            //Assert
            _mockRepository.Verify();
            result.Should().BeTrue();
        }

        [Fact]
        public async Task Should_return_failure_if_notification_not_added()
        {
            //Arrange
            var model = new NotificationModel { Type = NotificationType.AppointmentCancelled, TypeName = NotificationType.AppointmentCancelled.ToString() };
            _mockRepository.Setup(x => x.GetNotificationType(It.IsAny<NotificationType>())).ReturnsAsync(new NotificationTypeEntity()).Verifiable();
            _mockRepository.Setup(x => x.AddNotification(It.IsAny<NotificationEntity>())).ReturnsAsync(Guid.Empty).Verifiable();

            //Act
            var result = await _sut.AddNotificationEvent(model);

            //Assert
            _mockRepository.Verify();
            result.Should().BeFalse();
        }

        [Fact]
        public async Task Should_return_notifications_for_valid_parameters()
        {
            //Arrange
            var model = new NotificationModel { Type = NotificationType.AppointmentCancelled, TypeName = NotificationType.AppointmentCancelled.ToString() };
            var list = _mapper.Map<List<NotificationModel>>(NotificationsDataFixture.NotificationList());
            _mockRepository.Setup(x => x.GetNotifications(NotificationsDataFixture.UserId, NotificationType.AppointmentCancelled))
                .ReturnsAsync(list).Verifiable();

            //Act
            var result = await _sut.GetNotifications(NotificationsDataFixture.UserId, NotificationType.AppointmentCancelled);

            //Assert
            _mockRepository.Verify();
            result.Count.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Should_return_no_notifications_for_invalid_parameters()
        {
            //Arrange
            _mockRepository.Setup(x => x.GetNotifications(It.IsAny<Guid>(), It.IsAny<NotificationType>())).ReturnsAsync(new List<NotificationModel>()).Verifiable();

            //Act
            var result = await _sut.GetNotifications(NotificationsDataFixture.NotificationId, NotificationType.AppointmentCreated);

            //Assert
            _mockRepository.Verify();
            result.Count.Should().Be(0);
        }
    }
}