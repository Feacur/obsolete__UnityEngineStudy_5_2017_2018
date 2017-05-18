using UnityEngine;

public static class UnityUtils {
	///
	/// From WWW description: https://docs.unity3d.com/ScriptReference/WWW.html
	/// Note: When using file protocol on Windows and Windows Store Apps for accessing local files, you have to specify file:/// (with three slashes).
	///
	public static string StreamingAssetsUrl {
		get {
			#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN || UNITY_WSA || UNITY_WSA_8_1 || UNITY_WSA_10_0
			return string.Format("file:///{0}", Application.streamingAssetsPath);
			#else
			return Application.streamingAssetsPath;
			#endif
		}
	}
}