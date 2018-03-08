using System;
using UnityEngine;

///
/// Provides static access to MonoBehaviour of type <typeparam name="T">
/// If any is present and belongs to an active game object
///
/// Intended to be used like
/// public class ClassName : StaticInstanceMonoBehaviour<ClassName> { ... }
///
public abstract class StaticInstanceMonoBehaviour<T> : MonoBehaviour
	where T : StaticInstanceMonoBehaviour<T>
{
	//
	// API
	//

	private static T _instance;
	public static T instance {
		get {
			if (_instance) { return _instance; }
			return FindObjectOfType<T>();
		}
	}

	//
	// Callbacks from Unity
	//

	protected void Awake() {
		if (!_instance) {
			_instance = (T)this;
		}
	}
}
