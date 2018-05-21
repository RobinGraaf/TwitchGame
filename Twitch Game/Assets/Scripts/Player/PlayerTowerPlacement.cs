using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTowerPlacement : MonoBehaviour
{

    [SerializeField] private GameObject _placeableObjectPrefab;
    [SerializeField] private KeyCode _newObjectHotkey;
    [SerializeField] private float _spawnDistance;
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
            Vector3 currentPos = _currentPlaceableObject.transform.position;
            MapData.Tile tile = MapData.Instance().GetTileAt((int) currentPos.x, (int) currentPos.z);
            if (tile.Type == 2 && !tile.IsUsed)
            {
                _currentPlaceableObject = null;
               tile.IsUsed = true;
            }
        }
    }

    private void RotateCurrentPlaceableObject()
    {
        _mouseWheelRotation = Input.mouseScrollDelta.y;
        _currentPlaceableObject.transform.Rotate(Vector3.up, _mouseWheelRotation * 90.0f); // Rotate 90 degrees on every mousewheel tick
    }

    private void MoveCurrentPlaceableObject()
    {
        Vector3 playerPos = transform.position;
        Vector3 playerDirection = transform.forward;

        Vector3 spawnPos = playerPos + playerDirection * _spawnDistance;
        _currentPlaceableObject.transform.position = MapData.Instance().GetClosestNode(spawnPos) + Vector3.up/2;
    }

    private void PlaceNewObject()
    {
        if (Input.GetKeyDown(_newObjectHotkey))
        {
            if (_currentPlaceableObject == null)
            {
                Vector3 playerPos = transform.position;
                Vector3 playerDirection = transform.forward;

                Vector3 spawnPos = playerPos + playerDirection * _spawnDistance;
                _currentPlaceableObject = Instantiate(_placeableObjectPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                Destroy(_currentPlaceableObject);
            }
        }
    }
}
