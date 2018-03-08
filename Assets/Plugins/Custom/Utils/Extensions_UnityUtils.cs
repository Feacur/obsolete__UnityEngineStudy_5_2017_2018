using UnityEngine;

public static partial class Extensions {
	public static T GetAutoMonoBehaviour<T>(bool dontDestroyOnLoad = false) where T : MonoBehaviour {
		var instance = Object.FindObjectOfType<T>();
		if (!instance) {
			instance = new GameObject(
				string.Format("Auto instance: {0}", typeof(T).Name)
			).AddComponent<T>();
		}
		if (dontDestroyOnLoad) {
			Object.DontDestroyOnLoad(instance.gameObject);
		}
		return instance;
	}

	public static T GetAutoScriptableObject<T>() where T : ScriptableObject {
		var instance = Object.FindObjectOfType<T>();
		if (!instance) {
			instance = ScriptableObject.CreateInstance<T>();
			instance.name = string.Format("Auto instance: {0}", typeof(T).Name);
		}
		return instance;
	}
	public static T GetResourceMonoBehaviour<T>(bool dontDestroyOnLoad = false) where T : MonoBehaviour {
		var instance = Object.FindObjectOfType<T>();
		if (!instance) {
			var prefab = Resources.Load<T>(typeof(T).Name);
			instance = Object.Instantiate(prefab);
		}
		if (dontDestroyOnLoad) {
			Object.DontDestroyOnLoad(instance.gameObject);
		}
		return instance;
	}

	public static T GetResourceScriptableObject<T>() where T : ScriptableObject {
		var instance = Object.FindObjectOfType<T>();
		if (!instance) {
			var prefab = Resources.Load<T>(typeof(T).Name);
			instance = Object.Instantiate(prefab);
		}
		return instance;
	}
}
