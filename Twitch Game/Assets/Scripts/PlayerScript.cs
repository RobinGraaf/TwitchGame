using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	[SerializeField]
	private GameObject _tower;

	// Use this for initialization
	private void Start() { }

	// Update is called once per frame
	private void Update()
	{
		var ray = new Ray(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.down);

		if (Input.GetKey(KeyCode.Mouse0))
		{
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.collider.tag == "SpawnArea")
				{
					var mousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					Instantiate(_tower, new Vector3(mousePosWorld.x, 1.0f, mousePosWorld.z), Quaternion.identity);
				}
			}
		}
	}
}