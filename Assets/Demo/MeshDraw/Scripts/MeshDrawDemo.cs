using UnityEngine;

namespace Demo.MeshDraw {
	public class MeshDrawDemo : MonoBehaviour
	{
		public MeshType type;
		public MeshMode mode;
		public Color color;
		public int[] ints;

		void Update ()
		{
			global::MeshDraw.DrawMesh(type, mode, transform.position, transform.rotation, color, ints);
		}
	}
}
