using System;
using UnityEngine;
using UnityEngine.Events;

///
/// Abstracts drag input from platform
///
/// Subscribe to any of these events to get updates:
/// <see cref="onStart"> is called when input is valid and drag actualy starts
/// <see cref="onMove"> is called when there is a movement; immediately follows <see cref="onStart">
/// <see cref="onEnd"> is called when input doesn't qualify conditions
///
public class DragInput : AutoInstanceMonoBehaviour<DragInput> {
	public StartedEvent onStart = new StartedEvent();
	public MovedEvent onMove = new MovedEvent();
	public EndedEvent onEnd = new EndedEvent();

	private bool activeState;
	private Vector2 startPosition;
	private Vector2 previousPosition;

	private void Update() {
		#if UNITY_EDITOR
		if (UnityEditor.EditorApplication.isRemoteConnected)
		{
			UpdateWithTouches();
		}
		else
		{
			UpdateWithMouse();
		}
		#elif UNITY_ANDROID || UNITY_IOS
		UpdateWithTouches();
		#elif UNITY_STANDALONE || UNITY_WEBGL
		UpdateWithMouse();
		#endif
	}

	private void UpdateWithMouse() {
		AbstractUpdate(
			Input.GetMouseButton(0) ? 1 : 0,
			Input.mousePosition,
			(Vector2)Input.mousePosition - previousPosition
		);
	}

	private void UpdateWithTouches() {
		if (Input.touchCount == 0) {
			AbstractUpdate(0, previousPosition, Vector2.zero);
		}
		else {
			var touch = Input.touches[0];
			AbstractUpdate(Input.touchCount, touch.position, touch.deltaPosition);
		}
	}

	private void AbstractUpdate(int touchesCount, Vector2 currentPosition, Vector2 deltaPosition) {
		if (deltaPosition.sqrMagnitude >= 0.01f) {
			if (!activeState && (touchesCount == 1)) {
				activeState = true;
				startPosition = currentPosition - deltaPosition;
				onStart.Invoke(currentPosition, deltaPosition);
			}

			if (activeState) {
				onMove.Invoke(currentPosition, deltaPosition, currentPosition - startPosition);
			}
		}

		if (activeState && (touchesCount == 0)) {
			activeState = false;
			onEnd.Invoke(currentPosition, currentPosition - startPosition);
		}
		
		previousPosition = currentPosition;
	}

	///
	/// OnStart(Vector2 currentPosition, Vector2 deltaPosition) { ... }
	///
	[Serializable]
	public class StartedEvent : UnityEvent<Vector2, Vector2> { }
	
	///
	/// OnMove(Vector2 currentPosition, Vector2 deltaPosition, Vector2 totalDeltaPosition) { ... }
	///
	[Serializable]
	public class MovedEvent : UnityEvent<Vector2, Vector2, Vector2> { }
	
	///
	/// OnEnd(Vector2 currentPosition, Vector2 totalDeltaPosition) { ... }
	///
	[Serializable]
	public class EndedEvent : UnityEvent<Vector2, Vector2> { }
}