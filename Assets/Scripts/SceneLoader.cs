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
	public string environmentAssetBundleSubPath;
	public string scenesAssetBundleSubPath;

	private IEnumerator Start () {
		if (StreamingData.realAssetBundles) {
			// Load environment assets
			yield return StreamingData.LoadAssetBundleAsync(environmentAssetBundleSubPath);
		}

		// Load scenes
		yield return StreamingData.LoadScenesAsync(scenesAssetBundleSubPath, LoadSceneMode.Additive);

		// Unload loader scene
		SceneManager.UnloadSceneAsync(gameObject.scene);
	}
}
