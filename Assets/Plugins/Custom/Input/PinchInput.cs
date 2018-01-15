using System;
using UnityEngine;
using UnityEngine.Events;

///
/// Abstracts pinch input from platform
///
/// Pinch emulation for mouse is not ideal, of course
///
/// Subscribe to any of these events to get updates:
/// <see cref="onStart"> is called when touched
/// You can subscribe to <see cref="onMove"> and <see cref="onEnd"> at this point if did not so before
///
/// <see cref="onMove"> is called when there is a movement
///
/// <see cref="onEnd"> is called when released
/// You can unsubscribe from <see cref="onMove"> and <see cref="onEnd"> at this point
///
public class PinchInput : AutoInstanceMonoBehaviour<PinchInput>
{
	private const int REQUIRED_TOUCHES = 2;
	private const int MOUSE_BUTTON = 1;
	
	public EventData eventData = new EventData();

	public EventDataEvent onStart = new EventDataEvent();
	public EventDataEvent onMove = new EventDataEvent();
	public EventDataEvent onEnd = new EventDataEvent();

	public float mouseScrollIdleTimeThreshold = 0.1f;
	public float mousePinchMagnitudeMin = 10;
	public float mousePinchMagnitudeBase = 100;
	public float mousePinchMagnitudeMax = 200;
	public float mouseScrollToPinch = 10;

	private bool activeState;
	private int previousTouchesCount;

	private float mouseScrollIdleTime;
	private float mousePinchMagnitude;

	private void Update() {
		#if UNITY_EDITOR
		if (UnityEditor.EditorApplication.isRemoteConnected) {
			UpdateWithTouches();
		}
		else {
			UpdateWithMouse();
		}
		#elif UNITY_ANDROID || UNITY_IOS
		UpdateWithTouches();
		#elif UNITY_STANDALONE || UNITY_WEBGL
		UpdateWithMouse();
		#endif
	}

	private void UpdateWithMouse() {
		int touchCount = Input.GetMouseButton(MOUSE_BUTTON) ? REQUIRED_TOUCHES : 0;
		if (touchCount == 0) {
			if ((Input.mouseScrollDelta.y < -Mathf.Epsilon) || (Mathf.Epsilon < Input.mouseScrollDelta.y)) {
				mouseScrollIdleTime = 0;
			}
			else {
				mouseScrollIdleTime = Mathf.Min(mouseScrollIdleTime + Time.unscaledDeltaTime, mouseScrollIdleTimeThreshold);
			}
			touchCount = (mouseScrollIdleTime < mouseScrollIdleTimeThreshold) ? REQUIRED_TOUCHES : 0;
		}

		if (touchCount != REQUIRED_TOUCHES) {
			mousePinchMagnitude = 0;
			AbstractUpdate(touchCount, Input.mousePosition, eventData.previousPinchMagnitude);
		}
		else {
			mousePinchMagnitude = Mathf.Clamp(
				mousePinchMagnitude + Input.mouseScrollDelta.y * mouseScrollToPinch,
				mousePinchMagnitudeMin - mousePinchMagnitudeBase,
				mousePinchMagnitudeMax - mousePinchMagnitudeBase
			);
			AbstractUpdate(
				touchCount,
				Input.mousePosition,
				mousePinchMagnitudeBase + mousePinchMagnitude
			);
		}
	}

	private void UpdateWithTouches() {
		if (Input.touchCount != REQUIRED_TOUCHES) {
			AbstractUpdate(Input.touchCount, eventData.previousPosition, eventData.previousPinchMagnitude);
		}
		else {
			var touch1 = Input.touches[0];
			var touch2 = Input.touches[1];
			AbstractUpdate(
				Input.touchCount,
				(touch1.position + touch2.position) / 2,
				Vector2.Distance(touch1.position, touch2.position)
			);
		}
	}

	private void AbstractUpdate(int touchesCount, Vector2 currentPosition, float currentPinchMagnitude) {
		eventData.currentPosition = currentPosition;
		eventData.currentPinchMagnitude = currentPinchMagnitude;
		eventData.currentTime = Time.realtimeSinceStartup;
		
		bool canBeActivated = (touchesCount == REQUIRED_TOUCHES) && (previousTouchesCount < REQUIRED_TOUCHES);
		if (!activeState && canBeActivated) {
			activeState = true;
			eventData.previousPinchMagnitude = eventData.currentPinchMagnitude;
			eventData.startPosition = eventData.currentPosition;
			eventData.startPinchMagnitude = eventData.currentPinchMagnitude;
			eventData.startTime = eventData.currentTime;
			onStart.Invoke(eventData);
		}

		bool positionChanged = (eventData.DeltaPosition.sqrMagnitude > Mathf.Epsilon);
		bool pinchChanged = (eventData.DeltaPinchMagnitude < -Mathf.Epsilon) || (Mathf.Epsilon < eventData.DeltaPinchMagnitude);
		if (activeState && (positionChanged || pinchChanged)) {
			onMove.Invoke(eventData);
		}

		bool shouldBeDeactivated = (touchesCount == 0);
		if (activeState && shouldBeDeactivated) {
			activeState = false;
			onEnd.Invoke(eventData);
		}

		eventData.previousPosition = eventData.currentPosition;
		eventData.previousPinchMagnitude = eventData.currentPinchMagnitude;
		eventData.previousTime = eventData.currentTime;
		previousTouchesCount = touchesCount;
	}

	//
	//
	//
	
	[Serializable]
	public class EventData {
		// Position data
		public Vector2 startPosition;
		public Vector2 previousPosition;
		public Vector2 currentPosition;
		// Pinch data
		public float startPinchMagnitude;
		public float previousPinchMagnitude;
		public float currentPinchMagnitude;
		// Time data
		public float startTime;
		public float previousTime;		
		public float currentTime;

		public Vector2 DeltaPosition {
			get { return currentPosition - previousPosition; }
		}

		public Vector2 TotalDeltaPosition {
			get { return currentPosition - startPosition; }
		}

		public float DeltaPinchMagnitude {
			get { return currentPinchMagnitude - previousPinchMagnitude; }
		}

		public float TotalDeltaPinchMagnitude {
			get { return currentPinchMagnitude - startPinchMagnitude; }
		}

		public float DeltaTime {
			get { return currentTime - previousTime; }
		}

		public float TotalDeltaTime {
			get { return currentTime - startTime; }
		}
	}

	[Serializable]
	public class EventDataEvent : UnityEvent<EventData> { }
}
