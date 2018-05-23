using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class MapGraphics : MonoBehaviour {

    [SerializeField] private int _sizeX = 500;
    [SerializeField] private int _sizeZ = 500;
    [SerializeField] float _tileSize = 1.0f;

    [SerializeField] private Texture2D _texture2D;
    [SerializeField] private int _tileResolution;
    
    private int _playerAreaSizeX;
    private int _playerAreaSizeZ;

    private enum Area { EPlayer, EEnemy }
    
    [SerializeField] private GameObject _playerPrefab;

    // Use this for initialization
    void Start ()
    {
        _playerAreaSizeX = _sizeX;
        _playerAreaSizeZ = _sizeZ / 4;
        BuildMesh();
    }

    private void BuildMesh()
    {
        MapData.Instance().CreateMap(_sizeX, _sizeZ);

        int numTiles = _sizeX * _sizeZ;
        int numTriangles = numTiles * 2;

        int vSizeX = _sizeX + 1;
        int vSizeZ = _sizeZ + 1;
        int numVerts = vSizeX * vSizeZ;

        Vector3[] vertices = new Vector3[numVerts];
        Vector3[] normals = new Vector3[numVerts];
        Vector2[] uv = new Vector2[numVerts];

        int[] triangles = new int[numTriangles * 3];

        int x, z;
        for (z = 0; z < vSizeZ; z++) {
            for (x = 0; x < vSizeX; x++) {
                vertices[z * vSizeX + x] = new Vector3(x * _tileSize, 0, z * _tileSize);
                normals[z * vSizeX + x] = Vector3.up;
                uv[z * vSizeX + x] = new Vector2((float)x / _sizeX, (float)z / _sizeZ);
            }
        }

        for (z = 0; z < _sizeZ; z++) {
            for (x = 0; x < _sizeX; x++) {
                int squareIndex = z * _sizeX + x;
                int triangleOffset = squareIndex * 6;

                triangles[triangleOffset + 0] = z * vSizeX + x + 0;
                triangles[triangleOffset + 1] = z * vSizeX + x + vSizeX + 0;
                triangles[triangleOffset + 2] = z * vSizeX + x + vSizeX + 1;

                triangles[triangleOffset + 3] = z * vSizeX + x + 0;
                triangles[triangleOffset + 4] = z * vSizeX + x + vSizeX + 1;
                triangles[triangleOffset + 5] = z * vSizeX + x + 1;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uv;

        MeshFilter filter = GetComponent<MeshFilter>();
        MeshCollider collider = GetComponent<MeshCollider>();

        filter.mesh = mesh;
        collider.sharedMesh = mesh;

        BuildTexture();
    }

    private void BuildTexture()
    {
        int texWidth = _sizeX * _tileResolution;
        int texHeight = _sizeZ * _tileResolution;
        Texture2D texture = new Texture2D(texWidth, texHeight);

        Color[][] tiles = ChopUpTiles();

        for (int y = 0; y < _sizeZ; y++) {
            for (int x = 0; x < _sizeX; x++) {
                Color[] color = tiles[MapData.Instance().GetTileAt(x, y).Type];
                texture.SetPixels(x * _tileResolution, y * _tileResolution, _tileResolution, _tileResolution, color);
            }
        }

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;
        texture.Apply();

        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.sharedMaterials[0].mainTexture = texture;

        //For testing node locations
        //for (int x = 1; x < _playerAreaSizeX; x++) {
        //    for (int z = 1; z < _playerAreaSizeZ; z++) {
        //        Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), MapData.Instance().GetNodeAt(x, z),
        //            Quaternion.identity);
        //    }
        //}

        AddGameObject();
    }

    private void AddGameObject()
    {
        Instantiate(_playerPrefab, GameObject.FindWithTag("Castle").transform.position + (Vector3.forward * 4), Quaternion.identity);
    }

    private Color[][] ChopUpTiles()
    {
        int numTilesPerRow = _texture2D.width / _tileResolution;
        int numRows = _texture2D.height / _tileResolution;

        Color[][] tiles = new Color[numTilesPerRow * numRows][];

        for (int y = 0; y < numRows; y++) {
            for (int x = 0; x < numTilesPerRow; x++) {
                tiles[y * numTilesPerRow + x] = _texture2D.GetPixels(x * _tileResolution, y * _tileResolution, _tileResolution, _tileResolution);
            }
        }

        return tiles;
    }
}
