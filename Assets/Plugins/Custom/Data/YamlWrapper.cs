using YamlDotNet.Serialization;

///
/// Conveniency script to work with YamlDotNet library
///
public static class YamlWrapper {
	public static readonly Serializer serializer;
	public static readonly Deserializer deserializer;

	static YamlWrapper() {
		var serializerBuilder = new SerializerBuilder();
		serializer = serializerBuilder.Build();

		var deserializerBuilder = new DeserializerBuilder();
		deserializerBuilder.IgnoreUnmatchedProperties();
		deserializer = deserializerBuilder.Build();
	}

	public static string Serialize<T>(T value) where T: class {
		return serializer.Serialize(value);
	}

	public static T Deserialize<T>(string yaml) where T: class {
		return deserializer.Deserialize<T>(yaml);
	}
}
