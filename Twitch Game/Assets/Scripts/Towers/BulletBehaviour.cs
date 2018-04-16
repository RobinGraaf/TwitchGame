using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
	private GameObject _target;
	private float _speed;
	private float _damage;
	private float _lifespan;
	private float _lifetime;

	private void Start()
	{
		_speed = 7.5f;
		_lifespan = 10f;
		_lifetime = 0;
	}

	// Update is called once per frame
	private void Update()
	{
	    _lifetime += Time.deltaTime;
		if (_lifetime >= _lifespan || _target == null)
		{
			Destroy(gameObject);
		}
		if (_target)
		{
			var step = _speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, step);
		}
	}

	public void SetTarget(GameObject target)
	{
		this._target = target;
	}

	public void SetDamage(float damage)
	{
		this._damage = damage;
	}

	public float GetDamage()
	{
		return _damage;
	}
}