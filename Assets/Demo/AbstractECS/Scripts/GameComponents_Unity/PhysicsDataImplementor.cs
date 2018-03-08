using System;
using ECS.Callbacks;
using UnityEngine;

namespace GameComponents.Unity {
	[DisallowMultipleComponent]
	public class PhysicsDataImplementor : MonoBehaviour
		, IVelocityComponent, IAccelerationComponent
		, IRemovable
	{
		public Vector3 Velocity;
		public Vector3 Acceleration;

		//
		// Components
		//

		Vector3 IVelocityComponent.velocity {
			get { return Velocity; }
			set { Velocity = value; }
		}

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
			((IRemovable)this).callback?.Invoke();
		}
	}
}
