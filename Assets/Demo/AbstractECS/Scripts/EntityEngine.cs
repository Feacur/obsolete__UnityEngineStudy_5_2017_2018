using ECS.Services;
using System.Collections.Generic;
using System.Linq;
using ECS.Callbacks;
using ECS.Injects;

namespace ECS {
	public class EntityEngine
		: IEntitiesRoot, IComponentsRoot, ISystemsRoot
	{
		private Dictionary<ISystem, HashSet<IEntity>> systems = new Dictionary<ISystem, HashSet<IEntity>>();
		private HashSet<IEntity> entities = new HashSet<IEntity>();

		private HashSet<IRemovable> removableEntities = new HashSet<IRemovable>();
		private HashSet<IRemovable> removableComponents = new HashSet<IRemovable>();

		//
		// Constructor
		//

		private IEntitiesRoot entitiesRoot;
		private IComponentsRoot componentsRoot;
		private ISystemsRoot systemsRoot;
		public EntityEngine() {
			this.entitiesRoot = this;
			this.componentsRoot = this;
			this.systemsRoot = this;
		}

		//
		// ISystemsRoot
		//

		void ISystemsRoot.AddSystem(ISystem system) {
			this.systems.Add(system, new HashSet<IEntity>());
			
			if (system is ISystemInject) {
				var implementor = (ISystemInject)system;
				implementor.SetEntitiesRoot(entitiesRoot: this);
			}
		}

		//
		// IComponentsRoot
		//

		void IComponentsRoot.AddComponent(IEntity entity, IComponent component) {
			bool shouldUpdateCache = this.entities.Contains(entity);
			if (shouldUpdateCache) {
				UpdateCache(entity);
			}

			if (component is IRemovable) {
				var implementor = (IRemovable)component;
				implementor.callback = () => {
					componentsRoot.RemoveComponent(entity, component);
				};
				removableComponents.Add(implementor);
			}
		}

		void IComponentsRoot.RemoveComponent(IEntity entity, IComponent component) {
			bool shouldUpdateCache = this.entities.Contains(entity);
			if (shouldUpdateCache) {
				UpdateCache(entity, removedComponent: component);
			}
			
			if (component is IRemovable) {
				var implementor = (IRemovable)component;
				removableComponents.Remove(implementor);
			}
		}

		//
		// IEntitiesRoot
		//

		void IEntitiesRoot.AddEntity(IEntity entity) {
			if (entity is IRemovable) {
				var implementor = (IRemovable)entity;
				implementor.callback = () => {
					entitiesRoot.RemoveEntity(entity);
				};
				removableEntities.Add(implementor);
			}

			foreach (var component in entity.GetComponents()) {
				componentsRoot.AddComponent(entity, component);
			}

			this.entities.Add(entity);
			UpdateCache(entity);
		}

		void IEntitiesRoot.RemoveEntity(IEntity entity) {
			this.entities.Remove(entity);
			UpdateCache(entity);

			if (entity is IRemovable) {
				var implementor = (IRemovable)entity;
				removableEntities.Remove(implementor);
			}
			
			foreach (var component in entity.GetComponents()) {
				componentsRoot.RemoveComponent(entity, component);
			}
		}

		IEntity[] IEntitiesRoot.GetEntities(ISystem system) {
			return systems[system].ToArray();
			// var requiredComponentTypes = system.GetRequiredComponentTypes();
			// return this.entities.Where(
			// 	entity => requiredComponentTypes.All(
			// 		type => entity.GetComponent(type) != null
			// 	)
			// ).ToArray();
		}

		//
		// caching
		//
		
		private void UpdateCache(IEntity entity, IComponent removedComponent = null) {
			foreach (var system in systems) {
				var requiredComponentTypes = system.Key.GetRequiredComponentTypes();
				bool fits = requiredComponentTypes.All(
					type => {
						var componentInstance = entity.GetComponent(type);
						return (componentInstance != null) && (componentInstance != removedComponent);
					}
				);
				if (fits) {
					system.Value.Add(entity);
				}
				else {
					system.Value.Remove(entity);
				}
			}
		}
	}
}
