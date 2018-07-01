using UnityEngine;

public class MeshDrawDemo : MonoBehaviour
{
	public Color color;

	void Update ()
	{
		transform.rotation *= Quaternion.Euler(0, Mathf.Rad2Deg * Time.deltaTime, 0);
		MeshDraw.DrawBox(transform.position, transform.rotation, color);
	}
}
