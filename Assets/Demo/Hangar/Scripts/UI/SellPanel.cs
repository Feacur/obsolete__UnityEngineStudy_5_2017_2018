using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Demo.Hangar {
	///
	/// Sell confirmation representation
	///
	[RequireComponent(typeof(Panel))]
	public class SellPanel : MonoBehaviour
	{
		public Button confirmButton;
		public Button cancelButton;
		public Text message;

		private UserConfig userConfig;
		private TankConfig tankConfig;

		private HangarConfigProvider HangarConfigProvider;
		private void Awake() {
			this.HangarConfigProvider = HangarConfigProvider.instance;
		}
		
		public void Open(UserConfig userConfig, TankConfig tankConfig) {
			this.userConfig = userConfig;
			this.tankConfig = tankConfig;

			message.text = $"Selling \"{tankConfig.name}\"";

			UpdateButtons();

			gameObject.SetActive(true);
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
			Sell(userConfig, tankConfig);
			UpdateButtons();
			gameObject.SetActive(false);
		}

		private void Cancel() {
			gameObject.SetActive(false);
		}

		private void UpdateButtons() {
			bool owned = userConfig.HasTank(tankConfig.uid);

			confirmButton.interactable = owned;
		}

		private void Sell(UserConfig userConfig, TankConfig tankConfig) {
			if (tankConfig.currency == CurrencyType.SILVER) {
				userConfig.silver += tankConfig.price;
			}
			else if (tankConfig.currency == CurrencyType.GOLD) {
				userConfig.gold += tankConfig.price;
			}

			userConfig.ownedTanksUids.Remove(tankConfig.uid);
			HangarConfigProvider.SetUser(userConfig);
		}
	}
}
