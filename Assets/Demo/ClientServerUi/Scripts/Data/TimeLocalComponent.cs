using UnityEngine;

///
/// Данные по времени сервера и клиента
/// Локальный для сервера и клиента компонент
///
[System.Serializable]
public class TimeLocalComponent {
	public long lastChangeTimestamp;
	public float localSyncronizationTime;
	
	public long Current {
		get {
			float secondsOffset = (Time.realtimeSinceStartup - this.localSyncronizationTime);
			long timestampOffset = (long)(secondsOffset * 1000);
			return this.lastChangeTimestamp + timestampOffset; 
		}
		set {
			this.lastChangeTimestamp = value;
			this.localSyncronizationTime = Time.realtimeSinceStartup;
		}
	}
}
