using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

///
/// Provides convenient way to fetch panel objects without juggling references all around
///
public class PanelsRegistry : AutoInstanceMonoBehaviour<PanelsRegistry> {
	private readonly List<GameObject> registry = new List<GameObject>();

	public static void Add(Panel panel) {
		instance.registry.Add(panel.gameObject);
	}
	
	public static void Remove(GameObject panel) {
		instance.registry.Remove(panel);
	}

	public static T Get<T>() where T : MonoBehaviour {
		return instance.registry
			.Select(it => it.GetComponent<T>())
			.FirstOrDefault(it => it != null);
	}
}