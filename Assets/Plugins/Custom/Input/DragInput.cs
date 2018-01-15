using System;
using UnityEngine;
using UnityEngine.Events;

///
/// Abstracts drag input from platform
///
/// Subscribe to any of these events to get updates:
/// <see cref="onStart"> is called when touched
/// You can subscribe to <see cref="onMove"> and <see cref="onEnd"> at this point if did not so before
///
/// <see cref="onMove"> is called when there is a movement
///
/// <see cref="onEnd"> is called when released
/// You can unsubscribe from <see cref="onMove"> and <see cref="onEnd"> at this point
/// If you need a swipe gesture use this event and check any relevent position and time changes
///
public class DragInput : AutoInstanceMonoBehaviour<DragInput>
{
	private const int REQUIRED_TOUCHES = 1;
	private const int MOUSE_BUTTON = 0;
	
	public EventData eventData = new EventData();

	public EventDataEvent onStart = new EventDataEvent();
	public EventDataEvent onMove = new EventDataEvent();
	public EventDataEvent onEnd = new EventDataEvent();

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
		int touchCount = Input.GetMouseButton(MOUSE_BUTTON) ? REQUIRED_TOUCHES : 0;
		AbstractUpdate(touchCount, Input.mousePosition);
	}

	private void UpdateWithTouches() {
		if (Input.touchCount != REQUIRED_TOUCHES) {
			AbstractUpdate(Input.touchCount, eventData.previousPosition);
		}
		else {
			var touch = Input.touches[0];
			AbstractUpdate(Input.touchCount, touch.position);
		}
	}

	private void AbstractUpdate(int touchesCount, Vector2 currentPosition) {
		eventData.currentPosition = currentPosition;
		eventData.currentTime = Time.realtimeSinceStartup;

		bool canBeActivated = (touchesCount == REQUIRED_TOUCHES) && (previousTouchesCount < REQUIRED_TOUCHES);
		if (!activeState && canBeActivated) {
			activeState = true;
			eventData.startPosition = eventData.currentPosition;
			eventData.startTime = eventData.currentTime;
			onStart.Invoke(eventData);
		}

		bool positionChanged = (eventData.DeltaPosition.sqrMagnitude > Mathf.Epsilon);
		if (activeState && positionChanged) {
			onMove.Invoke(eventData);
		}

		bool shouldBeDeactivated = (touchesCount == 0);
		if (activeState && shouldBeDeactivated) {
			activeState = false;
			onEnd.Invoke(eventData);
		}

		eventData.previousPosition = eventData.currentPosition;
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
