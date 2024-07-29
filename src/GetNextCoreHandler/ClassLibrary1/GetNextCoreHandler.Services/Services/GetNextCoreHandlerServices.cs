using Remanufacturing.Extensions;
using Remanufacturing.Messages;
using Remanufacturing.Responses;
using Remanufacturing.Services;
using System.Net;

namespace Remanufacturing.OrderNextCore.Services;

public class GetNextCoreHandlerServices(GetNextCoreServicesOptions options)
{

	private readonly GetNextCoreServicesOptions _servicesOptions = options;

	public async Task<IResponse> OrderNextCoreAsync(OrderNextCoreMessage orderNextCoreMessage, string instance)
	{
		try
		{
			ArgumentException.ThrowIfNullOrEmpty(orderNextCoreMessage.PodId, nameof(orderNextCoreMessage.PodId));
			ArgumentException.ThrowIfNullOrEmpty(orderNextCoreMessage.CoreId, nameof(orderNextCoreMessage.CoreId));
			if (orderNextCoreMessage.RequestDateTime == default)
				orderNextCoreMessage.RequestDateTime = DateTime.UtcNow;
			orderNextCoreMessage.MessageType = MessageTypes.OrderNextCore;
			await ServiceBusServices.SendMessageAsync(_servicesOptions.ServiceBusClient, _servicesOptions.OrderNextCoreTopicName, orderNextCoreMessage);
			return new StandardResponse()
			{
				Type = "https://httpstatuses.com/201", // HACK: In a real-world scenario, you would want to provide a more-specific URI reference that identifies the response type.
				Title = "Request for next core sent.",
				Status = HttpStatusCode.Created,
				Detail = "The request for the next core has been sent to the warehouse.",
				Instance = instance,
				Extensions = new Dictionary<string, object>()
				{
					{ "PodId", orderNextCoreMessage.PodId },
					{ "CoreId", orderNextCoreMessage.CoreId }
				}
			};
		}
		catch (ArgumentException ex)
		{
			return new ProblemDetails(ex, instance);
		}
		catch (Exception ex)
		{
			return new ProblemDetails()
			{
				Type = "https://httpstatuses.com/500", // HACK: In a real-world scenario, you would want to provide a more-specific URI reference that identifies the response type.
				Title = "An error occurred while sending the message to the Service Bus",
				Status = HttpStatusCode.InternalServerError,
				Detail = ex.Message, // HACK: In a real-world scenario, you would not want to expose the exception message to the client.
				Instance = instance
			};
		}
	}

	public async Task<IResponse> GetNextCoreAsync(HttpClient httpClient, OrderNextCoreMessage orderNextCoreMessage)
	{
		try
		{

			// Get the URI for the pod
			if (!_servicesOptions.GetNextCoreUris.TryGetValue(orderNextCoreMessage.PodId, out Uri? getNextCoreUrl))
				throw new ArgumentOutOfRangeException(nameof(orderNextCoreMessage.PodId), $"The pod ID '{orderNextCoreMessage.PodId}' is not valid.");
			getNextCoreUrl = new Uri(getNextCoreUrl.ToString().Replace("{podId}", orderNextCoreMessage.PodId));
			getNextCoreUrl = new Uri(getNextCoreUrl.ToString().Replace("{date}", orderNextCoreMessage.RequestDateTime.ToString("yyyy-MM-dd")));

			// Add the subscription key to the request headers
			httpClient.DefaultRequestHeaders.Add(_servicesOptions.ProductionScheduleAPIKeyKey, _servicesOptions.ProductionScheduleAPIKeyValue);

			// Call the GetNextCore API operation
			HttpResponseMessage httpResponse = await httpClient.GetAsync(getNextCoreUrl);

			// Parse the response
			httpResponse.EnsureSuccessStatusCode();
			string responseBody = await httpResponse.Content.ReadAsStringAsync();
			IResponse? response = responseBody.ToResponse();
			return response ?? throw new InvalidOperationException("The response from the GetNextCore service was not in the expected format.");

		}
		catch (ArgumentException ex)
		{
			return new ProblemDetails(ex);
		}
		catch (Exception ex)
		{
			return new ProblemDetails()
			{
				Type = "https://httpstatuses.com/500", // HACK: In a real-world scenario, you would want to provide a more-specific URI reference that identifies the response type.
				Title = "An error occurred while sending the message to the Service Bus",
				Status = HttpStatusCode.InternalServerError,
				Detail = ex.Message // HACK: In a real-world scenario, you would not want to expose the exception message to the client.
			};
		}
	}

}