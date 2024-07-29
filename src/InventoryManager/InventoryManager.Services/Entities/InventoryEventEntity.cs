using System.Text.Json.Serialization;

namespace Remanufacturing.InventoryManager.Entities;

public class InventoryEventEntity
{
	[JsonPropertyName("id")]
	public string Id { get; set; } = Guid.NewGuid().ToString();

	[JsonPropertyName("eventType")]
	public string EventType { get; set; } = null!;

	[JsonPropertyName("finishedProductId")]
	public string FinishedProductId { get; set; } = null!;

	[JsonPropertyName("podId")]
	public string PodId { get; set; } = null!;

	[JsonPropertyName("coreId")]
	public string CoreId { get; set; } = null!;

	[JsonPropertyName("status")]
	public string Status { get; set; } = null!;

	[JsonPropertyName("statusDetail")]
	public string? StatusDetail { get; set; }

	[JsonPropertyName("eventTimestamp")]
	public string EventTimestamp { get; set; } = DateTime.UtcNow.ToString();

}