using UnityEditor;

public static class HangarBuilder {
	private static readonly string subprojectSubPath = "Demo/Hangar";

	[MenuItem ("Custom/Hangar/Prepare streaming assets")]
	public static void PrepareStreamingAssets () {
		SubprojectBuilder.PrepareStreamingAssets(subprojectSubPath);
	}
}
