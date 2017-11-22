using System.Collections;
using System.Threading;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

public class DoTask_Coroutine: MonoBehaviour {
	private void OnEnable() {
		Debug.LogWarning(
@"DoTask_Coroutine: no freezing, but long
This will take about 16 seconds for 60 FPS settings
This will take 1000 frames to finish"
		);
		Debug.Log("Before starting DoTask coroutine");
		DoTask();
		Debug.Log("After starting DoTask coroutine");
	}

	private Coroutine DoTask() {
		return StartCoroutine(DoTaskCoroutine());
	}

	private IEnumerator DoTaskCoroutine() {
		var sw = Stopwatch.StartNew();
		Debug.Log("Started DoTask");
		for (int i = 0; i < 1000; i++) {
			Thread.Sleep(2);
			yield return null;
		}
		Debug.LogFormat("Finished DoTask in {0} ms", sw.Elapsed.TotalMilliseconds);
	}
}