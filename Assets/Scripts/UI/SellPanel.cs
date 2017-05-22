using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

///
/// Sell confirmation representation
///
[RequireComponent(typeof(Panel))]
public class SellPanel : MonoBehaviour {
	public Button confirmButton;
	public Button cancelButton;
	public Text message;

	public UserConfig userConfig { get; private set; }
	public TankConfig tankConfig { get; private set; }

	public void SetUserInfo(UserConfig userConfig) {
		this.userConfig = userConfig;
	}

	public void SetTankInfo(TankConfig tankConfig) {
		this.tankConfig = tankConfig;
		message.text = string.Format("Selling \"{0}\"", tankConfig.name);
	}

	private void OnEnable() {
		confirmButton.onClick.AddListener(Confirm);
		cancelButton.onClick.AddListener(Cancel);
	}
	
	private void OnDisable() {
		confirmButton.onClick.RemoveListener(Confirm);
		cancelButton.onClick.RemoveListener(Cancel);
	}

	private void Confirm() {
		if (tankConfig.currency == CurrencyType.SILVER) {
			userConfig.silver += tankConfig.price;
		}
		else if (tankConfig.currency == CurrencyType.GOLD) {
			userConfig.gold += tankConfig.price;
		}

		userConfig.ownedTanksUids.Remove(tankConfig.uid);
		HangarUI.instance.UpdateUI();
		
		Debug.LogFormat("Saved {0}/{1}", Application.persistentDataPath, HangarUI.instance.userConfigSubPath);
		PersistentData.WriteYaml(HangarUI.instance.userConfigSubPath, userConfig);

		gameObject.SetActive(false);
	}

	private void Cancel() {
		gameObject.SetActive(false);
	}
}