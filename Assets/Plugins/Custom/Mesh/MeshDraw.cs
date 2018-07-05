using UnityEngine;
using UnityEngine.Rendering;

public static partial class MeshDraw
{
	public static void DrawMesh(MeshType type, MeshMode mode, Vector3 position, Quaternion rotation, Color color, int[] ints)
	{
		var mesh = MeshCache.Mesh(type, mode, ints);
		
		var material = GetMaterial(mode);

		var materialProperties = GetMaterialPropertyBlock();
		materialProperties.SetColor("_Color", color);

		Graphics.DrawMesh(
			mesh: mesh, position: position, rotation: rotation,
			material: material, layer: 0, camera: null,
			submeshIndex: 0, properties: materialProperties,
			castShadows: ShadowCastingMode.Off, receiveShadows: false,
			probeAnchor: null, useLightProbes: false
		);
	}
}
