using System;
using System.Collections.Generic;
using UnityEngine;

public class CachedAssetBundleLoader : AutoInstanceMonoBehaviour<CachedAssetBundleLoader> {
	private readonly Dictionary<string, AssetBundle> cache = new Dictionary<string, AssetBundle>();

	public static Coroutine LoadAsync(string url, Action<AssetBundle> callback) {
		AssetBundle result = null;
		if (instance.cache.TryGetValue(url, out result)) {
			if (callback != null) {
				callback(result);
			}
			return null;
		}

		return instance.LoadDataAsync(url, (resultValue) => {
			instance.cache.Add(url, resultValue);
			if (callback != null) {
				callback(resultValue);
			}
		});
	}
	
	public static void Unload(string url, bool unloadAllLoadedObjects = true) {
		AssetBundle result = null;
		if (instance.cache.TryGetValue(url, out result)) {
			instance.cache.Remove(url);
			result.Unload(unloadAllLoadedObjects);
		}
	}

	private Coroutine LoadDataAsync(string url, Action<AssetBundle> callback) {
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(url, callback));
	}
}