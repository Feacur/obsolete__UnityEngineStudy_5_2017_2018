using UnityEngine;

public static class MeshCache
{
	private static Mesh boxSolid;
	private static Mesh boxWireframe;
	private static Mesh boxNormals;
	public static Mesh Box(MeshMode mode)
	{
		switch (mode) {
		case MeshMode.Solid:
			return boxSolid ? boxSolid : (boxSolid = MeshGenerator.BoxSolid());
		case MeshMode.Wireframe:
			return boxWireframe ? boxWireframe : (boxWireframe = MeshGenerator.BoxWireframe());
		case MeshMode.Normals:
			return boxNormals ? boxNormals : (boxNormals = MeshGenerator.BoxNormals());
		}
		return null;
	}
}
