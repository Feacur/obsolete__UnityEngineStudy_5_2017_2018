using UnityEngine;

public static partial class MeshDraw
{
	private static Material material;

	public static Material GetMaterial()
	{
		if (!material) {
			var shader = Shader.Find("Custom/MeshDraw/Color");
			material = new Material(shader);
		}
		return material;
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
