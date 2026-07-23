using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CubeType
{
    Normal,
    Wall
}

public class MapGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private Transform mapRoot;

    [Header("Tile Prefabs")]
    [SerializeField]
    private GameObject normalFloorPrefab;

    [SerializeField]
    private GameObject wallFloorPrefab;

    [Header("Map Settings")]
    [SerializeField]
    private int size = 10;

    private float tileSizeX;

    private float tileSizeZ;

    private void Awake()
    {
        InitializeTileSize();

        GenerateMap();
    }

    private void InitializeTileSize()
    {
        Renderer renderer = normalFloorPrefab.GetComponentInChildren<Renderer>();

        if (renderer == null)
        {
            Debug.LogError("Normal Floor Prefab에서 Renderer를 찾을 수 없습니다.");

            enabled = false;

            return;
        }

        tileSizeX = renderer.bounds.size.x;

        tileSizeZ = renderer.bounds.size.z;
    }

    public void GenerateMap()
    {
        if (mapRoot == null)
        {
            Debug.LogError("MapRoot가 연결되지 않았습니다.");

            return;
        }

        ClearMap();

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                CubeType cubeType = (CubeType)Random.Range(0, 2);

                GameObject tile = Instantiate(GetTilePrefab(cubeType), mapRoot);

                tile.transform.localPosition = GetTilePosition(x, z);
            }
        }

        Debug.Log("Map Generated");
    }

    private Vector3 GetTilePosition(int x, int z)
    {
        float offsetX = (size - 1) * tileSizeX * 0.5f;

        float offsetZ = (size - 1) * tileSizeZ * 0.5f;

        return new Vector3(x * tileSizeX - offsetX, 0f, z * tileSizeZ - offsetZ);
    }

    private GameObject GetTilePrefab(CubeType cubeType)
    {
        switch (cubeType)
        {
            case CubeType.Normal:

                return normalFloorPrefab;

            case CubeType.Wall:

                return wallFloorPrefab;

            default:

                return normalFloorPrefab;
        }
    }

    private void ClearMap()
    {
        for (int i = mapRoot.childCount - 1; i >= 0; i--)
        {
            Destroy(mapRoot.GetChild(i).gameObject);
        }
    }
}
