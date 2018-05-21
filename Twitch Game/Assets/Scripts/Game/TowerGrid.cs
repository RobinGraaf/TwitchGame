using UnityEngine;

public class TowerGrid : MonoBehaviour
{
    [SerializeField] private float _sizeX = 1.0f;
    [SerializeField] private float _sizeZ = 1.0f;
    private float _sizeY = 1.0f;

    public Vector3 GetNearestPointOnGrid(Vector3 pos)
    {
        pos -= transform.position;

        var xCount = Mathf.RoundToInt(pos.x / _sizeX);
        var yCount = Mathf.RoundToInt(pos.y / _sizeY);
        var zCount = Mathf.RoundToInt(pos.z / _sizeZ);

        var result = new Vector3(
            xCount * _sizeX,
            yCount * _sizeY,
            zCount * _sizeZ);

        result += transform.position;

        return result;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        for (float x = -40; x < 40; x += _sizeX)
        for (float z = -40; z < 40; z += _sizeZ)
        {
            var point = GetNearestPointOnGrid(new Vector3(x, 0f, z));
            Gizmos.DrawSphere(point, 0.1f);
        }
    }
}