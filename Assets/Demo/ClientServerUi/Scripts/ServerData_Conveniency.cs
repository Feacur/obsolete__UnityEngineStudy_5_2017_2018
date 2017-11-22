using UnityEngine;

///
/// Функции для удобного обращения к данным
/// В редакторе изменения будут переданы "клиенту" за счёт <see cref="OnValidate">
///
/// В реальности не будет такой связности:
/// * несколько пользователей
/// * несколько миссий
///
public partial class ServerData
{
	public int UserEnergy_Current {
		get {
			return userEnergy.GetCurrent(timestamp.Current);
		}
		set {
			userEnergy.SetCurrent(value, timestamp.Current);
		}
	}
	
	public bool UserEnergy_IsSufficient {
		get {
			return UserEnergy_Current >= missionRequirements.energy;
		}
	}

	//
	// Hack for editor
	//

	private void OnValidate() {
		// Editor hack to pass manual changes immediately
		if (Application.isPlaying && ClientData) {
			ClientData.SetTimestamp(this.timestamp.Current);
			ClientData.SetMissionRequirements(this.missionRequirements.Clone());
			ClientData.SetUserEnergy(this.userEnergy.Clone());
		}
	}
}
