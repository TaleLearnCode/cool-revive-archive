namespace Remanufacturing.Messages;

public class OrderNextCoreMessage : IMessage
{
	public string MessageId { get; set; } = Guid.NewGuid().ToString();
	public string MessageType { get; set; } = MessageTypes.OrderNextCore;
	public string PodId { get; set; } = null!;
	public string? CoreId { get; set; }
	public string? FinishedProductId { get; set; }
	public DateTime RequestDateTime { get; set; }
}