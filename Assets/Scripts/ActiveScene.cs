using UnityEngine;
using UnityEngine.SceneManagement;

///
/// Marks current scene active
///
/// Active scene is the default parent for new game objects
/// Active scene determines current lighting settings
///
public class ActiveScene : MonoBehaviour {
	void Start () {
		SceneManager.SetActiveScene(gameObject.scene);
	}
}
