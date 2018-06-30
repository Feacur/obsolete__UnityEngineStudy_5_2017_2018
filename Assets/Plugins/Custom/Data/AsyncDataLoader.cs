using System;
using System.Collections;
using UnityEngine;

namespace Custom.Data
{
	public static class AsyncDataLoader
	{
		///
		/// Loads an asset by <param name="assetPath"> from <param name="assetBundle">.
		/// Result will be sent async as a <param name="callback"> param.
		///
		/// Intended to be used something like
		/// StartCoroutine(assetBundle.LoadCoroutine<T>(assetPath, (resultValue) => { ... }));
		/// StartCoroutine(AsyncDataLoader.LoadCoroutine<T>(assetBundle, assetPath, (resultValue) => { ... }));
		///
		public static IEnumerator LoadCoroutine<T>(this AssetBundle assetBundle, string assetPath, Action<T> callback)
			where T : class
		{
			T result = default(T);

			if (!assetBundle)
			{
				Debug.LogError("Asset bundle is null");
			}
			else if (!assetBundle.Contains(assetPath))
			{
				Debug.LogError($"{assetBundle.name} does not contain {assetPath}");
			}
			else if (typeof(T) == typeof(byte[]))
			{
				var loadAssetAsync = assetBundle.LoadAssetAsync(assetPath);
				yield return loadAssetAsync;
				var textAsset = loadAssetAsync.asset as TextAsset;
				result = textAsset ? textAsset.bytes as T : default(T);
			}
			else if (typeof(T) == typeof(string))
			{
				var loadAssetAsync = assetBundle.LoadAssetAsync(assetPath);
				yield return loadAssetAsync;
				var textAsset = loadAssetAsync.asset as TextAsset;
				result = textAsset ? textAsset.text as T : default(T);
			}
			else
			{
				var loadAssetAsync = assetBundle.LoadAssetAsync(assetPath);
				yield return loadAssetAsync;
				result = loadAssetAsync.asset as T;
			}

			callback?.Invoke(result);
		}

		///
		/// Loads an asset from <param name="url">.
		/// Result will be sent async as a <param name="callback"> param.
		///
		/// Intended to be used something like
		/// StartCoroutine(AsyncDataLoader.LoadCoroutine<T>(url, (resultValue) => { ... }));
		///
		public static IEnumerator LoadCoroutine<T>(string url, Action<T> callback)
			where T : class
		{
			T result = default(T);

			using (var www = new WWW(url))
			{
				yield return www;

				if (!string.IsNullOrEmpty(www.error))
				{
					Debug.LogError($"Error loading {url}:{Environment.NewLine}{www.error}");
				}
				else if (www.bytesDownloaded == 0)
				{
					Debug.LogError($"Loaded zero bytes from {url}");
				}
				else if (typeof(T) == typeof(byte[]))
				{
					result = www.bytes as T;
				}
				else if (typeof(T) == typeof(string))
				{
					result = www.text as T;
				}
				else if (typeof(T) == typeof(Texture2D))
				{
					result = www.texture as T;
				}
				else if (typeof(T) == typeof(AudioClip))
				{
					result = www.GetAudioClipCompressed() as T;
				}
				else if (typeof(T) == typeof(AssetBundle))
				{
					result = www.assetBundle as T;
				}
			}

			callback?.Invoke(result);
		}
	}
}
