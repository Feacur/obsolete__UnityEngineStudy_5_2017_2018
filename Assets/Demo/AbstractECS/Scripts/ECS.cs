using System;

namespace ECS {
	public interface IEntity {
		T GetComponent<T>() where T : class, IComponent;
		IComponent GetComponent(Type type);
		IComponent[] GetComponents();
	}

	public interface IComponent {

	}

	public interface ISystem {
		Type[] GetRequiredComponentTypes();
	}
}

namespace ECS.Callbacks {
	public interface IRemovable {
		Action callback { get; set; }
	}
}

namespace ECS.Injects {
	public interface ISystemInject {
		void SetEntitiesRoot(ECS.Services.IEntitiesRoot entitiesRoot);
	}
}

namespace ECS.Services {
	public interface IEntitiesRoot {
		void AddEntity(IEntity entity);
		void RemoveEntity(IEntity entity);
		IEntity[] GetEntities(ISystem system);
	}
	
	public interface IComponentsRoot {
		void AddComponent(IEntity entity, IComponent component);
		void RemoveComponent(IEntity entity, IComponent component);
	}

	public interface ISystemsRoot {
		void AddSystem(ISystem system);
	}
}
