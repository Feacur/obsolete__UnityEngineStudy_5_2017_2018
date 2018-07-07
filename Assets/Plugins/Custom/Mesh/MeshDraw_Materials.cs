using UnityEngine;

public static partial class MeshDraw
{
	private static Material material;
	private static Material materialN;

	public static Material GetMaterial(bool normals)
	{		
		if (normals) {
			if (!materialN) {
				var shader = Shader.Find("Custom/MeshDraw/Color");
				materialN = new Material(shader);
				materialN.EnableKeyword("NORMALS_ON");
			}
			return materialN;
		}
		else {
			if (!material) {
				var shader = Shader.Find("Custom/MeshDraw/Color");
				material = new Material(shader);
				material.DisableKeyword("NORMALS_ON");
			}
			return material;
		}
	}
	
	private static MaterialPropertyBlock materialPropertyBlock;

	public static MaterialPropertyBlock GetMaterialPropertyBlock()
	{
		if (materialPropertyBlock == null) {
			materialPropertyBlock = new MaterialPropertyBlock();
		}
		return materialPropertyBlock;
	}
}
