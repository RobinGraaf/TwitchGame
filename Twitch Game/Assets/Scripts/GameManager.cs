using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	private string _username, _password;
	private List<GameObject> _enemyList;

	private void Start()
	{
		_enemyList = new List<GameObject>();
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