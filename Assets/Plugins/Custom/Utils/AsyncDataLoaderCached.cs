using System;
using System.Collections;
using System.Collections.Generic;

public class AsyncDataLoaderCached<T> where T : class {
	private readonly Dictionary<string, T> cache = new Dictionary<string, T>();

	public IEnumerator LoadCoroutine(string url, Action<T> callback) {
		T result = null;
		if (cache.TryGetValue(url, out result)) {
			if (callback != null) {
				callback(result);
			}
			yield break;
		}

		for (var e = LoadAndCacheCoroutine(url, callback); e.MoveNext();) {
			yield return e.Current;
		}
	}

	public T RemoveAndGet(string url) {
		T result = null;
		if (cache.TryGetValue(url, out result)) {
			cache.Remove(url);
		}
		return result;
	}
	
	private IEnumerator LoadAndCacheCoroutine(string url, Action<T> callback) {
		return AsyncDataLoader.LoadCoroutine<T>(url, (resultValue) => {
			cache.Add(url, resultValue);
			if (callback != null) {
				callback(resultValue);
			}
		});
	}
}