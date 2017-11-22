using UnityEngine;
using UnityEngine.UI;

///
/// Код для экрана боёвки
/// * Выход на экран миссии
///
[RequireComponent(typeof(Screen))]
public class BattleScreen : MonoBehaviour
{
	public Button exitButton;

	//
	// "Injects"
	//

	private NetworkService NetworkService;
	private ScreensNavigation ScreensNavigation;

	//
	// Callbacks from Unity
	//

	private void Awake() {
		this.NetworkService = NetworkService.instance;
		this.ScreensNavigation = ScreensNavigation.instance;
	}

	private void OnEnable() {
		this.exitButton.onClick.AddListener(RequestExit);
	}

	private void OnDisable() {
		this.exitButton.onClick.RemoveListener(RequestExit);
	}

	//
	// Callbacks from UI
	//

	private void RequestExit() {
		NetworkService.SendRequest_ExitBattle();
		
		var loadingScreen = ScreensNavigation.GoTo<LoadingScreen>();
		loadingScreen.LoadTo<MissionScreen>();
	}
}
