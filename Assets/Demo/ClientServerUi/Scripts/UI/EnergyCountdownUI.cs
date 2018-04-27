using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Custom.Utils;

///
/// Код для таймера недостатка энергии
/// * Включение и выключение кнопки перехода в бой
/// * Обновление текста времени до восстановления 
/// * Анимация восстановления достаточной энергии
///
public class EnergyCountdownUI : MonoBehaviour
{
	public Selectable battleButton;
	public Slider energyCountdownSlider;
	public Text energyCountdownText;

	//
	// "Injects"
	//

	private NetworkService NetworkService;
	private ClientData ClientData;

	//
	// Callbacks from Unity
	//

	private void Awake() {
		this.NetworkService = NetworkService.instance;
		this.ClientData = ClientData.instance;
	}

	private void OnEnable() {
		this.battleButton.interactable = false;
		this.energyCountdownSlider.gameObject.SetActive(false);

		ClientData.onTimestampChanged.AddListener(UpdateUI);
		ClientData.onMissionRequirementsChanged.AddListener(UpdateUI);
		ClientData.onUserEnergyChanged.AddListener(UpdateUI);
		UpdateUI();

		NetworkService.SendRequest_GetMissionRequirements();
		NetworkService.SendRequest_GetUserEnergy();
	}

	private void OnDisable() {
		this.updateEnergyCountdownCoroutine = null;

		if (!ClientData.destroyed) {
			ClientData.onTimestampChanged.RemoveListener(UpdateUI);
			ClientData.onMissionRequirementsChanged.RemoveListener(UpdateUI);
			ClientData.onUserEnergyChanged.RemoveListener(UpdateUI);
		}
	}

	//
	// Data responses from server
	//

	private void UpdateUI() {
		UpdateEnergyCountdown();
	}

	//
	// UpdateEnergyCountdown
	//

	private bool UpdateEnergyCountdown_Instant() {
		if (ClientData.timestamp == null) { return false; }
		if (ClientData.userEnergy == null) { return false; }
		if (ClientData.missionRequirements == null) { return false; }

		bool shouldStop = ClientData.UserEnergy_IsSufficient;
		this.battleButton.interactable = shouldStop;
		this.energyCountdownSlider.gameObject.SetActive(!shouldStop);
		this.energyCountdownSlider.value = ClientData.UserEnergy_CountdownFraction;
		this.energyCountdownText.text = TimeFormatter.SecondsToFormat(
			Mathf.RoundToInt(ClientData.UserEnergy_CountdownSeconds)
		);

		return !shouldStop;
	}

	private Coroutine updateEnergyCountdownCoroutine;
	private Coroutine UpdateEnergyCountdown() {
		if ((this.updateEnergyCountdownCoroutine == null) && UpdateEnergyCountdown_Instant()) {
			this.updateEnergyCountdownCoroutine = StartCoroutine(UpdateEnergyCountdownCoroutine());
		}
		return this.updateEnergyCountdownCoroutine;
	}

	private IEnumerator UpdateEnergyCountdownCoroutine() {
		do {
			yield return null;
		} while (UpdateEnergyCountdown_Instant());
		this.updateEnergyCountdownCoroutine = null;
	}
}
