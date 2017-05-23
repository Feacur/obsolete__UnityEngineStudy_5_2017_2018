using System;
using UnityEngine.Events;

///
/// Data storage for hangar
///
/// It is a part of an experiment with these results:
/// * UI scripts are less coupled
/// * Code became more readable
/// * Code didn't become shorter at all, sigh
///
public class HangarDataProvider : AutoInstanceMonoBehaviour<HangarDataProvider> {
	public UserConfig user { get; private set; }
	public TankConfig selectedTank { get; private set; }

	public UserConfigEvent onUserChanged = new UserConfigEvent();
	public TankConfigEvent onTankSelected = new TankConfigEvent();

	public static void SetUser(UserConfig userConfig) {
		instance.user = userConfig;
		instance.onUserChanged.Invoke(userConfig);
	}

	public static void SetSelectedTank(TankConfig tankConfig) {
		if (instance.selectedTank != tankConfig) {
			instance.selectedTank = tankConfig;
			instance.onTankSelected.Invoke(tankConfig);
		}
	}

	[Serializable]
	public class UserConfigEvent : UnityEvent<UserConfig> { }
	
	[Serializable]
	public class TankConfigEvent : UnityEvent<TankConfig> { }
}