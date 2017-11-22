using ECS;
using ECS.Injects;
using ECS.Services;
using GameComponents;
using System;
using UnityEventServices;

namespace GameSystems.Unity {
	public class VelocitySystem : ISystem
		, ISystemInject
	{
		public VelocitySystem(IUpdateService updateService) {
			updateService.onUpdate += Update;
		}

		//
		// ISystem
		//

		private static readonly Type[] requiredComponentTypes = new Type[] {
			typeof(IPositionComponent),
			typeof(IVelocityComponent),
		};

		Type[] ISystem.GetRequiredComponentTypes() {
			return requiredComponentTypes;
		}

		private IEntitiesRoot entitiesRoot;
		void ISystemInject.SetEntitiesRoot(IEntitiesRoot entitiesRoot) {
			this.entitiesRoot = entitiesRoot;
		}

		//
		// internal
		//
		
		private void Update(float delta) {
			var entities = entitiesRoot.GetEntities(system: this);
			foreach (var entity in entities) {
				var position = entity.GetComponent<IPositionComponent>();
				var velocity = entity.GetComponent<IVelocityComponent>();

				position.position += velocity.velocity * delta;
			}
		}
	}
}
