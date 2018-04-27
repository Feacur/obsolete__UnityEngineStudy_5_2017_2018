using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Custom.Data
{
	///
	/// Conveniency script to work with Application.persistentDataPath
	///
	public static class PersistentData
	{
		public static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

		public static void WriteYaml<T>(string subPath, T data) where T : class
		{
			string path = $"{Application.persistentDataPath}/{subPath}";
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			using (var streamWriter = File.CreateText(path))
			{
				YamlWrapper.serializer.Serialize(streamWriter, data);
			}
		}

		public static T ReadYaml<T>(string subPath) where T : class
		{
			string path = $"{Application.persistentDataPath}/{subPath}";
			if (!File.Exists(path))
			{
				return null;
			}

			using (var streamReader = File.OpenText(path))
			{
				return YamlWrapper.deserializer.Deserialize<T>(streamReader);
			}
		}

		public static void WriteBinary<T>(string subPath, T data) where T : class
		{
			string path = $"{Application.persistentDataPath}/{subPath}";
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			using (var streamWriter = File.Create(path))
			{
				binaryFormatter.Serialize(streamWriter, data);
			}
		}

		public static T ReadBinary<T>(string subPath) where T : class
		{
			string path = $"{Application.persistentDataPath}/{subPath}";
			if (!File.Exists(path))
			{
				return null;
			}

			using (var streamReader = File.Open(path, FileMode.Open, FileAccess.Read))
			{
				return binaryFormatter.Deserialize(streamReader) as T;
			}
		}
	}
}
