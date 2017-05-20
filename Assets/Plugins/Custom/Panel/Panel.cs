using UnityEngine;

///
/// Marks game object so as to allow <see cref="PanelsBundle"> find it
///
public sealed class Panel : MonoBehaviour {
	private void OnDestroy() {
		PanelsRegistry.Remove(gameObject);
	}
}