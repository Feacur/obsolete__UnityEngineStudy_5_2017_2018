using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

///
/// Custom containers of components in a map
/// verdict: cumbersome maintenance of a custom container 
///
public class Processor_2 : MonoBehaviour {
	public Dictionary<Type, IArrayOfStructs> components; 

	private void Awake() {
		const int dataSize = 10;
		components = new Dictionary<Type, IArrayOfStructs>() {
			{typeof(Position),     new ArrayOfStructs<Position>(dataSize)},
			{typeof(Velocity),     new ArrayOfStructs<Velocity>(dataSize)},
			{typeof(Acceleration), new ArrayOfStructs<Acceleration>(dataSize)},
		};

		Init();
		
		Debug.Log("Processor_2");
		Print();
		float delta = 1.0f;
		Test_Velocity(delta);
		Test_Acceleration(delta);
		Print();
	}
	
	private void Print() {
		var positions     = components[typeof(Position)]     as ArrayOfStructs<Position>;
		var velocities    = components[typeof(Velocity)]     as ArrayOfStructs<Velocity>;
		var accelerations = components[typeof(Acceleration)] as ArrayOfStructs<Acceleration>;
		
		Debug.LogFormat(
			"Positions: {0}", positions.data.Aggregate("", (acc, val) => acc + val.value + ", ")
		);
		
		Debug.LogFormat(
			"Velocities: {0}", velocities.data.Aggregate("", (acc, val) => acc + val.value + ", ")
		);
		
		Debug.LogFormat(
			"Accelerations: {0}", accelerations.data.Aggregate("", (acc, val) => acc + val.value + ", ")
		);
	}

	private void Init() {
		var positions     = components[typeof(Position)]     as ArrayOfStructs<Position>;
		var velocities    = components[typeof(Velocity)]     as ArrayOfStructs<Velocity>;
		var accelerations = components[typeof(Acceleration)] as ArrayOfStructs<Acceleration>;

		for (int i = 0; i < positions.data.Length; i++) {
			positions.data[i].value = Random.insideUnitCircle;
		}
		
		for (int i = 0; i < velocities.data.Length; i++) {
			velocities.data[i].value = Random.insideUnitCircle;
		}
		
		for (int i = 0; i < accelerations.data.Length; i++) {
			accelerations.data[i].value = Random.insideUnitCircle;
		}
	}

	public void Test_Velocity(float delta) {
		var positions  = components[typeof(Position)] as ArrayOfStructs<Position>;
		var velocities = components[typeof(Velocity)] as ArrayOfStructs<Velocity>;

		for (int i = 0; i < positions.data.Length; i++) {
			positions.data[i].value =
				positions.data[i].value + velocities.data[i].value * delta;
		}
	}
	
	public void Test_Acceleration(float delta) {
		var velocities    = components[typeof(Velocity)]     as ArrayOfStructs<Velocity>;
		var accelerations = components[typeof(Acceleration)] as ArrayOfStructs<Acceleration>;
		
		for (int i = 0; i < velocities.data.Length; i++) {
			velocities.data[i].value =
				velocities.data[i].value + accelerations.data[i].value * delta;
		}
	}
}