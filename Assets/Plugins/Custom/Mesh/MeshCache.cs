using UnityEngine;

public static class MeshCache
{
	private static Mesh boxWireframe;
	private static Mesh boxDense;
	private static Mesh boxSparse;
	public static Mesh Box(MeshMode mode)
	{
		switch (mode) {
		case MeshMode.Wireframe:
			return boxWireframe ? boxWireframe : (boxWireframe = MeshGenerator.BoxWireframe());
		case MeshMode.Dense:
			return boxDense ? boxDense : (boxDense = MeshGenerator.BoxDense());
		case MeshMode.Sparse:
			return boxSparse ? boxSparse : (boxSparse = MeshGenerator.BoxSparse());
		}
		return null;
	}
}
