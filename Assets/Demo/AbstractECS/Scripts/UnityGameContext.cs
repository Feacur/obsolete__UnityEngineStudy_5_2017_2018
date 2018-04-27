using ECS;
using GameSystems.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEventServices;
using ECS.Services;
using Custom.Singleton;

[DisallowMultipleComponent]
public class UnityGameContext : StaticInstanceMonoBehaviour<UnityGameContext>
	, IUpdateService, ILateUpdateService, IFixedUpdateService
{
	private ISystemsRoot systemsRoot;
	private IComponentsRoot componentsRoot;
	private IEntitiesRoot entitiesRoot;
	
	//
	// API
	//

	public void AddEntity(IEntity entity) {
		this.entitiesRoot.AddEntity(entity);
	}

	public void AddComponent(IEntity entity, IComponent component) {
		this.componentsRoot.AddComponent(entity, component);
	}

	//
	// IUpdateListener
	//

	private event Action<float> OnUpdate;

	event Action<float> IUpdateService.onUpdate {
		add {
			OnUpdate += value;
		}
		remove {
			OnUpdate -= value;
		}
	}

	//
	// ILateUpdateListener
	//
	
	private event Action<float> OnLateUpdate;

	event Action<float> ILateUpdateService.onLateUpdate {
		add {
			OnLateUpdate += value;
		}
		remove {
			OnLateUpdate -= value;
		}
	}

	//
	// IFixedUpdateListener
	//

	private event Action<float> OnFixedUpdate;

	event Action<float> IFixedUpdateService.onFixedUpdate {
		add {
			OnFixedUpdate += value;
		}
		remove {
			OnFixedUpdate -= value;
		}
	}

	//
	// Callbacks from Unity
	//

	new protected void Awake() {
		base.Awake();
		var engine     = new ECS.EntityEngine();
		systemsRoot    = engine;
		componentsRoot = engine;
		entitiesRoot   = engine;
		
		IUpdateService      updateService      = this;
		ILateUpdateService  lateUpdateService  = this;
		IFixedUpdateService fixedUpdateService = this;

		systemsRoot.AddSystem(new VelocitySystem(updateService));
		systemsRoot.AddSystem(new AccelerationSystem(updateService));
		systemsRoot.AddSystem(new PersonalBoundsSystem(updateService));
	}

	private void Update() {
		OnUpdate?.Invoke(Time.deltaTime);
	}

	private void LateUpdate() {
		OnLateUpdate?.Invoke(Time.deltaTime);
	}

	private void FixedUpdate() {
		OnFixedUpdate?.Invoke(Time.fixedDeltaTime);
	}
}
