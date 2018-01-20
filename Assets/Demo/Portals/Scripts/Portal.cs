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
		transformPlayer.SetParent(transformPortalAnother.parent, worldPositionStays: true);
		
		var transformPortalAnotherParentRotation = transformPortalAnother.parent.rotation;
		transformPortalAnother.parent.rotation = y180;
		
		transformCameraThis.localPosition = GetPosition(
			target: transformPortalThis.localPosition,
			relative: transformPortalAnother.position,
			p: transformPlayer.position
		);

		transformCameraThis.localRotation = GetRotation(
			target: transformPortalThis.localRotation * y180,
			relative: transformPortalAnother.rotation,
			p: transformPlayer.rotation
		);
		
		transformPortalAnother.parent.rotation = transformPortalAnotherParentRotation;
		
		transformPlayer.SetParent(null, worldPositionStays: true);
	}

	private void Teleport() {
		float directionProjection = Vector3.Dot(transformPortalThis.forward, transformPlayer.forward);
		if (directionProjection > 0) {
			transformPlayer.position = transformCameraAnother.position;
			transformPlayer.rotation = transformCameraAnother.rotation;
		}
	}

	private static Vector3 GetPosition(Vector3 target, Vector3 relative, Vector3 p) {
		return new Vector3(
			target.x + (p.x - relative.x),
			target.y + (p.y - relative.y),
			target.z + (p.z - relative.z)
		);
	}

	private static Quaternion GetRotation(Quaternion target, Quaternion relative, Quaternion p) {
		return target * (p * Quaternion.Inverse(relative));
	}
}
