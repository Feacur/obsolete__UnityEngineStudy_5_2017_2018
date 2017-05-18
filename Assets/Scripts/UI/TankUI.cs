using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

///
/// Tank UI representation
///
public class TankUI : MonoBehaviour, IPointerClickHandler {
	public Text caption;

	private TankConfig _tankConfig;
	public TankConfig tankConfig {
		get {
			return _tankConfig;
		}
		set {
			_tankConfig = value;
			UpdateUI();
		}
	}

	void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
	{
		Hangar.instance.CreateTank(tankConfig);
	}

	private void UpdateUI() {
		name = string.Format("Tank UI: ", tankConfig.name);
		caption.text = tankConfig.name;
	}
}