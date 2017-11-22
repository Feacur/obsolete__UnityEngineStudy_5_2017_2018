using System;
using ECS.Callbacks;
using UnityEngine;

namespace GameComponents.Unity {
	[DisallowMultipleComponent]
	public class RotationComponent : MonoBehaviour
		, IRotationComponent
		, IRemovable
	{
		//
		// IRotationComponent
		//

		Quaternion IRotationComponent.rotation {
			get { return transform.rotation; }
			set { transform.rotation = value; }
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