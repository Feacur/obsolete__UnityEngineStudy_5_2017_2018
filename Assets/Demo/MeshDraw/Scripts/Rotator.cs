using UnityEngine;

namespace Demo.MeshDraw {
	public class Rotator : MonoBehaviour
	{
		void Update ()
		{
			transform.rotation *= Quaternion.Euler(0, Mathf.Rad2Deg * Time.deltaTime, 0);
		}
	}
}
