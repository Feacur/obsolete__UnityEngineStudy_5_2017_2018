using YamlDotNet.Serialization;

namespace Custom.Data
{
	///
	/// Conveniency script to work with YamlDotNet library
	///
	/// Note (Feacur, 2017, May 28):
	/// Currently this is a sufficient setup for my needs and can be safely used project-wide
	/// There is no reason to have any other specific settings
	///
	public static class YamlWrapper
	{
		public static readonly Serializer serializer;
		public static readonly Deserializer deserializer;

		static YamlWrapper()
		{
			var serializerBuilder = new SerializerBuilder();
			serializer = serializerBuilder.Build();

			var deserializerBuilder = new DeserializerBuilder();
			deserializerBuilder.IgnoreUnmatchedProperties();
			deserializer = deserializerBuilder.Build();
		}

		public static string Serialize<T>(T value) where T : class
		{
			return serializer.Serialize(value);
		}

		public static T Deserialize<T>(string yaml) where T : class
		{
			return deserializer.Deserialize<T>(yaml);
		}
	}
}
