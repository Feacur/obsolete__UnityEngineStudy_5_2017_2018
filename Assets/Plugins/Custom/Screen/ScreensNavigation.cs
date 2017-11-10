using UnityEngine;

public class ScreensNavigation : AutoInstanceMonoBehaviour<ScreensNavigation> {
	public GameObject current;

	public T GoTo<T>() where T : MonoBehaviour {
		var screensRegistry = ScreensRegistry.instance;

		if (current) {
			current.SetActive(false);
		}

		T currentSpecific = screensRegistry.Get<T>();
		current = currentSpecific.gameObject;
		
		if (current) {
			current.SetActive(true);
		}

		return currentSpecific;
	}
}