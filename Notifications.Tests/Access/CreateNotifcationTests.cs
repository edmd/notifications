using AutoMapper;
using FluentAssertions;
using Notifications.DataAccess.Access;
using Notifications.DataAccess.Entities;
using Notifications.Mapping;
using Notifications.Tests.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Notifications.Tests.Access
{
    [Collection("Notifications")]
    public class CreateNotificationTests
    {
        private readonly NotificationsAccess _sut;
        private readonly NotificationsDataFixture _notificationsDataFixture;
        private readonly IMapper _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<NotificationMapper>(); }).CreateMapper();

        public CreateNotificationTests(NotificationsDataFixture notificationsDataFixture)
        {
            _notificationsDataFixture = notificationsDataFixture;

            _sut = new NotificationsAccess(_notificationsDataFixture.DbContext, _mapper);
        }

        [Fact]
        public async Task Should_insert_new_record_if_success()
        {
            //Arrange
            var notification = new NotificationEntity
            {
                AppointmentDateTime = NotificationsDataFixture.AppointmentDateTime,
                FirstName = NotificationsDataFixture.FirstName,
                MessageContent = NotificationsDataFixture.MessageBody,
                OrganisationName = NotificationsDataFixture.OrganisationName,
                Reason = NotificationsDataFixture.Reason,
                UserId = NotificationsDataFixture.UserId
            };

            //Act
            Guid result = await _sut.AddNotification(notification);

            //Assert
            result.ToString().Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_update_existing_record_success()
        {
            //Arrange
            var messageContent = "New message content";
            var notification = NotificationsDataFixture.Notification();
            notification.MessageContent = messageContent;

            //Act
            var result = await _sut.AddNotification(notification);

            //Assert
            result.Should().Be(NotificationsDataFixture.NotificationId);
        }
    }
}
