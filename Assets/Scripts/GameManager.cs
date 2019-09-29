using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private GameObject spawnPoint;

    [SerializeField]
    private GameObject[] enemies;

    [SerializeField]
    private int maxEnemiesAlive;

    [SerializeField]
    private int totalEnemies;

    [SerializeField]
    private int enemiesPerSpawn;

    private int enemiesAlive = 0;

    private List<Vector2Int> buildLocations = new List<Vector2Int>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (tilemap != null)
        {
            // get build spots
            var bounds = tilemap.cellBounds;
            var cells = tilemap.GetTilesBlock(bounds);

            Debug.Log("Bounds: ");
            Debug.Log(bounds);
            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    var cell = cells[x + y * bounds.size.x];

                    if (cell is BuildTile)
                    {
                        Debug.Log("Found build tile");
                        Debug.Log(cell);

                        var position = new Vector2Int(x + bounds.xMin, y + bounds.yMin);

                        Debug.Log("Position");
                        Debug.Log(position);

                    } else if(cell is WalkableTile)
                    {
                        Debug.Log("Found walkable tile");
                        Debug.Log(cell);

                        var position = new Vector2Int(x + bounds.xMin, y + bounds.yMin);

                        Debug.Log("Position");
                        Debug.Log(position);
                    }
                }
            }

        }

        //SpawnEnemy();
    }

    void SpawnEnemy()
    {
        if (enemiesPerSpawn > 0 && enemiesAlive < totalEnemies)
        {
            for (var i = 0; i < enemiesPerSpawn; i++)
            {
                if (enemiesAlive < maxEnemiesAlive)
                {
                    var newEnemey = Instantiate(enemies[0]);

                    newEnemey.transform.position = spawnPoint.transform.position;
                    enemiesAlive++;
                }
            }
        }
    }

    public void removeEnemy()
    {
        if (enemiesAlive > 0) enemiesAlive--;
    }
}
