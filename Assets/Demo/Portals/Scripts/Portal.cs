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
	public Transform transformPortalThis;
	public Transform transformPortalAnother;
	public Transform transformPlayer;

	//
	// Callbacks from Unity
	//

	private void Awake() {
		SetupRendering();
	}

	private void LateUpdate() {
		UpdateTransforms();
	}

	private void OnTriggerEnter(Collider other) {
		if (other.name != "Player") { return; }
		float directionProjection = Vector3.Dot(transform.forward, other.transform.position - transform.position);
		if (directionProjection > 0) { return; }

		other.transform.rotation = GetRotation(
			transformPortalAnother.rotation, transformPortalThis.rotation, other.transform.rotation
		);

		other.transform.position = GetPosition(
			transformPortalAnother.position, transformPortalThis.position, other.transform.position
		);
	}

	private void OnTriggerExit(Collider other) {
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

	private void UpdateTransforms() {
		transformCameraThis.localRotation = GetRotation(
			transformPortalThis.localRotation, transformPortalAnother.rotation, transformPlayer.rotation
		);

		transformCameraThis.localPosition = GetPosition(
			transformPortalThis.localPosition, transformPortalAnother.position, transformPlayer.position
		);
	}

	private static Vector3 GetPosition(Vector3 target, Vector3 relative, Vector3 p) {
		return new Vector3(
			target.x + (relative.x - p.x),
			target.y - (relative.y - p.y),
			target.z + (relative.z - p.z)
		);
	}

	private static readonly Quaternion invertY = Quaternion.AngleAxis(180, Vector3.up);
	private static Quaternion GetRotation(Quaternion target, Quaternion relative, Quaternion p) {
		return target * invertY * Quaternion.Inverse(relative) * p;
	}
}
