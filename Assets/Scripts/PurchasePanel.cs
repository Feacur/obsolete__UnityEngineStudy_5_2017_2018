using System;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Panel))]
public class PurchasePanel : MonoBehaviour, IPointerClickHandler {
	private UserConfig userConfig;
	private TankConfig tankConfig;

	public void SetUserInfo(UserConfig userConfig) {
		this.userConfig = userConfig;
	}

	public void SetTankInfo(TankConfig tankConfig) {
		this.tankConfig = tankConfig;
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData) {
		gameObject.SetActive(false);
	}
}