using System;
using ECS.Callbacks;
using UnityEngine;

namespace GameComponents.Unity {
	[DisallowMultipleComponent]
	public class AccelerationComponent : MonoBehaviour
		, IAccelerationComponent
		, IRemovable
	{
		public Vector3 Acceleration;

		//
		// IAccelerationComponent
		//

		Vector3 IAccelerationComponent.acceleration {
			get { return Acceleration; }
			set { Acceleration = value; }
		}

		//
		// IRemovable
		//

		Action IRemovable.callback { get; set; }

		//
		// Callbacks from Unity
		//

		private UnityGameContext UnityGameContext;
		private void Awake() {
			this.UnityGameContext = UnityGameContext.instance;
			UnityGameContext.AddComponent(
				base.GetComponent<ECS.IEntity>(), component: this
			);
		}

		private void OnDestroy() {
			((IRemovable)this).callback.SafeInvoke();
		}
	}
}
