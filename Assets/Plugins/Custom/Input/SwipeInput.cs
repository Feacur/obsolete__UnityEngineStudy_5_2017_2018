using System;
using UnityEngine;
using UnityEngine.Events;

///
/// Abstracts swipe input from platform
///
/// Subscribe to this event to get updates:
/// <see cref="onSwipe"> is called when moved and released
///
public class SwipeInput : AutoInstanceMonoBehaviour<SwipeInput> {
	private const int REQUIRED_TOUCHES = 1;
	
	[Serializable]
	public class EventData {
		public Vector2 startPosition;
		public Vector2 previousPosition;
		public Vector2 currentPosition;
		public float startTime;
		public float currentTime;

		public Vector2 DeltaPosition {
			get { return currentPosition - previousPosition; }
		}

		public Vector2 TotalDeltaPosition {
			get { return currentPosition - startPosition; }
		}

		public float Duration {
			get { return currentTime - startTime; }
		}
	}
	
	public EventData eventData = new EventData();

	public SwipedEvent onSwipe = new SwipedEvent();

	private bool canBeActivated;
	private bool activeState;
	private int previousTouchesCount;

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
		AbstractUpdate(
			Input.GetMouseButton(0) ? REQUIRED_TOUCHES : 0,
			Input.mousePosition
		);
	}

	private void UpdateWithTouches() {
		if (Input.touchCount == 0) {
			AbstractUpdate(0, eventData.previousPosition);
		}
		else {
			var touch = Input.touches[0];
			AbstractUpdate(Input.touchCount, touch.position);
		}
	}

	private void AbstractUpdate(int touchesCount, Vector2 currentPosition) {
		canBeActivated = (touchesCount == REQUIRED_TOUCHES) && (canBeActivated || (previousTouchesCount < REQUIRED_TOUCHES));

		eventData.currentPosition = currentPosition;
		eventData.currentTime = Time.realtimeSinceStartup;
			
		if (!activeState && canBeActivated) {
			activeState = true;
			eventData.startPosition = currentPosition;
			eventData.startTime = Time.realtimeSinceStartup;
		}

		bool positionChanged = (eventData.TotalDeltaPosition.sqrMagnitude > Mathf.Epsilon);
		bool shouldBeDeactivated = (touchesCount == 0) && positionChanged;
		if (activeState && shouldBeDeactivated) {
			activeState = false;
			onSwipe.Invoke(eventData);
		}

		eventData.previousPosition = currentPosition;
		previousTouchesCount = touchesCount;
	}

	[Serializable]
	public class SwipedEvent : UnityEvent<EventData> { }
}