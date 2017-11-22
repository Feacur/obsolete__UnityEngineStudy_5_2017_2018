using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

///
/// Raw arrays of components as fields
/// verdict: rigid implementation
///
public class Processor_1 : MonoBehaviour {
	public Position[]     positions;
	public Velocity[]     velocities;
	public Acceleration[] accelerations;

	private void Awake() {
		const int dataSize = 10;
		positions     = new Position[dataSize];
		velocities    = new Velocity[dataSize];
		accelerations = new Acceleration[dataSize];

		Init();

		Debug.Log("Processor_1");
		Print();
		float delta = 1.0f;
		Test_Velocity(delta);
		Test_Acceleration(delta);
		Print();
	}
	
	private void Print() {
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
		for (int i = 0; i < positions.Length; i++) {
			positions[i].value += velocities[i].value * delta;
		}
	}

	public void Test_Acceleration(float delta) {
		for (int i = 0; i < velocities.Length; i++) {
			velocities[i].value += accelerations[i].value * delta;
		}
	}
}