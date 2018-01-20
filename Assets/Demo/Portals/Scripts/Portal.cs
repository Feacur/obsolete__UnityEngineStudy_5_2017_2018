using UnityEngine;

///
/// A blob class for now
///
public class Portal : MonoBehaviour
{
	[Header("Rendering")]
	public Camera renderCamera;
	public Renderer renderTarget;

	[Header("Transforms")]
	public Transform transformCameraThis;
	public Transform transformCameraAnother;
	public Transform transformPortalThis;
	public Transform transformPortalAnother;
	public Transform transformPlayer;

	private static readonly Quaternion y180 = Quaternion.AngleAxis(180, Vector3.up);

	//
	// Callbacks from Unity
	//

	private void Awake() {
		SetupRendering();
	}

	private void LateUpdate() {
		UpdateCameraTransform();
		if (touching) {
			Teleport();
		}
	}

	private bool touching;
	private void OnTriggerEnter(Collider other) {
		if (other.name != "Player") { return; }
		touching = true;
	}

	private void OnTriggerExit(Collider other) {
		if (other.name != "Player") { return; }
		touching = false;
	}

	//
	//
	//

	private void SetupRendering() {
		var renderTexture = new RenderTexture(
			UnityEngine.Screen.width, UnityEngine.Screen.height, 24
		);
		renderTexture.Create();

		renderCamera.targetTexture = renderTexture;
		renderTarget.sharedMaterial.mainTexture = renderTexture;
	}

	private void UpdateCameraTransform() {
		var playerLocalPosition = transformPortalAnother.parent.InverseTransformPoint(transformPlayer.position);
		var offsetPosition = playerLocalPosition - transformPortalAnother.localPosition;
		transformCameraThis.localPosition = transformPortalThis.localPosition + y180 * offsetPosition;

		var playerLocalRotation = Quaternion.Inverse(transformPortalAnother.parent.rotation) * transformPlayer.rotation;
		var offsetRotation = playerLocalRotation * Quaternion.Inverse(transformPortalAnother.localRotation);
		transformCameraThis.localRotation = transformPortalThis.localRotation * offsetRotation * y180;
	}

	private void Teleport() {
		float directionProjection = Vector3.Dot(transformPortalThis.forward, transformPlayer.forward);
		if (directionProjection > 0) {
			transformPlayer.position = transformCameraAnother.position;
			transformPlayer.rotation = transformCameraAnother.rotation;
		}
	}
}
