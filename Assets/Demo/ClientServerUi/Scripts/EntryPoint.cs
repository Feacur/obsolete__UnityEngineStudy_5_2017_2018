using UnityEngine;

///
/// Импровизированная точка входа для активации интерфейса
///
public class EntryPoint : MonoBehaviour
{
	//
	// "Injects"
	//

	private ScreensNavigation ScreensNavigation;

	//
	// Callbacks from Unity
	//

	private void Awake() {
		this.ScreensNavigation = ScreensNavigation.instance;
	}

	private void Start () {
		var loadingScreen = ScreensNavigation.GoTo<LoadingScreen>();
		loadingScreen.LoadTo<MissionScreen>();
	}
}
