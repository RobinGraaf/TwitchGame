using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private readonly List<GameObject> _enemies;
    protected enum Effects { ENone, EPoison, EBurn, EFreeze }

    protected float Health { get; set; }
    protected float Damage { get; set; }
    protected float Range { get; set; }
    protected float FireRate { get; set; }
    protected Vector2 Size { get; set; }
    protected int Cost { get; set; }
    protected bool Aerial { get; set; }
    public int Effect { get; set; }

    private float _timer;
    [SerializeField] private GameObject _bulletPrefab;

    public Tower()
    {
        _enemies = GameManager.Instance().GetEnemies();

        Health = 20.0f;
        Damage = 10.0f;
        Range = 30.0f;
        FireRate = 1.0f;
        Size = new Vector2(1, 1);
        Cost = 0;
        Aerial = false;
        Effect = (int)Effects.ENone;
    }

    private void Update() {
       ShootInterval();
    }

    protected virtual void ShootInterval()
    {
        if (_enemies.Count > 0) {
            _timer += Time.deltaTime;
            if (_timer >= FireRate) {
                _timer = 0;
                CalculateDistance();
            }
        }
    }

    protected virtual void CalculateDistance() {
        var closestEnemy = _enemies[0];
        var closestDistance = Range + 0.1f;

        foreach (var enemy in _enemies) {
            var dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < closestDistance) {
                closestDistance = dist;
                closestEnemy = enemy;
            }
        }

        if (closestDistance <= Range)
        {
            ShootAt(closestEnemy);
        }
    }

    protected virtual void ShootAt(GameObject target) {
        transform.LookAt(target.transform);

        var bullet = Instantiate(_bulletPrefab, new Vector3(GetComponentInChildren<Transform>().position.x, GetComponentInChildren<Transform>().position.y, GetComponentInChildren<Transform>().position.z), Quaternion.identity);

        bullet.GetComponent<Bullet>().Damage = Damage;
        bullet.GetComponent<Bullet>().Target = target;
        bullet.GetComponent<Bullet>().Effect = Effect;
    }
}
