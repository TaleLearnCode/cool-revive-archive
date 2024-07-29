using Azure.Messaging.ServiceBus;
using Remanufacturing.Helpers;
using Remanufacturing.InventoryManager.Entities;
using Remanufacturing.Messages;
using System.Text.Json;

namespace Remanufacturing.InventoryManager.Extensions;

public static class InventoryEventEntityExtensions
{

	public static InventoryEventEntity? ToInventoryEventEntity(this ServiceBusReceivedMessage serviceBusReceivedMessage)
	{

		ArgumentException.ThrowIfNullOrWhiteSpace(nameof(serviceBusReceivedMessage));

		JsonSerializerOptions options = new();
		options.Converters.Add(new InterfaceConverter<IMessage, ConcreteMessage>());
		IMessage? deserializedMessage = JsonSerializer.Deserialize<IMessage>(serviceBusReceivedMessage.Body.ToString(), options);

		if (deserializedMessage == null)
		{
			return null;
		}
		else if (deserializedMessage.MessageType == MessageTypes.OrderNextCore)
		{
			InventoryEventEntity orderNextCoreMessage = JsonSerializer.Deserialize<InventoryEventEntity>(serviceBusReceivedMessage.Body.ToString())!;
			return new()
			{
				Id = serviceBusReceivedMessage.MessageId,
				EventType = InventoryEventTypes.OrderNextCore,
				FinishedProductId = orderNextCoreMessage.FinishedProductId,
				PodId = orderNextCoreMessage.PodId,
				CoreId = orderNextCoreMessage.CoreId,
				Status = orderNextCoreMessage.Status,
				StatusDetail = null
			};
		}
		else
		{
			return null;
		}

	}

}