using UnityEngine;

public class Loader : MonoBehaviour
{
	private void Awake()
	{
		GameManager.Instance();
#if UNITY_EDITOR
		var testUser = new GameObject();
		testUser.AddComponent<TestUserLogin>();
		Destroy(testUser);
#endif
	}
}