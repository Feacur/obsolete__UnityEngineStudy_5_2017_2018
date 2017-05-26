using System;
using UnityEngine;
using UnityEngine.Events;

///
/// Abstracts pinch input from platform
///
/// Subscribe to any of these events to get updates:
/// <see cref="onStart"> is called when input is valid and drag actually starts
/// <see cref="onMove"> is called when there is a movement; immediately follows <see cref="onStart">
/// <see cref="onEnd"> is called when input doesn't qualify conditions
///
public class PinchInput : AutoInstanceMonoBehaviour<PinchInput> {
	private const int REQUIRED_TOUCHES = 2;
	
	[Serializable]
	public class EventData {
		public Vector2 startPosition;
		public Vector2 previousPosition;
		public Vector2 currentPosition;
		public float startPinchMagnitude;
		public float previousPinchMagnitude;
		public float currentPinchMagnitude;

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
	}
	
	public EventData eventData = new EventData();

	public StartedEvent onStart = new StartedEvent();
	public MovedEvent onMove = new MovedEvent();
	public EndedEvent onEnd = new EndedEvent();

	public float mouseScrollIdleTimeThreshold = 0.1f;
	public float mousePinchMagnitudeMin = 10;
	public float mousePinchMagnitudeBase = 100;
	public float mousePinchMagnitudeMax = 200;
	public float mouseScrollToPinch = 10;

	private bool canBeActivated;
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
		int touchCount = Input.GetMouseButton(1) ? REQUIRED_TOUCHES : 0;
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
			AbstractUpdate(0, eventData.previousPosition, eventData.previousPinchMagnitude);
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
			AbstractUpdate(0, eventData.previousPosition, eventData.previousPinchMagnitude);
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
		canBeActivated = (touchesCount == REQUIRED_TOUCHES) && (canBeActivated || (previousTouchesCount < REQUIRED_TOUCHES));
		
		eventData.currentPosition = currentPosition;
		eventData.currentPinchMagnitude = currentPinchMagnitude;

		bool positionChanged = (eventData.DeltaPosition.sqrMagnitude > Mathf.Epsilon);
		bool pinchChanged = (eventData.DeltaPinchMagnitude < -Mathf.Epsilon) || (Mathf.Epsilon < eventData.DeltaPinchMagnitude);
		if (positionChanged || pinchChanged) {
			if (!activeState && canBeActivated) {
				activeState = true;
				eventData.startPosition = eventData.previousPosition;
				eventData.previousPinchMagnitude = currentPinchMagnitude;
				eventData.startPinchMagnitude = eventData.previousPinchMagnitude;
				onStart.Invoke(eventData);
			}

			if (activeState) {
				onMove.Invoke(eventData);
			}
		}

		bool shouldBeDeactivated = (touchesCount != REQUIRED_TOUCHES);
		if (activeState && shouldBeDeactivated) {
			activeState = false;
			onEnd.Invoke(eventData);
		}

		eventData.previousPosition = currentPosition;
		eventData.previousPinchMagnitude = currentPinchMagnitude;
		previousTouchesCount = touchesCount;
	}

	[Serializable]
	public class StartedEvent : UnityEvent<EventData> { }

	[Serializable]
	public class MovedEvent : UnityEvent<EventData> { }

	[Serializable]
	public class EndedEvent : UnityEvent<EventData> { }
}