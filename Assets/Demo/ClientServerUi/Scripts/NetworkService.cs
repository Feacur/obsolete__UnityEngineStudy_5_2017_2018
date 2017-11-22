using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

///
/// Код для имитации взаимодествия клиента и сервера
/// Запросы реализованы на Coroutine для имитации задержек запросов и ответов
///
/// Запросы представляют из себя парные функции <see cref="SendRequest">
/// 1) Код сервера, который был бы исполнен при запросе клиента
/// 2) Код клиента, который был бы вызван при ответе сервера
///
/// Данные клиента и сервера разделены друг от друга, но используют единую структуру
///
/// Что ещё можно сделать:
/// * передача данных серверу
/// * идентификатор запроса
/// * таймаут запроса
///
public class NetworkService : StaticInstanceMonoBehaviour<NetworkService>
{
	//
	// "Injects"
	//

	private ServerData ServerData;
	private ClientData ClientData;

	//
	// Callbacks from StaticInstanceMonoBehaviour
	//

	protected override void OnInit() {
		this.ServerData = ServerData.instance;
		this.ClientData = ClientData.instance;
	}

	//
	// Requests
	//

	public void SendRequest_GoToBattle(Action<bool> callback) {
		// check for instant rejection
		if (ClientData.UserEnergy_Current < ClientData.missionRequirements.energy) {
			callback(false);
			return;
		}
		
		// change user energy client side
		// later server will get us know actual changes
		ClientData.UserEnergy_Current -= ClientData.missionRequirements.energy;
		
		SendRequest(() => { // server code
			if (ServerData.UserEnergy_Current >= ServerData.missionRequirements.energy) {
				ServerData.UserEnergy_Current -= ServerData.missionRequirements.energy;
				return new ResponseData_GoToBattle(true, ServerData.userEnergy.Clone());
			}
			return new ResponseData_GoToBattle(false);
		}, (resultValue) => { // client callback
			callback(resultValue.allowed);
			if (resultValue.userEnergy != null) {
				ClientData.SetUserEnergy(resultValue.userEnergy);
			}
		});
	}
	
	public void SendRequest_ExitBattle() {
	}

	public void SendRequest_GetMissionRequirements() {
		SendRequest(() => { // server code
			return ServerData.missionRequirements.Clone();
		}, (resultValue) => { // client callback
			ClientData.SetMissionRequirements(resultValue);
		});
	}

	public void SendRequest_GetUserEnergy() {
		SendRequest(() => { // server code
			return ServerData.userEnergy.Clone();
		}, (resultValue) => { // client callback
			ClientData.SetUserEnergy(resultValue);
		});
	}

	public void SendRequest_GetServerTime() {
		float requestTime = Time.realtimeSinceStartup;
		SendRequest(() => { // server code
			return ServerData.timestamp.Current;
		}, (resultValue) => { // client callback
			float responseTime = Time.realtimeSinceStartup;
			float pingCorrectionSeconds = (responseTime - requestTime) / 2;
			long pingCorrection = (long)(pingCorrectionSeconds * 1000);
			ClientData.SetTimestamp(resultValue + pingCorrection);
		});
	}

	//
	// SendRequest
	//
	private Coroutine SendRequest<T>(Func<T> serverCode, Action<T> clientCallback) {
		return StartCoroutine(SendRequestCoroutine(serverCode, clientCallback));
	}

	private IEnumerator SendRequestCoroutine<T>(Func<T> serverCode, Action<T> clientCallback) {
		yield return PingCoroutine();
		var resultValue = serverCode();

		yield return PingCoroutine();
		if (clientCallback != null) {
			clientCallback(resultValue);
		}
	}

	private IEnumerator PingCoroutine() {
		const float pingMin = 30 / 1000.0f;
		const float pingMax = 200 / 1000.0f;
		
		yield return new WaitForSecondsRealtime(
			Random.Range(pingMin, pingMax)
		);
	}

	//
	// Local structures
	//

	private struct ResponseData_GoToBattle {
		public bool allowed;
		public EnergySharedComponent userEnergy;

		public ResponseData_GoToBattle(bool allowed, EnergySharedComponent userEnergy = null) {
			this.allowed = allowed;
			this.userEnergy = userEnergy;
		}
	}
}
