using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Map/MapData")]
public class MapData : ScriptableObject
{
    public float tileSize = 1f;

    // юс╫ц
    [TextArea(5, 20)]
    public string mapLayout =
        "WWWWWWWWWW\n" +
        "WFFFFFFFFW\n" +
        "WFFFFFFFFW\n" +
        "WFFFFFFFFW\n" +
        "WWWWWWWWWW";


    private int _width;

    private int _height;

    public TileType[] _tiles;


    public int Width => _width;

    public int Height => _height;

    public void Parse()
    {
        string[] rows = mapLayout.Split('\n');

        _height = rows.Length;

        _width = 0;

        foreach (var row in rows)
        {
            if (row.Length > _width)
            {
                _width = row.Length;
            }
        }

        _tiles = new TileType[_width * _height];

        for (int y = 0; y < _height; y++)
        {
            string row = rows[(_height - 1) - y];

            for (int x = 0; x < _width; x++)
            {
                if (x >= row.Length)
                {
                    _tiles[y * _width + x] = TileType.Empty;

                    continue;
                }

                _tiles[y * _width + x] = CharToTileType(row[x]);
            }
        }
    }

    private TileType CharToTileType(char c)
    {
        return c switch
        {
            'F' => TileType.Floor,
            'W' => TileType.Wall,
            'B' => TileType.BaseCamp,
            'E' => TileType.MazeEntrance,
            'X' => TileType.MazeExit,
            _ => TileType.Empty,
        };
    }

    public void Init()
    {
        _tiles = new TileType[Width * Height];
    }

    public TileType GetTile(int x, int y)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
        {
            return TileType.Empty;
        }

        return _tiles[y * _width + x];
    }

    public void SetTile(int x, int y, TileType type)
    {
        if (x < 0 || x >= _width || y < 0 || y >= _height)
        {
            return;
        }

        _tiles[y * _width + x] = type;
    }

    private void OnValidate()
    {
        int size = _width * _height;

        if (_tiles == null || _tiles.Length != size)
        {
            System.Array.Resize(ref _tiles, size);
        }
    }
}
