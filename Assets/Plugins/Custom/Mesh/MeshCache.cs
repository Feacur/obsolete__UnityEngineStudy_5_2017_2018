using System.Collections.Generic;
using UnityEngine;

public static class MeshCache
{
	public static Mesh Mesh(MeshType type, MeshMode mode, params int[] ints)
	{
		switch (type) {
		case MeshType.Box:
			return Box(mode);
		case MeshType.Sphere:
			return Sphere(mode, Mathf.Clamp(ints[0], 3, 32), Mathf.Clamp(ints[1], 3, 32));
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
		case MeshMode.DenseNormals:
			return boxDense ? boxDense : (boxDense = MeshGenerator.BoxDense());
		case MeshMode.Sparse:
			return boxSparse ? boxSparse : (boxSparse = MeshGenerator.BoxSparse());
		}
		return null;
	}
	
	private static Dictionary<int, Mesh> sphereWireframe = new Dictionary<int, Mesh>();
	private static Dictionary<int, Mesh> sphereDense = new Dictionary<int, Mesh>();
	private static Dictionary<int, Mesh> sphereSparse = new Dictionary<int, Mesh>();
	public static Mesh Sphere(MeshMode mode, int longitude, int latitude)
	{
		int key = (longitude << 16) | latitude;
		Mesh mesh = null;
		switch (mode) {
		case MeshMode.Wireframe:
			if (!sphereWireframe.TryGetValue(key, out mesh)) {
				mesh = MeshGenerator.SphereWireframe(longitude, latitude);
				sphereWireframe.Add(key, mesh);
			}
			break;

		case MeshMode.Dense:
			if (!sphereDense.TryGetValue(key, out mesh)) {
				mesh = MeshGenerator.SphereDense(longitude, latitude);
				sphereDense.Add(key, mesh);
			}
			break;

		case MeshMode.DenseNormals:
			if (!sphereDense.TryGetValue(key, out mesh)) {
				mesh = MeshGenerator.SphereDenseNormals(longitude, latitude);
				sphereDense.Add(key, mesh);
			}
			break;

		case MeshMode.Sparse:
			if (!sphereSparse.TryGetValue(key, out mesh)) {
				mesh = MeshGenerator.SphereSparse(longitude, latitude);
				sphereSparse.Add(key, mesh);
			}
			break;
		}
		return mesh;
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
		case MeshMode.DenseNormals:
			return sphereBoxDense ? sphereBoxDense : (sphereBoxDense = MeshGenerator.SphereBoxDense());
		case MeshMode.Sparse:
			return sphereBoxSparse ? sphereBoxSparse : (sphereBoxSparse = MeshGenerator.SphereBoxSparse());
		}
		return null;
	}
}
