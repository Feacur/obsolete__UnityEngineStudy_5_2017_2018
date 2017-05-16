using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
