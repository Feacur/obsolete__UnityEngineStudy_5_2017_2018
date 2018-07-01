using UnityEngine;

public static partial class MeshDraw
{
	public static void DrawBox(Vector3 position, Quaternion rotation, Color color)
	{
		var mesh = MeshCache.BoxSolid();
		
		var material = GetMaterial();
		var materialProperties = GetMaterialPropertyBlock();
		materialProperties.SetColor("_Color", color);

		Graphics.DrawMesh(
			mesh,
			position, rotation,
			material,
			0, null, 0,
			materialProperties,
			false, false, false
		);
	}
}
