using FluentAssertions;
using Newtonsoft.Json;
using Notifications.Common.Models;
using Notifications.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Notifications.Tests.Integration
{
	public class NotificationControllerIntegrationTests : IClassFixture<TestWebApplicationFactory>
	{
		private readonly HttpClient _client;
		private const string ApiControllerPath = "/api/notifications";

		public NotificationControllerIntegrationTests(TestWebApplicationFactory factory)
		{
			_client = factory.CreateClient();
		}

		[Theory]
		[InlineData("?userId=ade8d623-877d-41e3-9160-dc274c48c88b&type=AppointmentCancelled")]
		public async Task AppointmentEvent_should_return_success_valid_parameters(string queryString)
		{
			//Arrange
			string requestBody = JsonConvert.SerializeObject(NotificationsDataFixture.NotificationCancelledRequest());

			//Act
			HttpResponseMessage response = await _client.PostAsync(new Uri($"{ApiControllerPath}{queryString}", UriKind.Relative),
				new StringContent(requestBody, Encoding.UTF8, "application/json"));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			bool value = bool.Parse(await response.Content.ReadAsStringAsync());
			value.Should().BeTrue();
		}

		[Theory]
		[InlineData("?userId=ade8d623-877d-41e3-9160-dc274c48c89b&type=AppointmentCreated")]
		public async Task AppointmentEvent_should_return_failure_invalid_parameters(string queryString)
		{
			//Arrange
			string requestBody = JsonConvert.SerializeObject(NotificationsDataFixture.NotificationCancelledRequest());

			//Act
			HttpResponseMessage response = await _client.PostAsync(new Uri($"{ApiControllerPath}{queryString}", UriKind.Relative),
				new StringContent(requestBody, Encoding.UTF8, "application/json"));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			bool value = bool.Parse(await response.Content.ReadAsStringAsync());
			value.Should().BeFalse();
		}

		[Theory]
		[InlineData("?userId=ade8d623-877d-41e3-9160-dc274c48c88b&type=AppointmentCancelled")]
		[InlineData("?userId=ade8d623-877d-41e3-9160-dc274c48c88b&type=AppointmentCreated")]
		public async Task AppointmentEvent_should_return_badrequest_if_model_null(string queryString)
		{
			//Arrange
			string requestBody = JsonConvert.SerializeObject("");

			//Act
			HttpResponseMessage response = await _client.PostAsync(new Uri($"{ApiControllerPath}{queryString}", UriKind.Relative),
				new StringContent(requestBody, Encoding.UTF8, "application/json"));

			//Assert
			response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
			var responseBody = await response.Content.ReadAsStringAsync();
			
			responseBody.Should().Be("Notification Event data cannot be null");
		}

		[Theory]
		[InlineData("?userId=ade8d623-877d-41e3-9160-dc274c48c88b&type=AppointmentCancelled")]
		public async Task GetNotifications_should_pass_request_to_service(string queryString)
		{
			// Act
			HttpResponseMessage response = await _client.GetAsync(new Uri($"{ApiControllerPath}{queryString}", UriKind.Relative));

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			var value = JsonConvert.DeserializeObject<List<NotificationModel>>(await response.Content.ReadAsStringAsync());
			value.Should().BeAssignableTo<List<NotificationModel>>();
			value.Count.Should().BeGreaterThan(0);
		}

		[Theory]
		[InlineData("?userId=ade8d623-877d-41e3-9160-dc274c48c88b&type=AppointmentUpdated")]
		[InlineData("?userId=ade8d623-877d-41e3-9160-dc274c48c88b&type=AppointmentCreated")]
		public async Task GetNotifications_should_return_empty_list(string queryString)
		{
			// Act
			HttpResponseMessage response = await _client.GetAsync(new Uri($"{ApiControllerPath}{queryString}", UriKind.Relative));

			// Assert
			response.StatusCode.Should().Be(HttpStatusCode.OK);
			var value = JsonConvert.DeserializeObject<List<NotificationModel>>(await response.Content.ReadAsStringAsync());
			value.Should().BeAssignableTo<List<NotificationModel>>();
			value.Count.Should().Be(0);
		}
	}
}