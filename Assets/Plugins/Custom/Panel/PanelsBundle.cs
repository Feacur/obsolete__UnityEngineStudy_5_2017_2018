using UnityEngine;

///
/// Seeks for nested panels and registers them
///
public class PanelsBundle : MonoBehaviour {
	private void Start() {
		foreach (var panel in GetComponentsInChildren<Panel>(true)) {
			PanelsRegistry.Add(panel);
		}
	}
}