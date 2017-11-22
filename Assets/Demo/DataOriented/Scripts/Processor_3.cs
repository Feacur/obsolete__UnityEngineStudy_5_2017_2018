using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

///
/// Lists of components in a map
/// verdict: cumbersome immutability
///
public class Processor_3 : MonoBehaviour {
	public Dictionary<Type, IList> components; 

	private void Awake() {
		const int dataSize = 10;
		components = new Dictionary<Type, IList>() {
			{typeof(Position),     new List<Position>(dataSize)},
			{typeof(Velocity),     new List<Velocity>(dataSize)},
			{typeof(Acceleration), new List<Acceleration>(dataSize)},
		};

		Init();
		
		Debug.Log("Processor_3");
		Print();
		float delta = 1.0f;
		Test_Velocity(delta);
		Test_Acceleration(delta);
		Print();
	}
	
	private void Print() {
		var positions     = components[typeof(Position)]     as List<Position>;
		var velocities    = components[typeof(Velocity)]     as List<Velocity>;
		var accelerations = components[typeof(Acceleration)] as List<Acceleration>;
		
		Debug.LogFormat(
			"Positions: {0}", positions.Aggregate("", (acc, val) => acc + val.value + ", ")
		);
		
		Debug.LogFormat(
			"Velocities: {0}", velocities.Aggregate("", (acc, val) => acc + val.value + ", ")
		);
		
		Debug.LogFormat(
			"Accelerations: {0}", accelerations.Aggregate("", (acc, val) => acc + val.value + ", ")
		);
	}

	private void Init() {
		var positions     = components[typeof(Position)]     as List<Position>;
		var velocities    = components[typeof(Velocity)]     as List<Velocity>;
		var accelerations = components[typeof(Acceleration)] as List<Acceleration>;

		for (int i = 0; i < positions.Capacity; i++) {
			positions.Add(new Position() {
				value = Random.insideUnitCircle
			});
		}
		
		for (int i = 0; i < velocities.Capacity; i++) {
			velocities.Add(new Velocity() {
				value = Random.insideUnitCircle
			});
		}
		
		for (int i = 0; i < accelerations.Capacity; i++) {
			accelerations.Add(new Acceleration() {
				value = Random.insideUnitCircle
			});
		}
	}

	public void Test_Velocity(float delta) {
		var positions  = components[typeof(Position)] as List<Position>;
		var velocities = components[typeof(Velocity)] as List<Velocity>;

		for (int i = 0; i < positions.Count; i++) {
			positions[i] = new Position() {
				value = positions[i].value + velocities[i].value * delta
			};
		}
	}
	
	public void Test_Acceleration(float delta) {
		var velocities    = components[typeof(Velocity)]     as List<Velocity>;
		var accelerations = components[typeof(Acceleration)] as List<Acceleration>;
		
		for (int i = 0; i < velocities.Count; i++) {
			velocities[i] = new Velocity() {
				value = velocities[i].value + accelerations[i].value * delta
			};
		}
	}
}