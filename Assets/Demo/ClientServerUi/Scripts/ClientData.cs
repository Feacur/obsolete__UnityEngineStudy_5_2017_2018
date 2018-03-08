using UnityEngine.Events;

///
/// Набор клиентских данных
/// Объект с данными будет создан автоматически
///
/// Что ещё можно сделать:
/// * проверять время отправки данных при обновлении
///
public partial class ClientData : AutoInstanceMonoBehaviour<ClientData>
{
	public TimeLocalComponent timestamp { get; } = new TimeLocalComponent();
	public RequirementsSharedComponent missionRequirements { get; private set; }
	public EnergySharedComponent userEnergy { get; private set; }

	public UnityEvent onTimestampChanged = new UnityEvent();
	public UnityEvent onMissionRequirementsChanged = new UnityEvent();
	public UnityEvent onUserEnergyChanged = new UnityEvent();

	//
	// Data setters
	//

	public void SetTimestamp(long value) {
		this.timestamp.Current = value;
		onTimestampChanged.Invoke();
	}
	
	public void SetMissionRequirements(RequirementsSharedComponent value) {
		this.missionRequirements = value;
		onMissionRequirementsChanged.Invoke();
	}
	
	public void SetUserEnergy(EnergySharedComponent value) {
		this.userEnergy = value;
		onUserEnergyChanged.Invoke();
	}
}
