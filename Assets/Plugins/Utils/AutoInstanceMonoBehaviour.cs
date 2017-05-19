using UnityEngine;

///
/// Provides static access to MonoBehaviour of type <typeparam name="T">
/// Creates persistent game object if none is present
///
/// Intended to be used like
/// public class ClassName : AutoInstanceMonoBehaviour<ClassName> { ... }
///
public abstract class AutoInstanceMonoBehaviour<T> : MonoBehaviour where T : AutoInstanceMonoBehaviour<T> {
	private static T _instance;
	public static T instance {
		get {
			if (!_instance) {
				_instance = FindObjectOfType<T>();
				if (!_instance) {
					string instanceName = string.Format("Auto instance: {0}", typeof(T).Name);
					var instanceGO = new GameObject(instanceName);
					DontDestroyOnLoad(instanceGO);
					_instance = instanceGO.AddComponent<T>();
				}
				if (_instance) {
					_instance.AutoInstanceInit();
				}
			}
			return _instance;
		}
	}

	protected virtual void AutoInstanceInit() { }
}