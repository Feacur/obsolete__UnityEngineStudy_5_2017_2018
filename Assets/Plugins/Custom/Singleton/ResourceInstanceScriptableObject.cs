using UnityEngine;

namespace Custom.Singleton
{
	///
	/// Provides static access to ScriptableObject of type <typeparam name="T">
	/// Loads an instance if none is present
	///
	/// Use <see cref="destroyed"> to protect against unwanted instantiations
	/// Like when accessing <see cref="instance"> inside OnDisable method
	///
	/// Intended to be used like
	/// public class ClassName : ResourceInstanceScriptableObject<ClassName> { ... }
	///
	public abstract class ResourceInstanceScriptableObject<T> : ScriptableObject
		where T : ResourceInstanceScriptableObject<T>
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

				_instance = GetResourceScriptableObject();
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
				destroyed = false;
			}
		}

		protected void OnDestroy()
		{
			if (ReferenceEquals(this, _instance))
			{
				destroyed = true;
			}
		}

		//
		//
		//

		public static T GetResourceScriptableObject()
		{
			var result = FindObjectOfType<T>();
			if (!result)
			{
				var prefab = Resources.Load<T>(typeof(T).Name);
				result = Instantiate(prefab);
			}

			return result;
		}
	}
}
