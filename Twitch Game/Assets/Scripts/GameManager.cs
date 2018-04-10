using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
	public string Username { get; set; }

	public string Password { get; set; }

	private string _channel;
	public string Channel
	{
		get { return _channel; }
		set { _channel = value.ToLower(); }
	}

	private string _username, _password;
	private List<GameObject> _enemyList;

	private void Start()
	{
		_enemyList = new List<GameObject>();
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