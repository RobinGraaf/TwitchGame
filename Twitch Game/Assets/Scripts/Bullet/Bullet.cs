using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Damage { get; set; }
    public GameObject Target { get; set; }
    public int Effect { get; set; }
    
    private float _speed;
    private float _lifespan;
    private float _lifetime;

    protected enum Effects
    {
        ENone,
        EPoison,
        EBurn,
        EFreeze
    }

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
        if (_lifetime >= _lifespan || Target == null) Destroy(gameObject);
        if (Target != null)
        {
            var step = _speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, step);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        print("hit");
        if (other.transform.tag == "Enemy")
        {
            print("hit enemy");
            other.gameObject.GetComponent<EnemyBehaviour>().Damage(Damage);
            Destroy(gameObject);
        }
    }
}