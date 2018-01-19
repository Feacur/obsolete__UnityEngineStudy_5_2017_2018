using UnityEngine;

///
/// Данные для настройки пользователя
/// Пересылаемый с сервера на клиент компонент
///
[System.Serializable]
public class EnergySharedComponent {
	public int lastChangeValue;
	public long lastChangeTimestamp;
	public int millisPerPoint;
	public int maximum;
}

///
/// Методы для работы с <see cref="EnergySharedComponent">
///
public static class EnergyComponent_Extension {
	public static int GetCurrentCapped(this EnergySharedComponent component, long timestamp) {
		int elapsedMillis = (int)(timestamp - component.lastChangeTimestamp);
		int offset = elapsedMillis / component.millisPerPoint;
		return Mathf.Clamp(component.lastChangeValue + offset, 0, component.maximum);
	}

	public static float GetCurrentCappedWithFraction(this EnergySharedComponent component, long timestamp) {
		float elapsedMillis = (float)(timestamp - component.lastChangeTimestamp);
		float offset = elapsedMillis / component.millisPerPoint;
		return Mathf.Clamp(component.lastChangeValue + offset, 0, component.maximum);
	}

	public static int GetCurrent(this EnergySharedComponent component, long timestamp) {
		return Mathf.Max(component.lastChangeValue, component.GetCurrentCapped(timestamp));
	}

	public static float GetCurrentWithFraction(this EnergySharedComponent component, long timestamp) {
		return Mathf.Max(component.lastChangeValue, component.GetCurrentCappedWithFraction(timestamp));
	}

	public static void SetCurrent(this EnergySharedComponent component, int value, long timestamp) {
		// set timestamp, but save elapsed fraction
		component.lastChangeTimestamp = (value >= component.maximum)
			? timestamp 
			: timestamp - component.GetFractionalMillis(timestamp);
		// set the value directly
		component.lastChangeValue = Mathf.Max(0, value);
	}

	public static long GetFractionalMillis(this EnergySharedComponent component, long timestamp) {
		long elapsedMillis = (timestamp - component.lastChangeTimestamp);
		return elapsedMillis % component.millisPerPoint;
	}

	public static float GetFractionToNextPoint(this EnergySharedComponent component, long timestamp) {
		return component.GetFractionalMillis(timestamp) / (float)component.millisPerPoint;
	}

	public static EnergySharedComponent Clone(this EnergySharedComponent component) {
		return new EnergySharedComponent() {
			lastChangeValue = component.lastChangeValue,
			lastChangeTimestamp = component.lastChangeTimestamp,
			millisPerPoint = component.millisPerPoint,
			maximum = component.maximum
		};
	}
}
