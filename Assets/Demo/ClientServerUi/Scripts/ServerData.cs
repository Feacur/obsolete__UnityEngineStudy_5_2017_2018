///
/// Набор серверных данных
/// Объект с данными должен находиться в сцене
///
public partial class ServerData : StaticInstanceMonoBehaviour<ServerData>
{
	public TimeLocalComponent timestamp;
	public RequirementsSharedComponent missionRequirements;
	public EnergySharedComponent userEnergy;

	//
	// "Injects"
	//

	private ClientData ClientData;

	//
	// Callbacks from Unity
	//

	new protected void Awake() {
		base.Awake();
		this.ClientData = ClientData.instance;
	}
}
