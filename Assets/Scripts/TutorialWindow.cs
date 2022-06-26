using UnityEngine;

namespace DefaultNamespace
{
	public class TutorialWindow : MonoBehaviour
	{
		public void OnCloseButtonClick()
		{
			Destroy(gameObject);
		}
	}
}