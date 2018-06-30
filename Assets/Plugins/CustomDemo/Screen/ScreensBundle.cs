using UnityEngine;

///
/// Seeks for nested <see cref="Screen"> and registers them into <see cref="ScreensRegistry">
///
public class ScreensBundle : MonoBehaviour
{
	private ScreensRegistry ScreensRegistry;
	private void Awake() {
		this.ScreensRegistry = ScreensRegistry.instance;
		
		foreach (var screen in GetComponentsInChildren<Screen>(includeInactive: true)) {
			ScreensRegistry.Add(screen);
		}
	}
}
