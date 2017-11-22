using ECS;
using ECS.Injects;
using ECS.Services;
using GameComponents;
using System;
using UnityEventServices;

namespace GameSystems.Unity {
	public class PersonalBoundsSystem : ISystem
		, ISystemInject
	{
		public PersonalBoundsSystem(IUpdateService updateService) {
			updateService.onUpdate += Update;
		}

		//
		// ISystem
		//

		private static readonly Type[] requiredComponentTypes = new Type[] {
			typeof(IPositionComponent),
			typeof(IVelocityComponent),
			typeof(IPersonalBoundsComponent),
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
				var position = entity.GetComponent<IPositionComponent>();
				var velocity = entity.GetComponent<IVelocityComponent>();
				var bounds = entity.GetComponent<IPersonalBoundsComponent>();

				var positionValue = position.position;
				var velocityValue = velocity.velocity;
				var boundsMin = bounds.bounds.min;
				var boundsMax = bounds.bounds.max;
				
				if (positionValue.x < boundsMin.x) {
					velocityValue.x = Math.Abs(velocityValue.x);
				}
				else if (positionValue.x > boundsMax.x) {
					velocityValue.x = -Math.Abs(velocityValue.x);
				}
				
				if (positionValue.y < boundsMin.y) {
					velocityValue.y = Math.Abs(velocityValue.y);
				}
				else if (positionValue.y > boundsMax.y) {
					velocityValue.y = -Math.Abs(velocityValue.y);
				}
				
				if (positionValue.z < boundsMin.z) {
					velocityValue.z = Math.Abs(velocityValue.z);
				}
				else if (positionValue.z > boundsMax.z) {
					velocityValue.z = -Math.Abs(velocityValue.z);
				}

				velocity.velocity = velocityValue;
			}
		}
	}
}
