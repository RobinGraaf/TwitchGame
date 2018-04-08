using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehaviour : MonoBehaviour {

    List<GameObject> enemies;
    [SerializeField] GameObject bulletPrefab;
    float timer, interval;
	// Use this for initialization
	void Start () {
        enemies = GameManager.instance.GetEnemies();
        timer = 0.0f;
        interval = 3.0f;
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        // TODO: Add interval
        if (timer >= interval) {
            timer = 0;
            if (enemies.Count > 0) {
                CalculateDistance();
            }
        }
    }

    void CalculateDistance(){
        GameObject closestEnemy = enemies[0];
        float closestDistance = 30.0f;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(this.transform.position, enemy.transform.position);
            print(dist);
            if (dist < closestDistance)
            {
                print(closestDistance);
                print(dist);
                closestDistance = dist;
                print(closestDistance);
                closestEnemy = enemy;
            }
        }
        
        if(closestDistance <= 20.0f) {
            ShootAt(closestEnemy);
        }
    }

    void ShootAt(GameObject closest) {
        this.transform.LookAt(closest.transform);
        GameObject bullet = Instantiate(bulletPrefab, new Vector3(this.transform.position.x, 1.5f, this.transform.position.z), Quaternion.identity);
        bullet.GetComponent<BulletBehaviour>().SetTarget(closest);
        bullet.GetComponent<BulletBehaviour>().SetDamage(10.0f);
    }
}
