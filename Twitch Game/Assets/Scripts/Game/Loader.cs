using UnityEngine;

public class Loader : MonoBehaviour
{
	private void Awake()
	{
		TwitchChat.Instance();
		GameManager.Instance();
	}
}