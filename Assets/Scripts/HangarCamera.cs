using UnityEngine;

public class HangarCamera : MonoBehaviour {
	public new Camera camera;
	public Transform rotationTransform;
	public Transform positionTransform;

	private void OnEnable() {
		DragInput.instance.onStart.AddListener(OnStart);
		DragInput.instance.onMove.AddListener(OnMove);
		DragInput.instance.onEnd.AddListener(OnEnd);
	}

	private void OnDisable() {
		if (!DragInput.destroyed) {
			DragInput.instance.onStart.RemoveListener(OnStart);
			DragInput.instance.onMove.RemoveListener(OnMove);
			DragInput.instance.onEnd.RemoveListener(OnEnd);
		}
	}
	
	private void OnStart(Vector2 currentPosition, Vector2 deltaPosition) {
		Debug.LogFormat("OnStart({0}, {1})", currentPosition, deltaPosition);
	}

	private void OnMove(Vector2 currentPosition, Vector2 deltaPosition, Vector2 totalDeltaPosition) {
		Debug.LogFormat("OnMove({0}, {1}, {2})", currentPosition, deltaPosition, totalDeltaPosition);
	}
	
	private void OnEnd(Vector2 currentPosition, Vector2 totalDeltaPosition) {
		Debug.LogFormat("OnEnd({0}, {1})", currentPosition, totalDeltaPosition);
	}
}
