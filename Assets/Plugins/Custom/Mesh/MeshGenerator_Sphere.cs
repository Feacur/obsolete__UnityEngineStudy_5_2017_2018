using UnityEngine;

public static partial class MeshGenerator
{
	public static Mesh SphereWireframe(int latitude, int longitude)
	{
		Vector3[] vertices = SphereVerticesDense(latitude, longitude);
		int[] indices = SphereIndicesLines(latitude, longitude);
		
		Mesh mesh = new Mesh {
			vertices = vertices,
		};
		mesh.SetIndices(indices, MeshTopology.Lines, 0);
		return mesh;
	}

	public static Mesh SphereDense(int latitude, int longitude)
	{
		Vector3[] vertices = SphereVerticesDense(latitude, longitude);
		int[] indices = SphereIndicesDense(latitude, longitude);

		Mesh mesh = new Mesh {
			vertices = vertices,
		};
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		return mesh;
	}

	public static Mesh SphereDenseNormals(int latitude, int longitude)
	{
		Vector3[] vertices = SphereVerticesDense(latitude, longitude);
		int[] indices = SphereIndicesDense(latitude, longitude);

		int[] indicesLines = SphereIndicesLines(latitude, longitude);
		Vector3[] normals = new Vector3[vertices.Length];
		for (int i = 0; i < indicesLines.Length; i += 2) {
			int i0 = indicesLines[i + 0];
			int i1 = indicesLines[i + 1];

			var line = vertices[i1] - vertices[i0];
			normals[i0] -= line;
			normals[i1] += line;
		}
		
		for (int i = 0; i < normals.Length; i++) {
			normals[i] = Vector3.Normalize(normals[i]);
		}

		Mesh mesh = new Mesh {
			vertices = vertices,
			normals = normals,
		};
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		return mesh;
	}

	public static Mesh SphereSparse(int latitude, int longitude)
	{
		Vector3[] verticesDense = SphereVerticesDense(latitude, longitude);
		int[] indicesDense = SphereIndicesDense(latitude, longitude);

		Vector3[] vertices = new Vector3[indicesDense.Length];
		int[] indices = new int[indicesDense.Length * 3];
		for (int i = 0; i < indicesDense.Length; i++) {
			vertices[i] = verticesDense[indicesDense[i]];
			indices[i] = i;
		}
		
		Vector3[] normals = new Vector3[vertices.Length];
		for (int i = 0; i < vertices.Length; i += 3) {
			var normal = Vector3.Normalize(Vector3.Cross(
				vertices[i + 1] - vertices[i + 0],
				vertices[i + 2] - vertices[i + 1]
			));
			normals[i + 0] = normal;
			normals[i + 1] = normal;
			normals[i + 2] = normal;
		}

		Mesh mesh = new Mesh {
			vertices = vertices,
			normals = normals,
		};
		mesh.SetIndices(indices, MeshTopology.Triangles, 0);
		return mesh;
	}
	
	private static Vector3[] SphereVerticesDense(int latitude, int longitude)
	{
		int longitudeSegments = longitude - 1;
		int latitudeCircles   = longitude - 2;
		int latitudeSegments  = latitude;

		int length = 1 + latitudeCircles * latitude + 1;

		Vector3[] vertices = new Vector3[length];
		vertices[0]          = new Vector3(0, -0.5f, 0); // vertex circle 0
		vertices[length - 1] = new Vector3(0,  0.5f, 0); // vertex circle N

		float longitudeBase = -Mathf.PI / 2;
		float longitudeStep = Mathf.PI / longitudeSegments;
		float latitudeStep = 2 * Mathf.PI / latitudeSegments;
		
		for (int y = 1, i = 1; y <= latitudeCircles; ++y) { // vertex circles 1..(N-1)
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
	
	private static int[] SphereIndicesLines(int latitude, int longitude)
	{
		int longitudeSegments = longitude - 1;
		int latitudeCircles = longitude - 2;
		int indexLast = 1 + latitudeCircles * latitude;

		int[] indices = new int[
			latitude * (latitudeCircles + longitudeSegments) * 2
		];
		
		int i = 0;
		
		for (int x = 0; x < latitude; ++x) { // longitude lines
			int index = x + 1;
			
			indices[i++] = 0;
			for (int y = 0; y < latitudeCircles; ++y) {
				indices[i++] = index;
				indices[i++] = index;
				
				index += latitude;
			}
			indices[i++] = indexLast;
		}
		
		for (int y = 0; y < latitudeCircles; ++y) { // latitude circles
			int iBase = y * latitude + 1;

			indices[i++] = iBase;
			for (int x = 1; x < latitude; ++x) {
				indices[i++] = iBase + x;
				indices[i++] = iBase + x;
			}
			indices[i++] = iBase;
		}

		return indices;
	}

	private static int[] SphereIndicesDense(int latitude, int longitude)
	{
		int latitudeCircles = longitude - 2;
		int latitudeStrips  = longitude - 3;
		int indexLast = 1 + latitudeCircles * latitude;

		int[] indices = new int[
			latitude * (latitudeStrips + 1) * 2 * 3
		];

		for (int x = 0, i = 0; x < latitude; ++x) { // longitude strips
			int iBase = 1;
			int xNext = (x + 1) % latitude;

			indices[i++] = 0;
			indices[i++] = iBase + x;
			indices[i++] = iBase + xNext;

			for (int y = 0; y < latitudeStrips; ++y) {
				indices[i++] = iBase + xNext;
				indices[i++] = iBase + x;
				indices[i++] = iBase + latitude + x;
				
				indices[i++] = iBase + latitude + x;
				indices[i++] = iBase + latitude + xNext;
				indices[i++] = iBase + xNext;
				
				iBase += latitude;
			}

			indices[i++] = iBase + xNext;
			indices[i++] = iBase + x;
			indices[i++] = indexLast;
		}

		return indices;
	}
}
