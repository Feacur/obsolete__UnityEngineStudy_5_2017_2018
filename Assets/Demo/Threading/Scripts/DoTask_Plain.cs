using System.Threading;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

public class DoTask_Plain: MonoBehaviour {
	private void OnEnable() {
		Debug.LogWarning(
@"DoTask_Plain: freezing, but fast
This will take about 2 seconds
This will take 1 frame to finish"
		);
		Debug.Log("Before calling DoTask");
		DoTask();
		Debug.Log("After calling DoTask");
	}

	private void DoTask() {
		var sw = Stopwatch.StartNew();
		Debug.Log("Started DoTask");
		for (int i = 0; i < 1000; i++) {
			Thread.Sleep(2);
		}
		Debug.LogFormat("Finished DoTask in {0} ms", sw.Elapsed.TotalMilliseconds);
	}
}