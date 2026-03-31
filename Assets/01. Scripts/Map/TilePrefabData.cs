using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="TilePrefabData", menuName = "Map/TilePrefabData")]
public class TilePrefabData : ScriptableObject
{
    [System.Serializable]
    public struct TileEntry
    {
        public TileType tileType;

        public GameObject prefab;
    }

    public TileEntry[] entries;

    public GameObject GetPrefab(TileType type)
    {
        foreach (var entry in entries)
        {
            if (entry.tileType == type)
            {
                return entry.prefab;
            }
        }

        return null;
    }
}
