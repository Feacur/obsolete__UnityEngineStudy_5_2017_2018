using UnityEngine;
using UnityEngine.EventSystems;

public class HangarCamera : MonoBehaviour {
	[Header("Controlled objects")]
	public new Camera camera;
	public Transform rotationTransform;
	public Transform positionTransform;
	[Header("Rotation parameters")]
	public Vector2 rotationSpeed = Vector2.one;
	[Range(0, 90)]
	public float minAngleAxisX = 10;
	[Range(0, 90)]
	public float maxAngleAxisX = 90;
	[Header("Distance parameters")]
	public float distanceAtMinAngleAxisX = -13;
	public float distanceAtMaxAngleAxisX = -10;
	public AnimationCurve distanceInterpolationCurve = AnimationCurve.Linear(0, 0, 1, 1);
	
	private bool canMove;

	private void OnEnable() {
		DragInput.instance.onStart.AddListener(OnStart);
		DragInput.instance.onMove.AddListener(OnMove);
	}

	private void OnDisable() {
		if (!DragInput.destroyed) {
			DragInput.instance.onStart.AddListener(OnStart);
			DragInput.instance.onMove.RemoveListener(OnMove);
		}
	}

	private void OnStart(Vector2 currentPosition, Vector2 deltaPosition) {
		canMove = !EventSystem.current.IsPointerOverGameObject();
	}
	
	private void OnMove(Vector2 currentPosition, Vector2 deltaPosition, Vector2 totalDeltaPosition) {
		if (!canMove) { return; }

		Vector2 currentEulerAngles = rotationTransform.localEulerAngles;
	
		Vector2 deltaEulerAngles = Vector2.Scale(deltaPosition, rotationSpeed);	
		currentEulerAngles.x = Mathf.Clamp(currentEulerAngles.x - deltaEulerAngles.y, minAngleAxisX, maxAngleAxisX);
		currentEulerAngles.y = currentEulerAngles.y + deltaEulerAngles.x;
		rotationTransform.localRotation = Quaternion.Euler(currentEulerAngles);

		Vector3 currentLocalPosition = positionTransform.localPosition;
		float linearFraction = (currentEulerAngles.x - minAngleAxisX) / (maxAngleAxisX - minAngleAxisX);
		float fraction = distanceInterpolationCurve.Evaluate(linearFraction);
		currentLocalPosition.z = Mathf.Lerp(distanceAtMinAngleAxisX, distanceAtMaxAngleAxisX, fraction);
		positionTransform.localPosition = currentLocalPosition;
	}
}
