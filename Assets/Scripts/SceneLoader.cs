using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

///
/// Asynchronously loads additional scenes in <see cref="scenesAssetBundleSubPath">
///
/// It is useful to store UI layout in scenes and load it in runtime.
///	This way it is possible to use prefabs for repetitive elements.
///
/// Again, it's useful if you want some sort of initial loading screen.
///
public class SceneLoader : MonoBehaviour {
	public string scenesAssetBundleSubPath;

	private IEnumerator Start () {
		// Get scenes asset bundle
		AssetBundle scenesAssetBundle = null;
		string scenesAssetBundleUrl = string.Format("{0}/{1}", UnityUtils.StreamingAssetsUrl, scenesAssetBundleSubPath);
		yield return CachedAssetBundleLoader.LoadAsync(scenesAssetBundleUrl, (resultValue) => {
			scenesAssetBundle = resultValue;
		});

		// Load scenes
		foreach (var scenePath in scenesAssetBundle.GetAllScenePaths()) {
			var asyncOperation = SceneManager.LoadSceneAsync(scenePath, LoadSceneMode.Additive);
			yield return asyncOperation;
		}
	}

	private Coroutine LoadDataAsync<T>(string url, Action<T> callback) where T : class {
		return StartCoroutine(DataLoader.LoadAsyncCoroutine(url, callback));
	}
}
