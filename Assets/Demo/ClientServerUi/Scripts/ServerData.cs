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
	// Callbacks from StaticInstanceMonoBehaviour
	//

	protected override void OnInit() {
		this.ClientData = ClientData.instance;
	}
}
