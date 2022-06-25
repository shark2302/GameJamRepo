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
		
		private FightController _fightController;

		public static FightController FightController
		{
			get { return Instance._fightController; }
			set { Instance._fightController = value; }
		}
		
		private FollowCamera _camera;

		public static FollowCamera Camera
		{
			get { return Instance._camera; }
			set { Instance._camera = value; }
		}

	}
}