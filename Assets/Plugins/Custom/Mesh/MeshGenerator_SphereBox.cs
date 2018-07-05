using UnityEngine;

public static partial class MeshGenerator
{
	public static Mesh SphereBoxWireframe()
	{
		Vector3[] vertices = { };
		int[] indices = { };

		Mesh mesh = new Mesh {
			vertices = vertices,
		};
		mesh.SetIndices(indices, MeshTopology.Lines, 0);
		return mesh;
	}

	public static Mesh SphereBoxDense()
	{
		Vector3[] vertices = { };
		int[] indices = { };

		Mesh mesh = new Mesh {
			vertices = vertices,
		};
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		return mesh;
	}

	public static Mesh SphereBoxSparse()
	{
		Vector3[] vertices = { };
		int[] indices = { };
		Vector3[] normals = { };

		Mesh mesh = new Mesh {
			vertices = vertices,
			normals = normals,
		};
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		return mesh;
	}
	
	private static Vector3[] SphereBoxVertices()
	{
		Vector3[] vertices = { };
		return vertices;
	}
}
