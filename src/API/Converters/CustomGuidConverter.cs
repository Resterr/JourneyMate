using System.Text.Json;
using System.Text.Json.Serialization;
using JourneyMate.Application.Common.Exceptions;

namespace JourneyMate.API.Converters;

public class CustomGuidConverter : JsonConverter<Guid>
{
	public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		if (!Guid.TryParse(value, out var guid))
		{
			throw new InvalidGuidFormatException(value);
		}

		return guid;
	}

	public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToString("D"));
	}
}