///
/// Данные для настройки миссии
/// Пересылаемый с сервера на клиент компонент
///
[System.Serializable]
public class RequirementsSharedComponent {
	public int energy;
}

///
/// Методы для работы с <see cref="RequirementsSharedComponent">
///
public static class RequirementsComponent_Extension {
	public static RequirementsSharedComponent Clone(this RequirementsSharedComponent component) {
		return new RequirementsSharedComponent() {
			energy = component.energy
		};
	}
}
