using UnityEngine;

public static partial class MeshDraw
{
	public static void DrawBox(MeshMode mode, Vector3 position, Quaternion rotation, Color color)
	{
		var mesh = MeshCache.Box(mode);
		
		var material = GetMaterial();

		var materialProperties = GetMaterialPropertyBlock();
		materialProperties.SetColor("_Color", color);

		Graphics.DrawMesh(
			mesh, position, rotation,
			material, 0, null,
			0, materialProperties,
			false, false, false
		);
	}
}
