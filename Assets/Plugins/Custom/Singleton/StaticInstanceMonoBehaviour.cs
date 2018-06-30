using UnityEngine;

namespace Custom.Singleton
{
	///
	/// Provides static access to MonoBehaviour of type <typeparam name="T">
	/// If any is present and belongs to an active game object
	///
	/// Intended to be used like
	/// public class ClassName : StaticInstanceMonoBehaviour<ClassName> { ... }
	///
	public abstract class StaticInstanceMonoBehaviour<T> : MonoBehaviour
		where T : StaticInstanceMonoBehaviour<T>
	{
		//
		// API
		//

		public static bool destroyed { get; private set; }

		private static T _instance;

		public static T instance
		{
			get
			{
				if (_instance)
				{
					return _instance;
				}

				_instance = FindObjectOfType<T>();
				destroyed = false;
				return _instance;
			}
		}

		//
		// Callbacks from Unity
		//

		protected void Awake()
		{
			if (!_instance || destroyed)
			{
				_instance = (T) this;
			}
		}

		protected void OnDestroy()
		{
			if (ReferenceEquals(this, _instance))
			{
				destroyed = true;
			}
		}
	}
}
