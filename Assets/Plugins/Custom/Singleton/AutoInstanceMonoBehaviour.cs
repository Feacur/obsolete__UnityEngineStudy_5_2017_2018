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
	public static bool destroyed { get; private set; }

	protected static T _instance;
	public static T instance {
		get {
			destroyed = false;
			if (!_instance) {
				_instance = FindObjectOfType<T>();
				if (!_instance) {
					string instanceName = string.Format("Auto instance: {0}", typeof(T).Name);
					var instanceGO = new GameObject(instanceName);
					DontDestroyOnLoad(instanceGO);
					_instance = instanceGO.AddComponent<T>();
				}
				// _instance should be non-null by now
				_instance.AutoInstanceInit();
			}
			return _instance;
		}
	}

	protected virtual void AutoInstanceInit() { }

	protected void OnDestroy() {
		destroyed = true;
	}
}
