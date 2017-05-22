using UnityEngine;

///
/// Seeks for nested <see cref="Panel"> and registers them into <see cref="PanelsRegistry">
///
public class PanelsBundle : MonoBehaviour {
	private void Start() {
		foreach (var panel in GetComponentsInChildren<Panel>(includeInactive: true)) {
			PanelsRegistry.Add(panel);
		}
	}
}