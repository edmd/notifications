using AutoMapper;
using FluentAssertions;
using Notifications.Common;
using Notifications.DataAccess.Access;
using Notifications.Mapping;
using Notifications.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;

namespace Notifications.Tests.Access
{
    [Collection("Notifications")]
    public class GetNotificationTypeTests
    {
        private readonly NotificationsAccess _sut;
        private readonly NotificationsDataFixture _notificationsDataFixture;
        private readonly IMapper _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<NotificationMapper>(); }).CreateMapper();

        public GetNotificationTypeTests(NotificationsDataFixture notificationsDataFixture)
        {
            _notificationsDataFixture = notificationsDataFixture;

            _sut = new NotificationsAccess(_notificationsDataFixture.DbContext, _mapper);
        }

        [Fact]
        public async Task Should_get_notification_type_record()
        {
            //Act
            var result = await _sut.GetNotificationType(NotificationType.AppointmentCancelled);

            //Assert
            result.Template.Should().Be(NotificationsDataFixture.Template);
            result.Type.Should().Be(NotificationType.AppointmentCancelled.ToString());
            result.TypeName.Should().Be(NotificationsDataFixture.NotificationTypeName);
        }

        [Fact]
        public async Task Should_get_empty_notification_type_parameters()
        {
            //Act
            var results = await _sut.GetNotificationType(NotificationType.AppointmentCreated);

            //Assert
            results.Should().BeNull();
        }
    }
}