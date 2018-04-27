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
		/// It is advised to make a wrapper handling StartCoroutine(); part.
		///
		public static IEnumerator LoadCoroutine<T>(this AssetBundle assetBundle, string assetPath, Action<T> callback)
			where T : class
		{
			T result = null;

			if (!assetBundle)
			{
				Debug.LogErrorFormat("Asset bundle is null");
			}
			else if (!assetBundle.Contains(assetPath))
			{
				Debug.LogErrorFormat("{0} does not contain {1}", assetBundle.name, assetPath);
			}
			else if (typeof(T) == typeof(byte[]))
			{
				var loadAssetAsync = assetBundle.LoadAssetAsync(assetPath);
				yield return loadAssetAsync;
				var textAsset = loadAssetAsync.asset as TextAsset;
				result = textAsset ? textAsset.bytes as T : null;
			}
			else if (typeof(T) == typeof(string))
			{
				var loadAssetAsync = assetBundle.LoadAssetAsync(assetPath);
				yield return loadAssetAsync;
				var textAsset = loadAssetAsync.asset as TextAsset;
				result = textAsset ? textAsset.text as T : null;
			}
			else
			{
				var loadAssetAsync = assetBundle.LoadAssetAsync(assetPath);
				yield return loadAssetAsync;
				result = loadAssetAsync.asset as T;
			}

			if (callback != null)
			{
				callback.Invoke(result);
			}
		}

		///
		/// Loads an asset from <param name="url">.
		/// Result will be sent async as a <param name="callback"> param.
		///
		/// Intended to be used something like
		/// StartCoroutine(AsyncDataLoader.LoadCoroutine<T>(url, (resultValue) => { ... }));
		///
		/// It is advised to make a wrapper handling StartCoroutine(); part.
		///
		public static IEnumerator LoadCoroutine<T>(string url, Action<T> callback, float idleTimeoutSeconds = 10,
			float idleProgressThreshold = float.Epsilon) where T : class
		{
			T result = null;

			using (var www = new WWW(url))
			{
				if (idleTimeoutSeconds <= 0)
				{
					yield return www;
				}
				else
				{
					for (var e = WaitLoadingWithIdleTimeoutCoroutine(www, idleTimeoutSeconds, idleProgressThreshold); e.MoveNext();)
					{
						yield return e.Current;
					}
				}

				if (!string.IsNullOrEmpty(www.error))
				{
					Debug.LogErrorFormat("Error loading {0}\r\n{1}", url, www.error);
				}
				else if (www.bytesDownloaded == 0)
				{
					Debug.LogErrorFormat("Loaded zero bytes from {0}", url);
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
#if !(UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL)
				else if (typeof(T) == typeof(MovieTexture))
				{
					result = www.GetMovieTexture() as T;
				}
#endif
				else if (typeof(T) == typeof(AssetBundle))
				{
					result = www.assetBundle as T;
				}
			}

			callback?.Invoke(result);
		}

		private static IEnumerator WaitLoadingWithIdleTimeoutCoroutine(WWW www, float idleTimeoutSeconds,
			float idleProgressThreshold)
		{
			float stepSeconds = 0.1f;
			float idleSeconds = 0;
			float lastProgress = www.progress;
			while (!www.isDone && (idleSeconds < idleTimeoutSeconds))
			{
				float progressDelta = www.progress - lastProgress;
				lastProgress = www.progress;
				if ((progressDelta < 0) || (progressDelta > idleProgressThreshold))
				{
					idleSeconds = 0;
				}
				else
				{
					idleSeconds += stepSeconds;
				}

				yield return new WaitForSecondsRealtime(stepSeconds);
			}
		}
	}
}
