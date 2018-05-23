using UnityEngine;

public class PlayerTowerPlacement : MonoBehaviour
{
    [SerializeField] private GameObject _placeableObjectPrefab;
    [SerializeField] private KeyCode _newObjectkey, _placeObjectKey;
    [SerializeField] private float _spawnDistance;
    private GameObject _currentPlaceableObject;
    private float _mouseWheelRotation;

    // Update is called once per frame
    private void Update()
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
        if (Input.GetKeyDown(_placeObjectKey))
            if (MapData.Instance().GetTileAt(Mathf.FloorToInt(_currentPlaceableObject.transform.position.x),
                    Mathf.FloorToInt(_currentPlaceableObject.transform.position.z)) == 2)
            {
                var newPosition = MapData.Instance().GetClosestNode(_currentPlaceableObject.transform.position);
                newPosition.y = 0.5f;

                _currentPlaceableObject.transform.position = newPosition;
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
        var playerPos = transform.position;
        var playerDirection = transform.forward;

        var spawnPos = playerPos + playerDirection * _spawnDistance;
        _currentPlaceableObject.transform.position = MapData.Instance().GetClosestNode(spawnPos) + Vector3.up / 2;
    }

    private void PlaceNewObject()
    {
        if (Input.GetKeyDown(_newObjectkey))
            if (_currentPlaceableObject == null)
            {
                var playerPos = transform.position;
                var playerDirection = transform.forward;

                var spawnPos = playerPos + playerDirection * _spawnDistance;
                _currentPlaceableObject = Instantiate(_placeableObjectPrefab, spawnPos, Quaternion.identity);
            }
            else
            {
                Destroy(_currentPlaceableObject);
            }
    }
}