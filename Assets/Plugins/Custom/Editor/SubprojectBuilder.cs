using System.IO;
using UnityEngine;

public static class SubprojectBuilder {
	private static readonly string projectPath = "Assets";
	private static readonly string streamingAssetsSubPath = "StreamingAssets";

	public static void PrepareStreamingAssets (string subprojectSubPath) {
		string subprojectPath = $"{projectPath}/{subprojectSubPath}";
		string streamingAssetsPath = $"{subprojectPath}/{streamingAssetsSubPath}";

		// delete previous
		if (Directory.Exists(Application.streamingAssetsPath)) {
			Directory.Delete(Application.streamingAssetsPath, true);
		}

		// prepare directories
		Directory.CreateDirectory(Application.streamingAssetsPath.Replace(subprojectPath, projectPath));
		foreach (var path in Directory.GetDirectories(streamingAssetsPath, "*", SearchOption.AllDirectories)) {
			Directory.CreateDirectory(path.Replace(subprojectPath, projectPath));
		}

		// copy files
		foreach (var path in Directory.GetFiles(streamingAssetsPath, "*.*", SearchOption.AllDirectories)) {
			File.Copy(path, path.Replace(subprojectPath, projectPath), true);
		}
	}
}
