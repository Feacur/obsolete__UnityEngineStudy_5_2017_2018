using ECS;
using UnityEngine;


namespace GameComponents {
	public interface IPositionComponent : IComponent {
		Vector3 position { get; set; }
	}

	public interface IVelocityComponent : IComponent {
		Vector3 velocity { get; set; }
	}

	public interface IAccelerationComponent : IComponent {
		Vector3 acceleration { get; set; }
	}

	public interface IRotationComponent : IComponent {
		Quaternion rotation { get; set; }
	}

	public interface IPersonalBoundsComponent : IComponent {
		Bounds bounds { get; set; }
	}
}
