using System;
using System.Collections;
using UnityEngine;

///
/// Hangar scene representation
/// Responsible for tank model updates
///
public class Hangar : StaticInstanceMonoBehaviour<Hangar> {
	public Transform tankParentTransform;
	
	private Coroutine createTankCoroutine;

	private void OnEnable() {
		HangarDataProvider.instance.onTankSelected.AddListener(OnTankSelected);
		if (HangarDataProvider.instance.selectedTank != null) {
			OnTankSelected(HangarDataProvider.instance.selectedTank);
		}
	}
	
	private void OnDisable() {
		if (!HangarDataProvider.destroyed) {
			HangarDataProvider.instance.onTankSelected.RemoveListener(OnTankSelected);
		}
		
		if (createTankCoroutine != null) {
			StopCoroutine(createTankCoroutine);
		}
	}
	
	private void OnTankSelected(TankConfig tankConfig) {
		if (createTankCoroutine != null) {
			StopCoroutine(createTankCoroutine);
		}
		createTankCoroutine = StartCoroutine(CreateTankCoroutine(tankConfig));
	}
	
	private IEnumerator CreateTankCoroutine(TankConfig tankConfig) {
		tankParentTransform.DestroyChildren();
		
		// Get tank prefab
		GameObject tankPrefab = null;
		yield return StreamingData.LoadAssetAsync<GameObject>(tankConfig.assetBundle, tankConfig.prefab, (resultValue) => {
			tankPrefab = resultValue;
		});

		// Instantiate prefab
		var instance = Instantiate(tankPrefab);
		instance.transform.SetParent(tankParentTransform, worldPositionStays: false);
		instance.name = string.Format("Tank: {0}", tankConfig.name);
	}
}
