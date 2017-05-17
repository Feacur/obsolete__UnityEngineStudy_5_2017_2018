using System;
using System.Collections;
using UnityEngine;

public static class DataLoader {
	///
	/// Loads an asset by <param name="assetName"> from <param name="assetBundle">.
	/// Result will be sent async as a <param name="callback"> param.
	///
	/// Intended to be used something like StartCoroutine(LoadAsyncCoroutine<T>(assetBundle, assetName, (resultValue) => { ... }));
	/// It is advised to make a wrapper handling StartCoroutine(); part.
	///
	public static IEnumerator LoadAsyncCoroutine<T>(AssetBundle assetBundle, string assetName, Action<T> callback) where T : class {
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

	///
	/// Loads an asset from <param name="url">.
	/// Result will be sent async as a <param name="callback"> param.
	///
	/// Intended to be used something like StartCoroutine(LoadAsyncCoroutine<T>(url, (resultValue) => { ... }));
	/// It is advised to make a wrapper handling StartCoroutine(); part.
	///
	public static IEnumerator LoadAsyncCoroutine<T>(string url, Action<T> callback) where T : class {
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