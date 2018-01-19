using UnityEngine;

///
/// Функции для удобного обращения к данным
///
/// В реальности не будет такой связности:
/// * несколько пользователей
/// * несколько миссий
///
public partial class ClientData
{
	public int UserEnergy_Current {
		get {
			return userEnergy.GetCurrent(timestamp.Current);
		}
		set {
			userEnergy.SetCurrent(value, timestamp.Current);
			onUserEnergyChanged.Invoke();
		}
	}

	public bool UserEnergy_IsMaximum {
		get {
			return UserEnergy_Current >= userEnergy.maximum;
		}
	}
	
	public bool UserEnergy_IsSufficient {
		get {
			return UserEnergy_Current >= missionRequirements.energy;
		}
	}

	public float UserEnergy_NextPointFraction {
		get {
			return userEnergy.GetFractionToNextPoint(timestamp.Current);
		}
	}

	public float UserEnergy_CountdownFraction {
		get {
			float currentUserEnergy = userEnergy.GetCurrentWithFraction(timestamp.Current);
			return Mathf.Clamp01(currentUserEnergy / missionRequirements.energy);
		}
	}

	public float UserEnergy_CountdownSeconds {
		get {
			float fraction = 1 - UserEnergy_CountdownFraction;
			int countdownMillis = userEnergy.millisPerPoint * missionRequirements.energy;
			return (fraction * countdownMillis / 1000);
		}
	}
}
