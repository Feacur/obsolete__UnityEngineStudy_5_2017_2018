using UnityEngine;
using UnityEngine.UI;

///
/// Код для экрана миссии
/// * Переход на экран боёвки
///
/// Что ещё можно сделать:
/// * блокировка кнопки до ответа или таймаута запроса
///
[RequireComponent(typeof(Screen))]
public class MissionScreen : MonoBehaviour
{
	public Button battleButton;

	//
	// "Injects"
	//

	private NetworkService NetworkService;
	private ScreensRegistry ScreensRegistry;
	private ScreensNavigation ScreensNavigation;

	//
	// Callbacks from Unity
	//

	private void Awake() {
		this.NetworkService = NetworkService.instance;
		this.ScreensRegistry = ScreensRegistry.instance;
		this.ScreensNavigation = ScreensNavigation.instance;
	}

	private void OnEnable() {
		this.battleButton.onClick.AddListener(RequestBattle);
	}

	private void OnDisable() {
		this.battleButton.onClick.RemoveListener(RequestBattle);
	}

	//
	// Callbacks from UI
	//

	private void RequestBattle() {
		ScreensNavigation.GoTo<LoadingScreen>();
		NetworkService.SendRequest_GoToBattle(Callback_GoToBattle);
	}

	//
	// Callbacks from server
	//

	private void Callback_GoToBattle(bool allowed) {
		var loadingScreen = ScreensRegistry.Get<LoadingScreen>();
		if (allowed) {
			loadingScreen.LoadTo<BattleScreen>();
		}
		else {
			Debug.LogWarning("Insufficient energy");
			loadingScreen.LoadTo<MissionScreen>();
		}
	}
}
