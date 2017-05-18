using UnityEngine;

///
/// Provides static access to MonoBehaviour of type <typeparam name="T">
/// If any is present and belongs to an active game object
///
/// Intended to be used like
/// public class ClassName : StaticInstanceBehaviour<ClassName> { ... }
///
public abstract class StaticInstanceBehaviour<T> : MonoBehaviour where T : StaticInstanceBehaviour<T> {
	private static T _instance;
	public static T instance {
		get {
			if (!_instance) {
				_instance = FindObjectOfType<T>();
			}
			return _instance;
		}
	}
}