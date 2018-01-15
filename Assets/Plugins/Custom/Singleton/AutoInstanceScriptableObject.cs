using UnityEngine;

///
/// Provides static access to ScriptableObject of type <typeparam name="T">
/// Creates an instance if none is present
///
/// Use <see cref="destroyed"> to protect against unwanted instantiations
/// Like when accessing <see cref="instance"> inside OnDisable method
///
/// Intended to be used like
/// public class ClassName : AutoInstanceScriptableObject<ClassName> { ... }
///
public abstract class AutoInstanceScriptableObject<T> : ScriptableObject
	where T : AutoInstanceScriptableObject<T>
{
	//
	// API
	//

	public static bool destroyed { get; private set; }

	private static T _instance;
	public static T instance {
		get {
			if (!_instance) {
				_instance = Extensions.GetAutoScriptableObject<T>();
				
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
