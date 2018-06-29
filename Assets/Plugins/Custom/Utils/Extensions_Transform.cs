using UnityEngine;

namespace Custom.Utils
{
	public static partial class Extensions
	{
		public static Transform[] GetPrimaryChildren(this Transform transform)
		{
			var result = new Transform[transform.childCount];
			foreach (Transform child in transform)
			{
				result[child.GetSiblingIndex()] = child;
			}
			return result;
		}

		public static void DestroyChildren(this Transform transform)
		{
			foreach (var child in transform.GetPrimaryChildren())
			{
				child.gameObject.SetActive(false);
				Object.Destroy(child.gameObject);
			}
		}
	}
}
