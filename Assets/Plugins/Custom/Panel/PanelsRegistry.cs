using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

///
/// Provides convenient way to fetch <see cref="Panel"> objects without juggling references all around
/// Use <see cref="PanelsBundle"> in order to register your <see cref="Panel"> automatically
///
/// Then you can easily access registered panel with PanelsRegistry.Get<PanelClassName>();
///
public class PanelsRegistry : AutoInstanceMonoBehaviour<PanelsRegistry>
{
	public List<GameObject> registry = new List<GameObject>();

	public void Add(Panel panel) {
		registry.Add(panel.gameObject);
	}
	
	public void Remove(GameObject panel) {
		registry.Remove(panel);
	}

	public T Get<T>() where T : MonoBehaviour {
		return registry
			.Select(it => it.GetComponent<T>())
			.FirstOrDefault(it => it != null);
	}
}
