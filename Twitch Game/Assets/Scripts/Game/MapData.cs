using UnityEngine;

public class MapData : Singleton<MapData>
{
    public class Tile
    {
        public Tile(int type)
        {
            IsUsed = false;
            Type = type;
        }

        public int Type { get; set; }
        public bool IsUsed { get; set; }
    }

    private int _sizeX;
    private int _sizeY;
    private Tile[,] _mapData;
    private Vector3[,] _nodes;

    private int _playerAreaSizeX;
    private int _playerAreaSizeY;

    private enum Area
    {
        EEnemy = 1,
        EPlayer = 2
    }

    public void CreateMap(int width, int height)
    {
        _sizeX = width;
        _sizeY = height;

        _playerAreaSizeX = _sizeX;
        _playerAreaSizeY = _sizeY / 4;

        _mapData = new Tile[_sizeX, _sizeY];
        _nodes = new Vector3[_playerAreaSizeX, _playerAreaSizeY];

        for (var x = 0; x < _sizeX; x++)
        for (var y = 0; y < _sizeY; y++)
            _mapData[x, y] = new Tile((int)Area.EEnemy);

        CalculatePlayerArea();
    }

    private void CalculatePlayerArea()
    {
        for (var x = 0; x < _playerAreaSizeX; x++)
        for (var y = 0; y < _playerAreaSizeY; y++)
            _mapData[x, y].Type = (int) Area.EPlayer;

        for (var x = 1; x < _playerAreaSizeX; x++)
        for (var y = 1; y < _playerAreaSizeY; y++)
            _nodes[x, y] = new Vector3(x, 0.0f, y + 0.5f);
    }

    public Tile GetTileAt(int x, int y)
    {
        return _mapData[x, y];
    }

    public Vector3 GetNodeAt(int x, int y)
    {
        return _nodes[x, y];
    }

    public Vector3 GetClosestNode(Vector3 position)
    {
        if (_mapData[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z)].Type == (int) Area.EPlayer)
            return _nodes[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z)];
        return position;
    }

    public int GetSizeX()
    {
        return _sizeX;
    }

    public int GetSizeY()
    {
        return _sizeY;
    }
}