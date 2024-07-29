using Remanufacturing.Helpers;
using Remanufacturing.Responses;
using System.Text.Json;

namespace Remanufacturing.Extensions;

public static class ResponseExtensions
{

	public static IResponse? ToResponse(this string serializedResponse)
	{

		JsonSerializerOptions options = new();
		options.Converters.Add(new InterfaceConverter<IResponse, Response>());
		IResponse? deserializedResponse = JsonSerializer.Deserialize<IResponse>(serializedResponse, options);

		if (deserializedResponse == null)
			return null;
		else if (deserializedResponse.ResponseType == ResponseType.StandardResponse)
			return JsonSerializer.Deserialize<StandardResponse>(serializedResponse, options);
		else if (deserializedResponse.ResponseType == ResponseType.ProblemDetails)
			return JsonSerializer.Deserialize<ProblemDetails>(serializedResponse, options);
		else
			return null;

	}

}