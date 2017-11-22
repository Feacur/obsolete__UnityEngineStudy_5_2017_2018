using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

///
/// Raw arrays of components in a map
/// verdict: so far, so good, apart resizing
///
public class Processor_4 : MonoBehaviour {
	public Dictionary<Type, IList> components; 

	private void Awake() {
		const int dataSize = 10;
		components = new Dictionary<Type, IList>() {
			{typeof(Position),     new Position[dataSize]},
			{typeof(Velocity),     new Velocity[dataSize]},
			{typeof(Acceleration), new Acceleration[dataSize]},
		};

		Init();
		
		Debug.Log("Processor_4");
		Print();
		float delta = 1.0f;
		Test_Velocity(delta);
		Test_Acceleration(delta);
		Print();
	}
	
	private void Print() {
		var positions     = components[typeof(Position)]     as Position[];
		var velocities    = components[typeof(Velocity)]     as Velocity[];
		var accelerations = components[typeof(Acceleration)] as Acceleration[];
		
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
		var positions     = components[typeof(Position)]     as Position[];
		var velocities    = components[typeof(Velocity)]     as Velocity[];
		var accelerations = components[typeof(Acceleration)] as Acceleration[];

		for (int i = 0; i < positions.Length; i++) {
			positions[i].value = Random.insideUnitCircle;
		}
		
		for (int i = 0; i < velocities.Length; i++) {
			velocities[i].value = Random.insideUnitCircle;
		}
		
		for (int i = 0; i < accelerations.Length; i++) {
			accelerations[i].value = Random.insideUnitCircle;
		}
	}

	public void Test_Velocity(float delta) {
		var positions  = components[typeof(Position)] as Position[];
		var velocities = components[typeof(Velocity)] as Velocity[];

		for (int i = 0; i < positions.Length; i++) {
			positions[i].value =
				positions[i].value + velocities[i].value * delta;
		}
	}
	
	public void Test_Acceleration(float delta) {
		var velocities    = components[typeof(Velocity)]     as Velocity[];
		var accelerations = components[typeof(Acceleration)] as Acceleration[];
		
		for (int i = 0; i < velocities.Length; i++) {
			velocities[i].value =
				velocities[i].value + accelerations[i].value * delta;
		}
	}
}