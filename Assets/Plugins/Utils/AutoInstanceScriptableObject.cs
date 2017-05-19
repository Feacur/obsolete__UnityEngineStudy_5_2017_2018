using UnityEngine;

///
/// Provides static access to ScriptableObject of type <typeparam name="T">
/// Creates an instance if none is present
///
/// Intended to be used like
/// public class ClassName : AutoInstanceScriptableObject<ClassName> { ... }
///
public abstract class AutoInstanceScriptableObject<T> : ScriptableObject where T : AutoInstanceScriptableObject<T> {
	private static T _instance;
	public static T instance {
		get {
			if (!_instance) {
				_instance = FindObjectOfType<T>();
				if (!_instance) {
					_instance = CreateInstance<T>();
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