using System.Collections;
using UnityEngine;

namespace Custom.Utils
{
	public static partial class Extensions
	{
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			var component = gameObject.GetComponent<T>();
			return component ? component : gameObject.AddComponent<T>();
		}

		public static void DeleteObject<T>(ref T reference, float delay = 0) where T : Object
		{
			if (!reference) { return; }
			Object.Destroy(reference, delay);
			reference = null;
		}
        
        public static void StopCoroutine(this MonoBehaviour parent, ref Coroutine coroutine)
        {
            if (coroutine == null) { return; }
			parent.StopCoroutine(coroutine);
			coroutine = null;
        }
	}
}
