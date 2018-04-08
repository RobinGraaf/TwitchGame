using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {

    GameObject target;
    float health, damage, speed;

    Rigidbody rb;
    Transform castle;
	// Use this for initialization
	void Start () {
        health = 20;
        damage = 10;
        speed = 4;

        castle = GameObject.FindWithTag("Castle").transform;
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, castle.position, step);
	}

    public void Damage(float damage) {
        print(health);
        this.health -= damage;
        print(health);
        if (health <= 0.0f) {
            Destroy(gameObject);
            GameManager.instance.DeleteEnemy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other) {
        print("hit");
        if (other.transform.tag == "Bullet") {
            print("hit enemy");
            Damage(other.gameObject.GetComponent<BulletBehaviour>().GetDamage());
            Destroy(other.gameObject);
        }
    }
}
