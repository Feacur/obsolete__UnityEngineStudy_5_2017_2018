using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

///
/// Class to manage asset bundles
/// Unity forbids multiple instances of the same asset bundle
///
public class AssetBundlesCache : AutoInstanceMonoBehaviour<AssetBundlesCache> {
	public List<Entry> cache = new List<Entry>();
	
	public static Coroutine LoadAsync(string url, Action<AssetBundle> callback) {
		var result = instance.cache.SingleOrDefault(it => it.key == url);
		if (result != null) {
			if (callback != null) {
				callback(result.value);
			}
			return null;
		}

		return instance.LoadDataAsync<AssetBundle>(url, (resultValue) => {
			instance.cache.Add(new Entry() {
				key = url,
				value = resultValue
			});
			if (callback != null) {
				callback(resultValue);
			}
		});
	}
	
	public static void Unload(string url, bool unloadAllLoadedObjects = true) {
		var result = instance.cache.SingleOrDefault(it => it.key == url);
		if (result != null) {
			result.value.Unload(unloadAllLoadedObjects);
		}
	}
	
	private Coroutine LoadDataAsync<T>(string url, Action<T> callback) where T : class {
		return StartCoroutine(AsyncDataLoader.LoadCoroutine(url, callback));
	}

	[Serializable]
	public class Entry {
		public string key;
		public AssetBundle value;
	}
}