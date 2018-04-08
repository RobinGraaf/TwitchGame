using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    [SerializeField] InputField usernameField, passwordField;

	// Use this for initialization
	public void StartGame () {
        string username = usernameField.text;
        string password = passwordField.text;

        GameManager.instance.SetInfo(username, password);

        SceneManager.LoadScene(1);
    }
}
