using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

    GameObject target;
    float speed;
    float damage;

    private void Start() {
        speed = 7.5f;
    }

    // Update is called once per frame
    void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, step);
	}

    public void SetTarget(GameObject target) {
        this.target = target;
    }

    public void SetDamage(float damage) {
        this.damage = damage;
    }

    public float GetDamage() {
        return this.damage;
    }
}
