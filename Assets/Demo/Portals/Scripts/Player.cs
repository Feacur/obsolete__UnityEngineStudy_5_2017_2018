using UnityEngine;

public class Player : MonoBehaviour
{
	public float positionScale = 1;
	public float rotationScale = 1;

	private Vector3 mousePositionPrevious;

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
		var inputPosition = new Vector3(
			GetKey(KeyCode.D) - GetKey(KeyCode.A),
			GetKey(KeyCode.E) - GetKey(KeyCode.Q),
			GetKey(KeyCode.W) - GetKey(KeyCode.S)
		);

		var inputRotation = (Input.GetMouseButton(0) || Input.GetMouseButton(1))
			? (Input.mousePosition - mousePositionPrevious)
			: Vector3.zero;
		mousePositionPrevious = Input.mousePosition;

		transform.eulerAngles += new Vector3(
			-inputRotation.y,
			inputRotation.x,
			0
		) * rotationScale * Time.deltaTime;

		transform.position += (
			transform.rotation * inputPosition
		) * positionScale * Time.deltaTime;
	}

	private static float GetKey(KeyCode key) {
		return Input.GetKey(key) ? 1 : 0;
	}
}
