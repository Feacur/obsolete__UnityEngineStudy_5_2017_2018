using UnityEngine;

public static partial class MeshGenerator
{
	private static readonly Vector3[] boxVertices = {
		new Vector3(-0.5f, -0.5f, -0.5f),
		new Vector3( 0.5f, -0.5f, -0.5f),
		new Vector3(-0.5f,  0.5f, -0.5f),
		new Vector3( 0.5f,  0.5f, -0.5f),
		new Vector3(-0.5f, -0.5f,  0.5f),
		new Vector3( 0.5f, -0.5f,  0.5f),
		new Vector3(-0.5f,  0.5f,  0.5f),
		new Vector3( 0.5f,  0.5f,  0.5f),
	};

	public static Mesh BoxSolid()
	{
		var vertices = boxVertices;
		int[] indices = {
			2, 1, 0, 1, 2, 3, // back
			4, 5, 6, 7, 6, 5, // front
			4, 2, 0, 2, 4, 6, // left
			1, 3, 5, 7, 5, 3, // right
			0, 1, 4, 5, 4, 1, // bottom
			6, 3, 2, 3, 6, 7, // top
		};

		Mesh mesh = new Mesh {
			vertices = vertices,
		};
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		return mesh;
	}
	
	public static Mesh BoxWireframe()
	{
		var vertices = boxVertices;
		int[] indices = {
			0, 1, 1, 3, 3, 2, 2, 0, // back
			4, 5, 5, 7, 7, 6, 6, 4, // front
			0, 4, 1, 5, 2, 6, 3, 7, // side
		};

		Mesh mesh = new Mesh {
			vertices = vertices,
		};
		mesh.SetIndices(indices, MeshTopology.Lines, 0);
		return mesh;
	}
}
