using System;
using ECS.Callbacks;
using UnityEngine;

namespace GameComponents.Unity {
	[DisallowMultipleComponent]
	public class BouncyObjectImplementor : MonoBehaviour
		, IPositionComponent, IRotationComponent
		, IVelocityComponent, IAccelerationComponent
		, IPersonalBoundsComponent
		, IRemovable
	{
		public Vector3 Velocity;
		public Vector3 Acceleration;
		public Bounds Bounds;

		//
		// Components
		//

		Vector3 IPositionComponent.position {
			get { return transform.position; }
			set { transform.position = value; }
		}
		
		Quaternion IRotationComponent.rotation {
			get { return transform.rotation; }
			set { transform.rotation = value; }
		}

		Vector3 IVelocityComponent.velocity {
			get { return Velocity; }
			set { Velocity = value; }
		}

		Vector3 IAccelerationComponent.acceleration {
			get { return Acceleration; }
			set { Acceleration = value; }
		}
		
		Bounds IPersonalBoundsComponent.bounds {
			get { return Bounds; }
			set { Bounds = value; }
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
