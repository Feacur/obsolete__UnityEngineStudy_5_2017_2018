using System;
using ECS;
using ECS.Callbacks;
using UnityEngine;

namespace ECS.Unity {
	[DisallowMultipleComponent]
	public class UnityEntity : MonoBehaviour
		, IEntity
		, IRemovable
	{
		//
		// IEntity
		//

		T IEntity.GetComponent<T>() {
			return base.GetComponent<T>();
		}

		IComponent IEntity.GetComponent(Type type) {
			return base.GetComponent(type) as IComponent;
		}

		IComponent[] IEntity.GetComponents() {
			return base.GetComponents<IComponent>();
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
		} 

		private void OnEnable() {
			UnityGameContext.AddEntity(
				base.GetComponent<IEntity>()
			);
		}

		private void OnDisable() {
			((IRemovable)this).callback?.Invoke();
		}
	}
}
