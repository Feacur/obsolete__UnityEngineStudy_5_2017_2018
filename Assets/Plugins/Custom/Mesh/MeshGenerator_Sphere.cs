using UnityEngine;

public static partial class MeshGenerator
{
	public static Mesh SphereWireframe(int latitude, int longitude)
	{
		Vector3[] vertices = SphereVertices(latitude, longitude);

		int longitudeSegments = longitude - 1;
		int latitudeCircles = longitude - 2;

		int[] indices = new int[
			latitude * (latitudeCircles + longitudeSegments) * 2
		];
		
		int i = 0;
		
		for (int x = 1; x <= latitude; ++x) {
			int index = x;
			indices[i++] = 0;
			for (int y = 0; y < latitudeCircles; ++y) {
				indices[i++] = index;
				indices[i++] = index;
				index += latitude;
			}
			indices[i++] = vertices.Length - 1;
		}
		
		for (int y = 0; y < latitudeCircles; ++y) {
			int iBase = 1 + y * latitude;
			indices[i++] = iBase;
			for (int x = 1; x < latitude; ++x) {
				indices[i++] = iBase + x;
				indices[i++] = iBase + x;
			}
			indices[i++] = iBase;
		}

		Mesh mesh = new Mesh {
			vertices = vertices,
		};
		mesh.SetIndices(indices, MeshTopology.Lines, 0);
		return mesh;
	}

	public static Mesh SphereDense(int latitude, int longitude)
	{
		Vector3[] vertices = { };
		int[] indices = { };

		Mesh mesh = new Mesh {
			vertices = vertices,
		};
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		return mesh;
	}

	public static Mesh SphereSparse(int latitude, int longitude)
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
	
	private static Vector3[] SphereVertices(int latitude, int longitude)
	{
		int longitudeSegments = longitude - 1;
		int latitudeSegments = latitude;
		int latitudeCircles = longitude - 2;

		int length = 1 + latitudeCircles * latitude + 1;

		Vector3[] vertices = new Vector3[length];
		vertices[0]          = new Vector3(0, -0.5f, 0);
		vertices[length - 1] = new Vector3(0,  0.5f, 0);

		float longitudeBase = -Mathf.PI / 2;
		float longitudeStep = Mathf.PI / longitudeSegments;
		float latitudeStep = 2 * Mathf.PI / latitudeSegments;
		
		for (int y = 1, i = 1; y <= latitudeCircles; ++y) {
			float longitudeAngle = longitudeBase + longitudeStep * y;
			var vertex = 0.5f * new Vector2(
				Mathf.Cos(longitudeAngle),
				Mathf.Sin(longitudeAngle)
			);
			
			for (int x = 0; x < latitude; ++x, ++i) {
				float latitudeAngle = latitudeStep * x;
				vertices[i] = new Vector3(
					vertex.x * Mathf.Cos(latitudeAngle),
					vertex.y,
					vertex.x * Mathf.Sin(latitudeAngle)
				);
			}
		}

		return vertices;
	}
}
