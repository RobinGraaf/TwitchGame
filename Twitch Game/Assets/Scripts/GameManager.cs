using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    string username, password;
    List<GameObject> enemyList;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
        enemyList = new List<GameObject>();
    }

    public void SetInfo(string username, string password) {
        this.username = username;
        this.password = password;
    }

    public string GetUsername() {
        return this.username;
    }

    public string GetPassword() {
        return this.password;
    }

    public List<GameObject> GetEnemies() {
        return this.enemyList;
    }

    public void AddEnemy(GameObject enemy) {
        enemyList.Add(enemy);
    }

    public void DeleteEnemy(GameObject enemy) {
        enemyList.Remove(enemy);
    }
}
