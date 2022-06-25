using System;
using DefaultNamespace.Utils;

namespace DefaultNamespace
{
	public class AppController : PrivateSingleton<AppController>
	{
		private GameplayHUDController _hudController;

		public static GameplayHUDController GameplayHudController
		{
			get { return Instance._hudController;}
			set { Instance._hudController = value; }
		}

		private SceneController _sceneController;

		public static SceneController SceneController
		{
			get { return Instance._sceneController; }
			set { Instance._sceneController = value; }
		}

	}
}