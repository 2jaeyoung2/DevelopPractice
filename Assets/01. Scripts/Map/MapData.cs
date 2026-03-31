using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MapData", menuName = "Map/MapData")]
public class MapData : ScriptableObject
{
    public int width = 10;

    public int height = 10;

    public float tileSize = 1f;

    public TileType[] tiles;


    public void Init()
    {
        tiles = new TileType[width * height];
    }

    public TileType GetTile(int x, int y)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return TileType.Empty;
        }

        return tiles[y * width + x];
    }

    public void SetTile(int x, int y, TileType type)
    {
        if (x < 0 || x >= width || y < 0 || y >= height)
        {
            return;
        }

        tiles[y * width + x] = type;
    }

    private void OnValidate()
    {
        int size = width * height;

        if (tiles == null || tiles.Length != size)
        {
            System.Array.Resize(ref tiles, size);
        }
    }
}
