using System;
using UnityEngine;
using UnityEngine.Events;

///
/// Abstracts tap input from platform
///
/// Subscribe to any of these events to get updates:
/// <see cref="onStart"> is called when touched
/// <see cref="onEnd"> is called when released
///
public class TapInput : AutoInstanceMonoBehaviour<TapInput> {
	private const int REQUIRED_TOUCHES = 1;
	
	[Serializable]
	public class EventData {
		public Vector2 startPosition;
		public Vector2 previousPosition;
		public Vector2 currentPosition;

		public Vector2 DeltaPosition {
			get { return currentPosition - previousPosition; }
		}

		public Vector2 TotalDeltaPosition {
			get { return currentPosition - startPosition; }
		}
	}
	
	public EventData eventData = new EventData();

	public StartedEvent onStart = new StartedEvent();
	public EndedEvent onEnd = new EndedEvent();

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
		
		if (!activeState && canBeActivated) {
			activeState = true;
			eventData.startPosition = currentPosition;
			onStart.Invoke(eventData);
		}

		bool shouldBeDeactivated = (touchesCount == 0);
		if (activeState && shouldBeDeactivated) {
			activeState = false;
			onEnd.Invoke(eventData);
		}

		eventData.previousPosition = currentPosition;
		previousTouchesCount = touchesCount;
	}

	[Serializable]
	public class StartedEvent : UnityEvent<EventData> { }

	[Serializable]
	public class EndedEvent : UnityEvent<EventData> { }
}