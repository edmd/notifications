using AutoMapper;
using FluentAssertions;
using Notifications.Common;
using Notifications.Common.Models;
using Notifications.DataAccess.Entities;
using Notifications.Mapping;
using System;
using Xunit;

namespace Notifications.Tests.Mapping
{
	public class NotificationMappingTests
	{
		private readonly IMapper _mapper;
		private readonly Guid notificationId = Guid.NewGuid();
		private readonly Guid notificationTypeId = Guid.NewGuid();
		private readonly Guid userId = Guid.NewGuid();

		private readonly DateTime appointmentDateTime = DateTime.Now;
		private readonly string cancelledNotificationType = NotificationType.AppointmentCancelled.ToString();
		private readonly string firstName = "John";
		private readonly string notificationTypeName = "Appointment Cancelled";
		private readonly string organisationName = "Brockelbank Group Practice";
		private readonly string messageContent = "Message Content";
		private readonly string template = "Template";
		private readonly string reason = "Patient Deceased";

		public NotificationMappingTests()
		{
			_mapper
				= new MapperConfiguration(cfg => { cfg.AddProfile<NotificationMapper>(); })
				.CreateMapper();
		}

		[Fact]
		public void Should_map_notification_properties()
		{
			//Arrange
			var source = new NotificationModel
			{
				Id = notificationId,
				AppointmentDateTime = appointmentDateTime,
				FirstName = firstName,
				OrganisationName = organisationName,
				MessageContent = messageContent,
				TypeName = notificationTypeName,
				Reason = reason,
				UserId = userId
			};

			//Act
			var result = _mapper.Map<NotificationEntity>(source);

			//Assert
			result.Id.Should().Be(source.Id);
			result.AppointmentDateTime.Should().Be(source.AppointmentDateTime);
			result.MessageContent.Should().Be(source.MessageContent);
			result.FirstName.Should().Be(source.FirstName);
			result.OrganisationName.Should().Be(source.OrganisationName);
			result.Reason.Should().Be(source.Reason);
			result.UserId.Should().Be(source.UserId);
		}

		[Fact]
		public void Should_map_notification_type_properties()
		{
			//Arrange
			var source = new NotificationTypeModel
			{
				Id = notificationTypeId, 
				Template = template, 
				Type = cancelledNotificationType,
				TypeName = notificationTypeName
			};

			//Act
			var result = _mapper.Map<NotificationTypeEntity>(source);

			//Assert
			result.Id.Should().Be(source.Id);
			result.Template.Should().Be(source.Template);
			result.Type.Should().Be(source.Type);
			result.TypeName.Should().Be(source.TypeName);
		}
	}
}
