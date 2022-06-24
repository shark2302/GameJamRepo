using UnityEngine;

namespace DefaultNamespace
{
	public class FollowCamera : MonoBehaviour
	{
		[SerializeField] 
		private GameObject FollowedObject;

		private void Update()
		{
			var position = FollowedObject.transform.position;
			transform.position = new Vector3(position.x, position.y, transform.position.z);
		}
	}
}