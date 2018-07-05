using UnityEngine;

public class MeshDrawDemo : MonoBehaviour
{
	public MeshType type;
	public MeshMode mode;
	public Color color;

	void Update ()
	{
		transform.rotation *= Quaternion.Euler(0, Mathf.Rad2Deg * Time.deltaTime, 0);
		MeshDraw.DrawMesh(type, mode, transform.position, transform.rotation, color);
	}
}
