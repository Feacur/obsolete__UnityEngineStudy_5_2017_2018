using UnityEngine;
using UnityEngine.SceneManagement;

///
/// Marks current scene active
///
public class ActiveScene : MonoBehaviour {
	void Start () {
		SceneManager.SetActiveScene(gameObject.scene);
	}
}
