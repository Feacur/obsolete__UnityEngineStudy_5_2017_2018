using System;
using ECS.Callbacks;
using UnityEngine;

namespace GameComponents.Unity {
	[DisallowMultipleComponent]
	public class PersonalBoundsComponent : MonoBehaviour
		, IPersonalBoundsComponent
		, IRemovable
	{
		public Bounds Bounds;

		//
		// IPersonalBoundsComponent
		//
		
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
			((IRemovable)this).callback.SafeInvoke();
		}
	}
}
