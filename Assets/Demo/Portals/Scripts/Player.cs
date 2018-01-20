using UnityEngine;

///
/// Basically is a simple free-flight camera
///
public class Player : MonoBehaviour
{
	public float positionScale = 1;
	public float rotationScale = 1;


	private void Awake() {
		mousePositionPrevious = Input.mousePosition;
	}

	private void OnApplicationFocus(bool hasFocus) {
		mousePositionPrevious = Input.mousePosition;
	}

	private void OnApplicationPause(bool isPause) {
		mousePositionPrevious = Input.mousePosition;
	}

	private void Update () {
		var inputPosition = GetKeysVector() * positionScale * Time.deltaTime;
		var inputRotation = GetMouseDelta() * rotationScale * Time.deltaTime;

		transform.rotation = transform.rotation
			* Quaternion.AngleAxis(-inputRotation.y, Vector3.right)
			* Quaternion.AngleAxis(inputRotation.x, Vector3.up);

		transform.position = transform.position
			+ transform.rotation * inputPosition;
	}

	private static Vector3 GetKeysVector() {
		return new Vector3(
			GetKey(KeyCode.D) - GetKey(KeyCode.A),
			GetKey(KeyCode.E) - GetKey(KeyCode.Q),
			GetKey(KeyCode.W) - GetKey(KeyCode.S)
		);
	}

	private static Vector3 mousePositionPrevious;
	private static Vector3 GetMouseDelta() {
		var result = (Input.GetMouseButton(0) || Input.GetMouseButton(1))
			? (Input.mousePosition - mousePositionPrevious)
			: Vector3.zero;
		mousePositionPrevious = Input.mousePosition;
		return result;
	}

	private static float GetKey(KeyCode key) {
		return Input.GetKey(key) ? 1 : 0;
	}
}
