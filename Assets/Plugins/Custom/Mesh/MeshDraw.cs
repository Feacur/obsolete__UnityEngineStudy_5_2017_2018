using UnityEngine;
using UnityEngine.Rendering;

public static partial class MeshDraw
{
	public static void DrawBox(MeshMode mode, Vector3 position, Quaternion rotation, Color color)
	{
		var mesh = MeshCache.Box(mode);
		
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
