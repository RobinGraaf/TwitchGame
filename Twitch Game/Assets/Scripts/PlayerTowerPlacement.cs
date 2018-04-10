using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerPlacement : MonoBehaviour
{

    [SerializeField] private GameObject _placeableObjectPrefab;
    [SerializeField] private KeyCode _newObjectHotkey = KeyCode.Tab;
    private GameObject _currentPlaceableObject;
    private float _mouseWheelRotation;

    // Update is called once per frame
    void Update ()
	{
	    PlaceNewObject();

	    if (_currentPlaceableObject != null)
	    {
	        MoveCurrentPlaceableObject();
	        RotateCurrentPlaceableObject();
	        ReleaseCurrentPlaceableObject();
	    }
	}

    private void ReleaseCurrentPlaceableObject()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _currentPlaceableObject = null;
        }
    }

    private void RotateCurrentPlaceableObject()
    {
        _mouseWheelRotation += Input.mouseScrollDelta.y;
        _currentPlaceableObject.transform.Rotate(Vector3.up, _mouseWheelRotation * 10.0f);
    }

    private void MoveCurrentPlaceableObject()
    {
        Vector3 playerPos = transform.position;
        Vector3 playerDirection = transform.forward;
        float spawnDistance = 2.0f;

        Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
        spawnPos.y = 1.0f;
        _currentPlaceableObject.transform.position = spawnPos;
    }

    private void PlaceNewObject()
    {
        if (Input.GetKeyDown(_newObjectHotkey))
        {
            if (_currentPlaceableObject == null)
            {
                Vector3 playerPos = transform.position;
                Vector3 playerDirection = transform.forward;
                float spawnDistance = 2.0f;

                Vector3 spawnPos = playerPos + playerDirection * spawnDistance;
                spawnPos.y = 1.0f;
                _currentPlaceableObject = Instantiate(_placeableObjectPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                Destroy(_currentPlaceableObject);
            }
        }
    }
}
