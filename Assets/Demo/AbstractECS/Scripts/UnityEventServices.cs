using System;
using UnityEngine.Events;

namespace UnityEventServices {
	public interface IUpdateService {
		event Action<float> onUpdate;
	}

	public interface ILateUpdateService {
		event Action<float> onLateUpdate;
	}

	public interface IFixedUpdateService {
		event Action<float> onFixedUpdate;
	}
}
