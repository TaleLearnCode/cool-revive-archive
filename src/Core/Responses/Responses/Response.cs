#nullable disable

using System.Net;

namespace Remanufacturing.Responses;

internal class Response : IResponse
{
	public ResponseType ResponseType { get; set; } = ResponseType.Response;
	public string Type { get; set; }
	public string Title { get; set; }
	public HttpStatusCode Status { get; set; }
	public string? Detail { get; set; }
	public string? Instance { get; set; }
	public Dictionary<string, object>? Extensions { get; set; }
}