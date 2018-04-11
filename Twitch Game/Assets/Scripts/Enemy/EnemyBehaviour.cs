using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
	private GameObject _target;
	private float _health, _damage, _speed;

	private Rigidbody _rb;
	private Transform _castle;

	private GameManager _gameManager;

	private void Awake()
	{
		_gameManager = GameManager.Instance();
	}

	// Use this for initialization
	private void Start()
	{
		_health = 20;
		_damage = 10;
		_speed = 4;

		_castle = GameObject.FindWithTag("Castle").transform;
	}

	// Update is called once per frame
	private void Update()
	{
		var step = _speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, _castle.position, step);
	}

	public void Damage(float damage)
	{
		print(_health);
		_health -= damage;
		print(_health);
		if (_health <= 0.0f)
		{
			Destroy(gameObject);
			_gameManager.DeleteEnemy(gameObject);
		}
	}

	private void OnCollisionEnter(Collision other)
	{
		print("hit");
		if (other.transform.tag == "Bullet")
		{
			print("hit enemy");
			Damage(other.gameObject.GetComponent<BulletBehaviour>().GetDamage());
			Destroy(other.gameObject);
		}
	}
}