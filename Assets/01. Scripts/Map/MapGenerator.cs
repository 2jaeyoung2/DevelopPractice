using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private MapData mapData;

    [SerializeField]
    private TilePrefabData tilePrefabData;

    [SerializeField]
    private Transform tileParent;

    // TODO: 나중에 명시적으로 바꿔주기
    private readonly Dictionary<TileType, Queue<GameObject>> _pool = new();

    private readonly List<GameObject> _activeTiles = new();


    private void Start()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        ClearMap();

        for (int y = 0; y < mapData.height; y++)
        {
            for (int x = 0; x < mapData.width; x++)
            {
                TileType type = mapData.GetTile(x, y);

                if (type == TileType.Empty)
                {
                    continue;
                }

                GameObject prefab = tilePrefabData.GetPrefab(type);

                if (prefab == null)
                {
                    continue;
                }

                GameObject tile = GetFromPool(type, prefab);

                tile.transform.SetParent(tileParent);

                tile.transform.position = new Vector3(x * mapData.tileSize, 0f, y * mapData.tileSize);

                tile.SetActive(true);

                _activeTiles.Add(tile);
            }
        }
    }

    private void ClearMap()
    {
        foreach (var tile in _activeTiles)
        {
            TileType type = GetTileTypeFromObject(tile);

            ReturnToPool(type, tile);
        }

        _activeTiles.Clear();
    }

    private GameObject GetFromPool(TileType type, GameObject prefab)
    {
        if (_pool.TryGetValue(type, out var queue) == true && queue.Count > 0)
        {
            return queue.Dequeue();
        }

        return Instantiate(prefab);
    }

    private void ReturnToPool(TileType type, GameObject obj)
    {
        obj.SetActive(false);

        if (_pool.ContainsKey(type) == false)
        {
            _pool[type] = new Queue<GameObject>();
        }

        _pool[type].Enqueue(obj);
    }

    // Tile type 역추적용
    private TileType GetTileTypeFromObject(GameObject obj)
    {
        var tracker = obj.GetComponent<TileTracker>();

        return tracker != null ? tracker.tileType : TileType.Floor;
    }
}
