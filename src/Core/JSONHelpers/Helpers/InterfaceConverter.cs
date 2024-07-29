using System.Text.Json;
using System.Text.Json.Serialization;

namespace Remanufacturing.Helpers;

public class InterfaceConverter<TInterface, TConcrete> : JsonConverter<TInterface> where TConcrete : TInterface, new()
{

	public override TInterface? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var jsonDocument = JsonDocument.ParseValue(ref reader);
		var jsonObject = jsonDocument.RootElement.GetRawText();
		return JsonSerializer.Deserialize<TConcrete>(jsonObject, options);
	}

	public override void Write(Utf8JsonWriter writer, TInterface value, JsonSerializerOptions options)
	{
		JsonSerializer.Serialize(writer, (TConcrete)value!, options);
	}

}