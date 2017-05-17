using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

///
/// Asynchronously loads additional scenes in <see cref="sceneNames">
///
/// It is useful to store UI layout in scenes and load it in runtime.
///	This way it is possible to use prefabs for repetitive elements.
///
/// Again, it's useful if you want some sort of initial loading screen.
///
public class SceneLoader : MonoBehaviour {
	public string[] sceneNames;

	private IEnumerator Start () {
		foreach (var sceneName in sceneNames)
		{
			var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
			yield return asyncOperation;
		}
	}
}
