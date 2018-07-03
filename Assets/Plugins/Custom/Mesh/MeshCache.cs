using UnityEngine;

public static class MeshCache
{
	private static Mesh boxSolid;
	private static Mesh boxWireframe;
	public static Mesh Box(MeshMode mode)
	{
		switch (mode) {
		case MeshMode.Solid:
			return boxSolid ? boxSolid : (boxSolid = MeshGenerator.BoxSolid());
		case MeshMode.Wireframe:
			return boxWireframe ? boxWireframe : (boxWireframe = MeshGenerator.BoxWireframe());
		}
		return null;
	}
}
