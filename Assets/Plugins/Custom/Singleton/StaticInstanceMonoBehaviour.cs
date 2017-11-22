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
			if (!_instance) {
				_instance = FindObjectOfType<T>();
				
				if (_instance) { _instance.OnInit(); }
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
}
