using UnityEngine;

///
/// Provides static access to ScriptableObject of type <typeparam name="T">
/// Creates an instance if none is present
/// Use <see cref="destroyed"> to protect against unwanted instantiations
///
/// Intended to be used like
/// public class ClassName : AutoInstanceScriptableObject<ClassName> { ... }
///
public abstract class AutoInstanceScriptableObject<T> : ScriptableObject where T : AutoInstanceScriptableObject<T> {
	public static bool destroyed { get; private set; }

	protected static T _instance;
	public static T instance {
		get {
			destroyed = false;
			if (!_instance) {
				_instance = FindObjectOfType<T>();
				if (!_instance) {
					_instance = CreateInstance<T>();
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