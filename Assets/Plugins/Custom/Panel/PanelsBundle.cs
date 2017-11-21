using UnityEngine;

///
/// Seeks for nested <see cref="Panel"> and registers them into <see cref="PanelsRegistry">
///
public class PanelsBundle : MonoBehaviour
{
	private PanelsRegistry PanelsRegistry;
	private void Awake() {
		this.PanelsRegistry = PanelsRegistry.instance;
		
		foreach (var panel in GetComponentsInChildren<Panel>(includeInactive: true)) {
			PanelsRegistry.Add(panel);
		}
	}
}
