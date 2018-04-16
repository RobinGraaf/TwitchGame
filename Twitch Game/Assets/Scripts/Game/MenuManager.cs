using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField]
	private InputField _usernameField;

	private GameManager _gameManager;

	private void Awake()
	{
		_gameManager = GameManager.Instance();
	}

	// Use this for initialization
	public void StartGame()
	{
		_gameManager.Channel = _usernameField.text;

	    Cursor.lockState = CursorLockMode.Locked;
	    Cursor.visible = false;

        StartCoroutine(LoadScene());
	}

	private IEnumerator LoadScene()
	{
		yield return SceneManager.LoadSceneAsync(1);
	}
}