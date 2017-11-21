#define EMULATE_ASSET_BUNDLES_IN_EDIT_MODE

using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

///
/// Conveniency script to work with Application.streamingAssetsPath
///
/// Define EMULATE_ASSET_BUNDLES_IN_EDIT_MODE to bypass asset bundles for editor whatsoever
/// Use <see cref="realAssetBundles"> to determine whether EMULATE_ASSET_BUNDLES_IN_EDIT_MODE is in use in your runtime code
///
public class StreamingData : AutoInstanceMonoBehaviour<StreamingData>
{
	#if UNITY_EDITOR && EMULATE_ASSET_BUNDLES_IN_EDIT_MODE
	public static readonly bool realAssetBundles = false;
	#else
	public static readonly bool realAssetBundles = true;
	#endif

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

	private AssetBundlesCache AssetBundlesCache;
	protected override void AutoInstanceInit() {
		this.AssetBundlesCache = AssetBundlesCache.instance;
	}

	public Coroutine LoadAssetBundleAsync(string assetBundlePath, Action<AssetBundle> callback = null) {
		return LoadAssetBundleAsyncInternal(assetBundlePath, callback);
	}

	public Coroutine LoadDataAsync<T>(string subPath, Action<T> callback) where T : class {
		return LoadDataAsyncInternal(subPath, callback);
	}

	#if UNITY_EDITOR && EMULATE_ASSET_BUNDLES_IN_EDIT_MODE
	public Coroutine LoadAssetAsync<T>(string assetBundlePath, string assetPath, Action<T> callback) where T : UnityEngine.Object {
		var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
		if (callback != null) {
			callback(asset);
		}
		return null;
	}
	#else
	public Coroutine LoadAssetAsync<T>(string assetBundlePath, string assetPath, Action<T> callback) where T : UnityEngine.Object {
		return StartCoroutine(
			LoadAssetAsyncCoroutine(assetBundlePath, assetPath, callback)
		);
	}
	#endif

	#if UNITY_EDITOR && EMULATE_ASSET_BUNDLES_IN_EDIT_MODE
	public IEnumerator LoadScenesAsync(string assetBundlePath, LoadSceneMode mode) {
		// 'subpath1/subpath2/subpath3' -> 'subpath3'
		string assetBundleName = System.IO.Path.GetFileName(assetBundlePath);
		
		var assetPaths = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(
			assetBundleName
		);

		var scenePaths = assetPaths.Where(it => it.EndsWith(".unity")).ToArray();

		if (mode == LoadSceneMode.Additive) {
			foreach (var scenePath in scenePaths) {
				yield return UnityEditor.EditorApplication.LoadLevelAdditiveAsyncInPlayMode(scenePath);
			}
		}
		else {
			foreach (var scenePath in scenePaths) {
				yield return UnityEditor.EditorApplication.LoadLevelAsyncInPlayMode(scenePath);
			}
		}
	}
	#else
	public IEnumerator LoadScenesAsync(string assetBundlePath, LoadSceneMode mode) {
		AssetBundle assetBundle = null;
		yield return LoadAssetBundleAsyncInternal(assetBundlePath, (resultValue) => {
			assetBundle = resultValue;
		});

		if (!assetBundle) {
			Debug.LogErrorFormat("Asset bundle hasn't been loaded: {0}", assetBundlePath);
			yield break;
		}

		foreach (var scenePath in assetBundle.GetAllScenePaths()) {
			yield return SceneManager.LoadSceneAsync(scenePath, mode);
		}
	}
	#endif

	private IEnumerator LoadAssetAsyncCoroutine<T>(string assetBundlePath, string assetName, Action<T> callback) where T : UnityEngine.Object {
		AssetBundle assetBundle = null;
		yield return LoadAssetBundleAsyncInternal(assetBundlePath, (resultValue) => {
			assetBundle = resultValue;
		});
		
		for (var e = assetBundle.LoadCoroutine(assetName, callback); e.MoveNext();) {
			yield return e.Current;
		}
	}

	private Coroutine LoadAssetBundleAsyncInternal(string assetBundlePath, Action<AssetBundle> callback) {
		string url = string.Format("{0}/{1}", StreamingAssetsUrl, assetBundlePath);
		return AssetBundlesCache.LoadAsync(url, callback);
	}
	
	private Coroutine LoadDataAsyncInternal<T>(string subPath, Action<T> callback) where T : class {
		string url = string.Format("{0}/{1}", StreamingAssetsUrl, subPath);
		return StartCoroutine(AsyncDataLoader.LoadCoroutine(url, callback));
	}
}