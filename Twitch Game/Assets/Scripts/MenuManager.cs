using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
	[SerializeField]
	private InputField _usernameField, _passwordField;

	private GameManager _gameManager;

	private void Awake()
	{
		_gameManager = GameManager.Instance();
	}

	// Use this for initialization
	public void StartGame()
	{
		var username = _usernameField.text == "" ? _gameManager.GetUsername() : _usernameField.text;
		var password = _passwordField.text == "" ? _gameManager.GetPassword() : _passwordField.text;

		_gameManager.SetInfo(username, password);
        
	    Cursor.lockState = CursorLockMode.Locked;
	    Cursor.visible = false;

        SceneManager.LoadScene(1);
	}
}