using System;
using System.Collections.Generic;
using System.Linq;
using Custom.Singleton;
using UnityEngine;

namespace Custom.Data
{
	///
	/// Class to manage asset bundles
	/// Unity forbids multiple instances of the same asset bundle
	///
	/// I could have used regular C# Dictionary for <see cref="cache">, however Unity won't serialize it.
	/// As a result hot reloading wouldn't work, and I don't want to sacrifice this feature.
	///
	public class AssetBundlesCache : AutoInstanceMonoBehaviour<AssetBundlesCache>
	{
		public List<Entry> cache = new List<Entry>();

		public Coroutine LoadAsync(string url, Action<AssetBundle> callback = null)
		{
			var result = cache.SingleOrDefault(it => it.key == url);
			if (result != null)
			{
				callback?.Invoke(result.value);
				return null;
			}

			return LoadDataAsync<AssetBundle>(url, (resultValue) =>
			{
				cache.Add(new Entry()
				{
					key = url,
					value = resultValue
				});
				callback?.Invoke(resultValue);
			});
		}

		public void Unload(string url, bool unloadAllLoadedObjects = true)
		{
			var result = cache.SingleOrDefault(it => it.key == url);
			result?.value.Unload(unloadAllLoadedObjects);
		}

		private Coroutine LoadDataAsync<T>(string url, Action<T> callback) where T : class
		{
			return StartCoroutine(AsyncDataLoader.LoadCoroutine(url, callback));
		}

		[Serializable]
		public class Entry
		{
			public string key;
			public AssetBundle value;
		}
	}
}
