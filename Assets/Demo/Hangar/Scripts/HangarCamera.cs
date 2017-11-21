using UnityEngine;
using UnityEngine.EventSystems;

namespace Demo.Hangar {
	public class HangarCamera : MonoBehaviour
	{
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
		
		private DragInput DragInput;
		private void Awake() {
			this.DragInput = DragInput.instance;
		}

		private void OnEnable() {
			DragInput.onStart.AddListener(OnDragStart);
		}

		private void OnDisable() {
			if (!DragInput.destroyed) {
				DragInput.onStart.RemoveListener(OnDragStart);
			}
			UnsubscribeFromDragUpdates();
		}

		private void OnDragStart(DragInput.EventData eventData) {
			if (!EventSystem.current.IsPointerOverGameObject()) {
				SubscribeToDragUpdates();
			}
		}
		
		private void OnDragMove(DragInput.EventData eventData) {
			Vector2 currentEulerAngles = rotationTransform.localEulerAngles;
		
			Vector2 deltaEulerAngles = Vector2.Scale(eventData.DeltaPosition, rotationSpeed);	
			currentEulerAngles.x = Mathf.Clamp(currentEulerAngles.x - deltaEulerAngles.y, minAngleAxisX, maxAngleAxisX);
			currentEulerAngles.y = currentEulerAngles.y + deltaEulerAngles.x;
			rotationTransform.localRotation = Quaternion.Euler(currentEulerAngles);

			Vector3 currentLocalPosition = positionTransform.localPosition;
			float linearFraction = (currentEulerAngles.x - minAngleAxisX) / (maxAngleAxisX - minAngleAxisX);
			float fraction = distanceInterpolationCurve.Evaluate(linearFraction);
			currentLocalPosition.z = Mathf.Lerp(distanceAtMinAngleAxisX, distanceAtMaxAngleAxisX, fraction);
			positionTransform.localPosition = currentLocalPosition;
		}

		private void OnDragEnd(DragInput.EventData eventData) {
			UnsubscribeFromDragUpdates();
		}

		private void SubscribeToDragUpdates() {
			DragInput.onMove.AddListener(OnDragMove);
			DragInput.onEnd.AddListener(OnDragEnd);
		}

		private void UnsubscribeFromDragUpdates() {
			if (!DragInput.destroyed) {
				DragInput.onMove.RemoveListener(OnDragMove);
				DragInput.onEnd.RemoveListener(OnDragEnd);
			}
		}
	}
}
