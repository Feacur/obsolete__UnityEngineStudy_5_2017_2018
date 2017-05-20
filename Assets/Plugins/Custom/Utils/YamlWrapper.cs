using YamlDotNet.Serialization;

public static class YamlWrapper {
	private static readonly Serializer serializer = new Serializer();
	private static readonly Deserializer deserializer = new Deserializer();

	public static string Serialize<T>(T value) where T: class {
		return serializer.Serialize(value);
	}

	public static T Deserialize<T>(string yaml) where T: class {
		return deserializer.Deserialize<T>(yaml);
	}
}
