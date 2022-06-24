using System.Reflection;

namespace DefaultNamespace.Utils
{
	[Obfuscation(Exclude = false, ApplyToMembers = true)]
	public class Singleton<T> where T : Singleton<T>, new()
	{
		private static T _instance;

		public static T Instance
		{
			get { return _instance ?? (_instance = new T()); }
		}

		public static void ReleaseInstance()
		{
			_instance = null;
		}
	}

	[Obfuscation(Exclude = false, ApplyToMembers = true)]
	public class PrivateSingleton<T> where T : PrivateSingleton<T>, new()
	{
		private static T _instance;

		protected static T Instance
		{
			get { return _instance ?? (_instance = new T()); }
		}

		protected static void ReleaseInstance()
		{
			_instance = null;
		}
	}
}