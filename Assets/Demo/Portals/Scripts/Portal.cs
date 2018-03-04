using System.Collections.Generic;
using UnityEngine;

///
/// A blob class for now
///
/// * Create portal texture, which is rendered by this camera for another portal
/// * Align render camera according the way player looks at another portal
/// * Display portal texture with Portal.shader, which uses screen-space coordinates
///
/// Portal textures matche the aspect ratio of the screen, kind of overshoot here
///
/// Player camera sees this portal at the same screen-space coordinates
/// as another portal's camera does, that's why using this special shader
///
public class Portal : MonoBehaviour
{
	[Header("This portal")]
	public Camera renderCamera;
	public Renderer renderTarget;

	[Header("Other objects")]
	public Portal linkedPortal;
	public Transform transformPlayerCamera;

	//
	// Callbacks from Unity
	//

	private void Awake() {
		SetupRendering();
	}

	private void LateUpdate() {
		UpdateCameraTransform();
		for (int i = 0; i < touching.Count; i++) {
			Teleport(touching[i]);
			touching.RemoveAt(i);
			i--;
		}
	}

	private List<Transform> touching = new List<Transform>();
	private void OnTriggerEnter(Collider other) {
		if (other.name != "Player") { return; }
		touching.Add(other.transform);
	}

	private void OnTriggerExit(Collider other) {
		if (other.name != "Player") { return; }
		touching.Remove(other.transform);
	}

	//
	//
	//

	private void SetupRendering() {
		var renderTexture = new RenderTexture(
			UnityEngine.Screen.width, UnityEngine.Screen.height, 24
		);
		renderTexture.Create();

		linkedPortal.renderCamera.targetTexture = renderTexture;
		renderTarget.sharedMaterial.mainTexture = renderTexture;
	}

	private static readonly Quaternion y180 = Quaternion.AngleAxis(180, Vector3.up);
	private void UpdateCameraTransform() {
		var transformCameraThis = renderCamera.transform;
		var transformPortalThis = renderTarget.transform;
		var transformPortalAnother = linkedPortal.renderTarget.transform;

		var playerLocalPosition = transformPortalAnother.parent.InverseTransformPoint(transformPlayerCamera.position);
		var offsetPosition = playerLocalPosition - transformPortalAnother.localPosition;
		transformCameraThis.localPosition = transformPortalThis.localPosition + y180 * offsetPosition;

		var playerLocalRotation = Quaternion.Inverse(transformPortalAnother.parent.rotation) * transformPlayerCamera.rotation;
		var offsetRotation = playerLocalRotation * Quaternion.Inverse(transformPortalAnother.localRotation);
		transformCameraThis.localRotation = transformPortalThis.localRotation * offsetRotation * y180;
	}

	private void Teleport(Transform transformPlayer) {
		var transformCameraAnother = linkedPortal.renderCamera.transform;
		var transformPortalThis = renderTarget.transform;

		float directionProjection = Vector3.Dot(transformPortalThis.forward, transformPlayer.forward);
		if (directionProjection > 0) {
			transformPlayer.position = transformCameraAnother.position;
			transformPlayer.rotation = transformCameraAnother.rotation;
		}
	}
}
