using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Demo.Hangar {
	///
	/// Purchase confirmation representation
	///
	[RequireComponent(typeof(Panel))]
	public class PurchasePanel : MonoBehaviour {
		public Button confirmButton;
		public Button cancelButton;
		public Text message;

		private UserConfig userConfig;
		private TankConfig tankConfig;
		
		public void Open(UserConfig userConfig, TankConfig tankConfig) {
			this.userConfig = userConfig;
			this.tankConfig = tankConfig;

			message.text = string.Format("Purchasing \"{0}\"", tankConfig.name);

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
			Purchase(userConfig, tankConfig);
			UpdateButtons();
			gameObject.SetActive(false);
		}

		private void Cancel() {
			gameObject.SetActive(false);
		}

		private void UpdateButtons() {
			bool owned = userConfig.HasTank(tankConfig.uid);
			bool hasEnoughMoney = false;
			if (tankConfig.currency == CurrencyType.SILVER) {
				hasEnoughMoney = (userConfig.silver >= tankConfig.price);
			}
			else if (tankConfig.currency == CurrencyType.GOLD) {
				hasEnoughMoney = (userConfig.gold >= tankConfig.price);
			}

			confirmButton.interactable = !owned && hasEnoughMoney;
		}

		private static void Purchase(UserConfig userConfig, TankConfig tankConfig) {
			if (tankConfig.currency == CurrencyType.SILVER) {
				userConfig.silver -= tankConfig.price;
			}
			else if (tankConfig.currency == CurrencyType.GOLD) {
				userConfig.gold -= tankConfig.price;
			}

			userConfig.ownedTanksUids.Add(tankConfig.uid);
			HangarConfigProvider.SetUser(userConfig);
		}
	}
}
