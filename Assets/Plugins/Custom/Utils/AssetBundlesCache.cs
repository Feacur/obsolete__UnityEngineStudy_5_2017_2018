using System;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundlesCache : AutoInstanceMonoBehaviour<AssetBundlesCache> {
	private readonly AsyncDataLoaderCached<AssetBundle> cache = new AsyncDataLoaderCached<AssetBundle>();
	
	public static Coroutine LoadAsync(string url, Action<AssetBundle> callback) {
		return instance.LoadDataAsync(url, (resultValue) => {
			if (callback != null) {
				callback(resultValue);
			}
		});
	}
	
	public static void Unload(string url, bool unloadAllLoadedObjects = true) {
		AssetBundle result = instance.cache.RemoveAndGet(url);
		if (result) {
			result.Unload(unloadAllLoadedObjects);
		}
	}

	private Coroutine LoadDataAsync(string url, Action<AssetBundle> callback) {
		return StartCoroutine(cache.LoadCoroutine(url, callback));
	}
}