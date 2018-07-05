using UnityEngine;

public static class MeshCache
{
	public static Mesh Mesh(MeshType type, MeshMode mode)
	{
		switch (type) {
		case MeshType.Box:
			return Box(mode);
		case MeshType.Sphere:
			return Sphere(mode);
		case MeshType.SpehereBox:
			return SphereBox(mode);
		}
		return null;
	}

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
	
	private static Mesh sphereWireframe;
	private static Mesh sphereDense;
	private static Mesh sphereSparse;
	public static Mesh Sphere(MeshMode mode)
	{
		switch (mode) {
		case MeshMode.Wireframe:
			return sphereWireframe ? sphereWireframe : (sphereWireframe = MeshGenerator.SphereWireframe());
		case MeshMode.Dense:
			return sphereDense ? sphereDense : (sphereDense = MeshGenerator.SphereDense());
		case MeshMode.Sparse:
			return sphereSparse ? sphereSparse : (sphereSparse = MeshGenerator.SphereSparse());
		}
		return null;
	}
	
	private static Mesh sphereBoxWireframe;
	private static Mesh sphereBoxDense;
	private static Mesh sphereBoxSparse;
	public static Mesh SphereBox(MeshMode mode)
	{
		switch (mode) {
		case MeshMode.Wireframe:
			return sphereBoxWireframe ? sphereBoxWireframe : (sphereBoxWireframe = MeshGenerator.SphereBoxWireframe());
		case MeshMode.Dense:
			return sphereBoxDense ? sphereBoxDense : (sphereBoxDense = MeshGenerator.SphereBoxDense());
		case MeshMode.Sparse:
			return sphereBoxSparse ? sphereBoxSparse : (sphereBoxSparse = MeshGenerator.SphereBoxSparse());
		}
		return null;
	}
}
