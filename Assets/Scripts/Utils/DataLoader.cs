using System;
using System.Collections;
using UnityEngine;

public static class DataLoader {

	public static IEnumerator LoadCoroutine<T>(AssetBundle assetBundle, string assetName, Action<T> callback) where T : class {
		T result = null;
		
		if (!assetBundle) {
			Debug.LogErrorFormat("Asset bundle is null");
		}
		else if (!assetBundle.Contains(assetName)) {
			Debug.LogErrorFormat("{0} does not contain {1}", assetBundle.name, assetName);
		}
		else {
			var loadAssetAsync = assetBundle.LoadAssetAsync(assetName);
			yield return loadAssetAsync;
			result = loadAssetAsync.asset as T;
		}

		if (callback != null) {
			callback.Invoke(result);
		}
	}

	public static IEnumerator LoadCoroutine<T>(string url, Action<T> callback) where T : class {
		T result = null;
		using(var www = new WWW(url)) {
			yield return www;
			if (!string.IsNullOrEmpty(www.error)) {
				Debug.LogError(www.error);
			}
			else if (www.bytesDownloaded == 0) {
				Debug.LogErrorFormat("Loaded zero bytes from {0}", url);
			}
			else if (typeof(T) == typeof(string)){
				result = www.text as T;
			}
			else if (typeof(T) == typeof(Texture2D)){
				result = www.texture as T;
			}
			else if (typeof(T) == typeof(AssetBundle)){
				result = www.assetBundle as T;
			}
		}
		if (callback != null) {
			callback.Invoke(result);
		}
	}
}