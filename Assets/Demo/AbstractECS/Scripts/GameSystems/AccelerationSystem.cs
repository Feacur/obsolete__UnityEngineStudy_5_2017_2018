using ECS;
using ECS.Injects;
using ECS.Services;
using GameComponents;
using System;
using UnityEventServices;

namespace GameSystems.Unity {
	public class AccelerationSystem : ISystem
		, ISystemInject
	{
		public AccelerationSystem(IUpdateService updateService) {
			updateService.onUpdate += Update;
		}

		//
		// ISystem
		//

		private static readonly Type[] requiredComponentTypes = new Type[] {
			typeof(IVelocityComponent),
			typeof(IAccelerationComponent),
		};

		Type[] ISystem.GetRequiredComponentTypes() {
			return requiredComponentTypes;
		}

		//
		// ISystemInject
		//

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
				var velocity = entity.GetComponent<IVelocityComponent>();
				var acceleration = entity.GetComponent<IAccelerationComponent>();

				velocity.velocity += acceleration.acceleration * delta;
			}
		}
	}
}
