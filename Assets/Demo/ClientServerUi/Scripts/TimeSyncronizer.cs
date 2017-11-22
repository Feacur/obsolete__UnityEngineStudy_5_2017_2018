using System.Collections;
using UnityEngine;

///
/// Объект для периодической синхронизации времени с сервером
///
public class TimeSyncronizer : MonoBehaviour
{
	//
	// "Injects"
	//

	private NetworkService NetworkService;

	//
	// Callbacks from Unity
	//

	private void Awake() {
		this.NetworkService = NetworkService.instance;
	}

	private void OnEnable () {
		SynchronizeTime();
	}

	//
	// SynchronizeTime
	//

	private Coroutine SynchronizeTime() {
		return StartCoroutine(SynchronizeTimeCoroutine());
	}

	private IEnumerator SynchronizeTimeCoroutine() {
		while (true) {
			NetworkService.SendRequest_GetServerTime();
			yield return new WaitForSecondsRealtime(30);
		}
	}
}
