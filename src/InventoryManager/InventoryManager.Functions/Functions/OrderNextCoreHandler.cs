using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Remanufacturing.InventoryManager.Entities;
using Remanufacturing.InventoryManager.Extensions;

namespace Remanufacturing.InventoryManager.Functions;

public class OrderNextCoreHandler(ILogger<OrderNextCoreHandler> logger)
{
	private readonly ILogger<OrderNextCoreHandler> _logger = logger;

	[Function(nameof(OrderNextCoreHandler))]
	[CosmosDBOutput(
		databaseName: "%EventSourceDatabaseName%",
		containerName: "%EventSourceContainerName%",
		PartitionKey = "%EventSourcePartitionKey%",
		Connection = "CosmosDBConnectionString",
		CreateIfNotExists = false)]
	public async Task<InventoryEventEntity?> RunAsync(
		[ServiceBusTrigger("%OrderNextCoreTopicName%", "%OrderNextCoreSubscriptionName%", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage message,
		ServiceBusMessageActions messageActions)
	{
		_logger.LogInformation("Message ID: {id}", message.MessageId);
		_logger.LogInformation("Message Body: {body}", message.Body);
		_logger.LogInformation("Message Content-Type: {contentType}", message.ContentType);

		InventoryEventEntity? inventoryEventEntity = message.ToInventoryEventEntity();

		// Complete the message
		await messageActions.CompleteMessageAsync(message);

		// Save the message to the Cosmos DB
		return inventoryEventEntity;

	}

}