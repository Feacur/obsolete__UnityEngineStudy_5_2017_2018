using UnityEngine;

public static partial class MeshDraw
{
	private static Material materialWireframe;
	private static Material materialDense;
	private static Material materialSparse;

	public static Material GetMaterial(MeshMode mode)
	{
		switch (mode) {
		case MeshMode.Wireframe:
			if (!materialWireframe) {
				var shader = Shader.Find("Custom/MeshDraw/Color");
				materialWireframe = new Material(shader);
				materialWireframe.DisableKeyword("NORMALS_ON");
			}
			return materialWireframe;
		case MeshMode.Dense:
			if (!materialDense) {
				var shader = Shader.Find("Custom/MeshDraw/Color");
				materialDense = new Material(shader);
				materialDense.DisableKeyword("NORMALS_ON");
			}
			return materialDense;
		case MeshMode.Sparse:
			if (!materialSparse) {
				var shader = Shader.Find("Custom/MeshDraw/Color");
				materialSparse = new Material(shader);
				materialSparse.EnableKeyword("NORMALS_ON");
			}
			return materialSparse;
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
