#nullable disable

namespace Remanufacturing.Messages;

public class ConcreteMessage : IMessage
{
	public string MessageId { get; set; }
	public string MessageType { get; set; }
}