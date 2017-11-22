using UnityEngine;

///
/// Provides static access to MonoBehaviour of type <typeparam name="T">
/// Creates persistent game object if none is present
///
/// Use <see cref="destroyed"> to protect against unwanted instantiations
/// Like when accessing <see cref="instance"> inside OnDisable method
///
/// Please note that auto created instances will be marked DontDestroyOnLoad
/// Thus consequently be placed into the very same named scene
///
/// Intended to be used like
/// public class ClassName : AutoInstanceMonoBehaviour<ClassName> { ... }
///
public abstract class AutoInstanceMonoBehaviour<T> : MonoBehaviour
	where T : AutoInstanceMonoBehaviour<T>
{
	//
	// API
	//

	public static bool destroyed { get; private set; }

	private static T _instance;
	public static T instance {
		get {
			if (!_instance) {
				_instance = UnityExtensions.GetAutoMonoBehaviour<T>(dontDestroyOnLoad: true);
				
				destroyed = false;
				_instance.OnInit();
			}
			return _instance;
		}
	}

	protected virtual void OnInit() { }

	//
	// Callbacks from Unity
	//

	protected void Awake() {
		if (!_instance) {
			instance.OnInit();
		}
	}

	protected void OnDestroy() {
		destroyed = true;
	}
}
