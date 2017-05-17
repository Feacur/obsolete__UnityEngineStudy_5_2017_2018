using UnityEngine;

public static class UnityUtils {
	public static string StreamingAssetsUrl {
		get {
			#if UNITY_EDITOR || UNITY_STANDALONE
			return string.Format("file://{0}", Application.streamingAssetsPath);
			#else
			return Application.streamingAssetsPath;
			#endif
		}
	}
}