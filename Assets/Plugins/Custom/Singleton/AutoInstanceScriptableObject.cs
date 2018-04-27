using UnityEngine;

namespace Custom.Singleton
{
	///
	/// Provides static access to ScriptableObject of type <typeparam name="T">
	/// Creates an instance if none is present
	///
	/// Use <see cref="destroyed"> to protect against unwanted instantiations
	/// Like when accessing <see cref="instance"> inside OnDisable method
	///
	/// Intended to be used like
	/// public class ClassName : AutoInstanceScriptableObject<ClassName> { ... }
	///
	public abstract class AutoInstanceScriptableObject<T> : ScriptableObject
		where T : AutoInstanceScriptableObject<T>
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

				_instance = GetAutoScriptableObject();
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

		private static T GetAutoScriptableObject()
		{
			var result = FindObjectOfType<T>();
			if (!result)
			{
				result = CreateInstance<T>();
				result.name = $"Auto instance: {typeof(T).Name}";
			}

			return result;
		}
	}
}
