using System.Collections.Generic;
using Custom.Singleton;
using UnityEngine;

///
/// Provides convenient way to fetch <see cref="Screen"> objects without juggling references all around
/// Use <see cref="ScreensBundle"> in order to register your <see cref="Screen"> automatically
///
/// Then you can easily access registered panel with ScreensRegistry.Get<ScreenClassName>();
///
public class ScreensRegistry : AutoInstanceMonoBehaviour<ScreensRegistry>
{
	public List<GameObject> registry = new List<GameObject>();

	public void Add(Screen panel) {
		registry.Add(panel.gameObject);
	}
	
	public void Remove(GameObject panel) {
		registry.Remove(panel);
	}

	public T Get<T>() where T : MonoBehaviour {
		return registry
			.ConvertAll(it => it.GetComponent<T>())
			.Find(it => it != null);
	}
}
