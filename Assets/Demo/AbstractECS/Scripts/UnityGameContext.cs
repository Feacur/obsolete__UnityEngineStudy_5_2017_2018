using ECS;
using GameSystems.Unity;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEventServices;
using ECS.Services;

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
	// Callback from StaticInstance
	//

	protected override void OnInit() {
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

	//
	// Callbacks from Unity
	//

	private void Update() {
		OnUpdate.SafeInvoke(Time.deltaTime);
	}

	private void LateUpdate() {
		OnLateUpdate.SafeInvoke(Time.deltaTime);
	}

	private void FixedUpdate() {
		OnFixedUpdate.SafeInvoke(Time.fixedDeltaTime);
	}
}
