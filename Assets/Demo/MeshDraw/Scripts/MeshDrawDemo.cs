using UnityEngine;

public class MeshDrawDemo : MonoBehaviour
{
	public MeshMode mode;
	public Color color;

	void Update ()
	{
		transform.rotation *= Quaternion.Euler(0, Mathf.Rad2Deg * Time.deltaTime, 0);
		MeshDraw.DrawBox(mode, transform.position, transform.rotation, color);
	}
}
