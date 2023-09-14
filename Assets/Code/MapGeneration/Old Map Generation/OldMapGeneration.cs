using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class OldMapGeneration : MonoBehaviour
{
    [System.Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;

        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }

    [SerializeField]
    private int columns;
    [SerializeField]
    private int rows;
    [SerializeField]
    private Count wallCount = new Count(5, 9);
    [SerializeField]
    private GameObject[] endpoints;
    [SerializeField]
    private GameObject[] floorTiles;
    [SerializeField]
    private GameObject[] wallTiles;
    [SerializeField]
    private GameObject[] enemyTiles;
    [SerializeField]
    private GameObject[] outerWallTiles;

    private Transform mapHolder;
    private List<Vector3> gridPositions = new List<Vector3>();
    private void InitialiseList()
    {
        gridPositions.Clear();

        for (int x = 1; x < columns-1; x++)
        {
            for (int y = 1; y < rows-1; y++)
            {
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
    }

    private void SetUpMap()
    {
        mapHolder = new GameObject("Map").transform;

        for (int x = -1; x < columns+1; x++)
        {
            for (int y = -1; y < rows + 1; y++)
            {
                GameObject toInstantiate = floorTiles[Random.Range(0, floorTiles.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstantiate = outerWallTiles[Random.Range(0, outerWallTiles.Length)];

                    GameObject instance = Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                    instance.transform.SetParent(mapHolder);
                }
            }
        }
    }

    private Vector3 RandomPosition()
    {
        int randomIndex = Random.Range(0, gridPositions.Count);
        Vector3 randomPosition = gridPositions[randomIndex];
        gridPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    private void LayoutObjectAtRandom(GameObject[] tileArray, int min, int max)
    {
        int objectCount = Random.Range(min, max + 1);
        for (int i = 0; i < objectCount; i++)
        {
            Vector3 randomPosition = RandomPosition();
            GameObject tileChoice = tileArray[Random.Range(0, tileArray.Length)];
            Instantiate(tileChoice, randomPosition, Quaternion.identity);
        }
    }

    public void SetupScene(int level)
    {
        SetUpMap();
        InitialiseList();
        LayoutObjectAtRandom(wallTiles, wallCount.minimum, wallCount.maximum);
        int enemyCount = level * 2;
        LayoutObjectAtRandom(enemyTiles, enemyCount, enemyCount);
        Instantiate(endpoints[0], new Vector3(columns - 1, rows - 1, 0f), Quaternion.identity);
    }
}
