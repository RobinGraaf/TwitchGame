using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager> {
    public string Username { get; set; }

    public string Password { get; set; }

    private string _channel;
    public string Channel {
        get { return _channel; }
        set { _channel = value.ToLower(); }
    }

    private string _username, _password;
    private List<GameObject> _enemyList;
    private bool _paused = false;

    private void Start() {
        _enemyList = new List<GameObject>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton9)) {
            if (_paused) {
                _paused = false;
                Time.timeScale = 1;

                GameObject.FindWithTag("Player").GetComponent<MouseLook>().enabled = true;
                GameObject.FindWithTag("MainCamera").GetComponent<MouseLook>().enabled = true;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            } else {
                _paused = true;
                Time.timeScale = 0;
                GameObject.FindWithTag("Player").GetComponent<MouseLook>().enabled = false;
                GameObject.FindWithTag("MainCamera").GetComponent<MouseLook>().enabled = false;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public List<GameObject> GetEnemies() {
        return _enemyList;
    }

    public void AddEnemy(GameObject enemy) {
        _enemyList.Add(enemy);
    }

    public void DeleteEnemy(GameObject enemy) {
        _enemyList.Remove(enemy);
    }
}