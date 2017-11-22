using System.Collections;
using UnityEngine;
using UnityEngine.UI;

///
/// Код для счётчика энергии
/// * Обновление текста количества энергии
/// * Анимация восстановления единицы энергии
///
public class EnergyCounterUI : MonoBehaviour
{
	public Text energyCounterText;
	public Slider energyPointSlider;

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
		this.energyPointSlider.gameObject.SetActive(false);

		ClientData.onTimestampChanged.AddListener(UpdateUI);
		ClientData.onUserEnergyChanged.AddListener(UpdateUI);
		UpdateUI();

		NetworkService.SendRequest_GetMissionRequirements();
		NetworkService.SendRequest_GetUserEnergy();
	}

	private void OnDisable() {
		this.updateEnergyCounterCoroutine = null;
		this.updateEnergyNextPointCoroutine = null;
		
		if (!ClientData.destroyed) {
			ClientData.onTimestampChanged.RemoveListener(UpdateUI);
			ClientData.onUserEnergyChanged.RemoveListener(UpdateUI);
		}
	}

	//
	// Data responses from server
	//

	private void UpdateUI() {
		UpdateEnergyCounter();
		UpdateEnergyNextPoint();
	}

	//
	// UpdateEnergyCounter
	//

	private static readonly string ENERGY_FORMAT = "{0} / {1}";
	private bool UpdateEnergyCounter_Instant() {
		if (ClientData.timestamp == null) { return false; }
		if (ClientData.userEnergy == null) { return false; }

		bool shouldStop = ClientData.UserEnergy_IsMaximum;
		this.energyCounterText.text = string.Format(
			ENERGY_FORMAT, ClientData.UserEnergy_Current, ClientData.userEnergy.maximum
		);

		return !shouldStop;
	}

	private Coroutine updateEnergyCounterCoroutine;
	private Coroutine UpdateEnergyCounter() {
		if ((this.updateEnergyCounterCoroutine == null) && UpdateEnergyCounter_Instant()) {
			this.updateEnergyCounterCoroutine = StartCoroutine(UpdateEnergyCounterCoroutine());
		}
		return this.updateEnergyCounterCoroutine;
	}

	private IEnumerator UpdateEnergyCounterCoroutine() {
		long lastEnergy;
		do {
			lastEnergy = ClientData.UserEnergy_Current;
			yield return null;
		} while ((lastEnergy == ClientData.UserEnergy_Current) || UpdateEnergyCounter_Instant());
		this.updateEnergyCounterCoroutine = null;
	}

	//
	// UpdateEnergyNextPoint
	//

	private bool UpdateEnergyNextPoint_Instant() {
		if (ClientData.timestamp == null) { return false; }
		if (ClientData.userEnergy == null) { return false; }

		bool shouldStop = ClientData.UserEnergy_IsMaximum;
		this.energyPointSlider.gameObject.SetActive(!shouldStop);
		this.energyPointSlider.value = ClientData.UserEnergy_NextPointFraction;
		
		return !shouldStop;
	}
	
	private Coroutine updateEnergyNextPointCoroutine;
	private Coroutine UpdateEnergyNextPoint() {
		if ((this.updateEnergyNextPointCoroutine == null) && UpdateEnergyNextPoint_Instant()) {
			this.updateEnergyNextPointCoroutine = StartCoroutine(UpdateEnergyNextPointCoroutine());
		}
		return this.updateEnergyNextPointCoroutine;
	}

	private IEnumerator UpdateEnergyNextPointCoroutine() {
		do {
			yield return null;
		} while (UpdateEnergyNextPoint_Instant());
		this.updateEnergyNextPointCoroutine = null;
	}
}
