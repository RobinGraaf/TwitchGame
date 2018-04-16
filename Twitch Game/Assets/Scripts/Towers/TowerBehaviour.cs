using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour
{
	private List<GameObject> _enemies;
	[SerializeField] private GameObject _bulletPrefab;
	private float _timer, _interval;
    [SerializeField] private float _towerRange;

	private GameManager _gameManager;

	private void Awake()
	{
		_gameManager = GameManager.Instance();
	}

	// Use this for initialization
	private void Start()
	{
		_enemies = _gameManager.GetEnemies();
		_timer = 0.0f;
		_interval = 1.5f;
	    _towerRange = 20.0f;
    }

	// Update is called once per frame
	private void Update()
	{
		_timer += Time.deltaTime;
		if (_timer >= _interval)
		{
			_timer = 0;
			if (_enemies.Count > 0)
			{
				CalculateDistance();
			}
		}
	}

	private void CalculateDistance()
	{
		var closestEnemy = _enemies[0];
		var closestDistance = 30.0f;

		foreach (var enemy in _enemies)
		{
			var dist = Vector3.Distance(transform.position, enemy.transform.position);
			if (dist < closestDistance)
			{
				closestDistance = dist;
				closestEnemy = enemy;
			}
		}

		if (closestDistance <= _towerRange)
		{
			ShootAt(closestEnemy);
		}
	}

	private void ShootAt(GameObject closest)
	{
		transform.LookAt(closest.transform);
		var bullet = Instantiate(_bulletPrefab, new Vector3(GetComponentInChildren<Transform>().position.x, GetComponentInChildren<Transform>().position.y, GetComponentInChildren<Transform>().position.z), Quaternion.identity);
		bullet.GetComponent<BulletBehaviour>().SetTarget(closest);
		bullet.GetComponent<BulletBehaviour>().SetDamage(10.0f);
	}
}