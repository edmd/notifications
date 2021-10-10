using Notifications.Common.Models;
using Notifications.DataAccess.Entities;

namespace Notifications.Mapping
{
	public class NotificationMapper : AutoMapper.Profile
	{
		public NotificationMapper()
		{
			CreateMap<NotificationEntity, NotificationModel>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
				.ForMember(dest => dest.AppointmentDateTime, opt => opt.MapFrom(x => x.AppointmentDateTime))
				.ForMember(dest => dest.MessageContent, opt => opt.MapFrom(x => x.MessageContent))
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(x => x.UserId))
				.ForMember(dest => dest.FirstName, opt => opt.MapFrom(x => x.FirstName))
				.ForMember(dest => dest.TypeName, opt => opt.MapFrom(x => x.NotificationType.TypeName)) // This one isn't loading - running out of time
				.ForMember(dest => dest.OrganisationName, opt => opt.MapFrom(x => x.OrganisationName))
				.ReverseMap();

			CreateMap<NotificationTypeModel, NotificationTypeEntity>()
				.ForMember(dest => dest.Id, opt => opt.MapFrom(x => x.Id))
				.ForMember(dest => dest.Template, opt => opt.MapFrom(x => x.Template))
				.ForMember(dest => dest.Type, opt => opt.MapFrom(x => x.Type))
				.ForMember(dest => dest.TypeName, opt => opt.MapFrom(x => x.TypeName))
				.ReverseMap();
		}
	}
}