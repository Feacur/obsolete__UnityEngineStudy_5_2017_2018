using UnityEngine;

public static partial class MeshDraw
{
	private static Material materialSolid;
	private static Material materialWireframe;
	private static Material materialNormals;

	public static Material GetMaterial(MeshMode mode)
	{
		switch (mode) {
		case MeshMode.Solid:
			if (!materialSolid) {
				var shader = Shader.Find("Custom/MeshDraw/Color");
				materialSolid = new Material(shader);
				materialSolid.DisableKeyword("NORMALS_ON");
			}
			return materialSolid;
		case MeshMode.Wireframe:
			if (!materialWireframe) {
				var shader = Shader.Find("Custom/MeshDraw/Color");
				materialWireframe = new Material(shader);
				materialWireframe.DisableKeyword("NORMALS_ON");
			}
			return materialWireframe;
		case MeshMode.Normals:
			if (!materialNormals) {
				var shader = Shader.Find("Custom/MeshDraw/Color");
				materialNormals = new Material(shader);
				materialNormals.EnableKeyword("NORMALS_ON");
			}
			return materialNormals;
		}
		return null;
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
