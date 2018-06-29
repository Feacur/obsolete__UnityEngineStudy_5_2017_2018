using System.Threading;
using UnityEngine;
using Stopwatch = System.Diagnostics.Stopwatch;

public class DoTask_Thread: MonoBehaviour {
	private void OnEnable() {
		Debug.LogWarning(
@"DoTask_Thread: no freezing, and fast
This will take about 2 seconds
This will take some frames to finish"
		);
		Debug.Log("Before starting a thread with DoTask");
		var thread = new Thread(DoTask);
		thread.Start();
		Debug.Log("After starting a thread with DoTask");
	}

	private void DoTask() {
		var sw = Stopwatch.StartNew();
		Debug.Log("Started DoTask");
		for (int i = 0; i < 1000; i++) {
			Thread.Sleep(2);
		}
		Debug.Log($"Finished DoTask in {sw.Elapsed.TotalMilliseconds} ms");
	}
}