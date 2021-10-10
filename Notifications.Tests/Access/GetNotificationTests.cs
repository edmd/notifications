using AutoMapper;
using FluentAssertions;
using Notifications.Common;
using Notifications.DataAccess.Access;
using Notifications.Mapping;
using Notifications.Tests.Fixtures;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Notifications.Tests.Access
{
    [Collection("Notifications")]
    public class GetNotificationTests
    {
        private readonly NotificationsAccess _sut;
        private readonly NotificationsDataFixture _notificationsDataFixture;
        private readonly IMapper _mapper = new MapperConfiguration(cfg => { cfg.AddProfile<NotificationMapper>(); }).CreateMapper();

        public GetNotificationTests(NotificationsDataFixture notificationsDataFixture)
        {
            _notificationsDataFixture = notificationsDataFixture;

            _sut = new NotificationsAccess(_notificationsDataFixture.DbContext, _mapper);
        }

        [Fact]
        public async Task Should_get_records_valid_user_parameter()
        {
            //Act
            var results = await _sut.GetNotifications(NotificationsDataFixture.UserId);

            //Assert
            results.Count().Should().BeGreaterThan(0);
            results.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_get_records_valid_parameters()
        {
            //Act
            var results = await _sut.GetNotifications(NotificationsDataFixture.UserId, NotificationType.AppointmentCancelled);

            //Assert
            results.Count().Should().BeGreaterThan(0);
            results.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Should_get_no_records_invalid_parameters()
        {
            //Act
            var results = await _sut.GetNotifications(NotificationsDataFixture.NotificationId, NotificationType.AppointmentCreated);

            //Assert
            results.Count().Should().Be(0);
            results.Should().BeNullOrEmpty();
        }
    }
}