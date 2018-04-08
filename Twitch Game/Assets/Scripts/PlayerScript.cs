using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    [SerializeField] GameObject tower;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        RaycastHit hit;
        Ray ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down);

        if (Input.GetKey(KeyCode.Mouse0)) {
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "SpawnArea") {
                    Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Instantiate(tower, new Vector3(mousePosWorld.x, 1.0f, mousePosWorld.z), Quaternion.identity);
                }
            }
        }
	}
}
