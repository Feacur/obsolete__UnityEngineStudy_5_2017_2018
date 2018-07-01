using UnityEngine;

public static class MeshCache
{
	private static Mesh boxSolid;
	public static Mesh BoxSolid()
	{
		if (!boxSolid) { boxSolid = MeshGenerator.BoxSolid(); }
		return boxSolid;
	}
}
