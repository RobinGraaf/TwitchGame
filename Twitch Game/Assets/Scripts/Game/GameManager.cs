using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private string _username, _password;
	private List<GameObject> _enemyList;
    private bool _paused;
    //[SerializeField] private GameObject _playerPrefab;

    private void Start()
	{
		_enemyList = new List<GameObject>();

        // ONLY FOR TESTING
        // SHOULD HAPPEN ONLY WHEN GOING TO GAME FROM MENU
     //   Instantiate(_playerPrefab, GameObject.FindWithTag("Castle").transform.position + (Vector3.forward * 4), Quaternion.identity);
     //   Cursor.lockState = CursorLockMode.Locked;
	    //Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_paused)
            {
                _paused = false;
                Time.timeScale = 1;

                GameObject.FindWithTag("Player").GetComponent<MouseLook>().enabled = true;
                GameObject.FindWithTag("MainCamera").GetComponent<MouseLook>().enabled = true;
                
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                _paused = true;
                Time.timeScale = 0;
                GameObject.FindWithTag("Player").GetComponent<MouseLook>().enabled = false;
                GameObject.FindWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;
                
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

	public void SetInfo(string username, string password)
	{
		_username = username;
		_password = password;
	}

	public string GetUsername()
	{
		return _username;
	}

	public string GetPassword()
	{
		return _password;
	}

	public List<GameObject> GetEnemies()
	{
		return _enemyList;
	}

	public void AddEnemy(GameObject enemy)
	{
		_enemyList.Add(enemy);
	}

	public void DeleteEnemy(GameObject enemy)
	{
		_enemyList.Remove(enemy);
	}
}