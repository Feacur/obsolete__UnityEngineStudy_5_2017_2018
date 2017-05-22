using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

///
/// Asynchronously loads additional scenes from <see cref="scenesAssetBundleSubPath">
///
/// It is useful to store UI layout in scenes and load it in runtime.
///	This way it is possible to use prefabs for repetitive elements.
///
/// Again, it's useful if you want some sort of initial loading screen.
///
/// Also loads <see cref="environmentAssetBundleSubPath">, but this probably should be factored out
/// Currently I use this one to store hangar environment assets
///
/// Surely, there is no much use for this script afterwards
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
