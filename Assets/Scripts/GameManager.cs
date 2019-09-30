using System.Collections;
using System.Collections.Generic;
using TowerDefense.AI;
using TowerDefense.AI.Navigation;
using TowerDefense.ScriptableTiles;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using UnityEngine.Events;

namespace TowerDefense
{
    public class GameManager : MonoBehaviour
    {
        public Tilemap tilemap;
        public  GameObject spawnPoint;
        public GameObject exitPoint;
        public GameObject[] enemies;
        public int maxEnemiesAlive;
        public int totalEnemies;
        public int enemiesPerSpawn;

        private int enemiesAlive = 0;

        private List<Vector2Int> buildLocations = new List<Vector2Int>();
        private LinkedList<Vector3> navigationPath;

        private Vector2Int startPoint;
        private Vector2Int endPoint;

        private UnityAction removeEnemyListener;

        private void Awake()
        {
            removeEnemyListener = new UnityAction(RemoveEnemy);
        }

        // Start is called before the first frame update
        void Start()
        {
            // can't do anything without a tilemap
            if (tilemap == null) return;

            // if there is something wrong with navigation we should return
            if (!InitalizeNavigation()) return;

            // all good, let's spawn an enemy
            // we would probably do a co-routine for this normally
            SpawnEnemy();
        }



        private void OnEnable()
        {
            EventManager.StartListening("RemoveEnemy", removeEnemyListener);
        }

        private void OnDisable()
        {
            EventManager.StopListening("RemoveEnemy", removeEnemyListener);
        }

        #region Navigation
        bool InitalizeNavigation()
        {
            var navGrid = BuildNavigationGrid();

            var navigation = new AStar();
            navigation.Initialize(navGrid);

            // normalize both the start point and end points for the AStar search
            startPoint = NormalizePoint(tilemap.WorldToCell(spawnPoint.transform.position));
            endPoint = NormalizePoint(tilemap.WorldToCell(exitPoint.transform.position));

            // ask A* for our path
            var path = navigation.FindPath(startPoint, endPoint);
            if (path == null) return false;

            // A* gives us a path that is actually reversed, so we need to reverse it to something we can use
            // We will also get the center of each tile in the process to aid navigation
            navigationPath = new LinkedList<Vector3>();
            foreach (var tileCenter in GetTileCenters(path))
            {
                navigationPath.AddLast(tileCenter);
            }

            return true;
        }

        private Vector2Int NormalizePoint(Vector3Int p)
        {
            return new Vector2Int(p.x - tilemap.cellBounds.x, p.y - tilemap.cellBounds.y);
        }

        private IEnumerable<Vector3> GetTileCenters(IEnumerable<Vector2Int> path)
        {
            foreach(var node in path)
            {
                yield return tilemap.GetCellCenterWorld(new Vector3Int(node.x + tilemap.cellBounds.xMin, node.y + tilemap.cellBounds.yMin, 0));
            }
        }

        private bool[,] BuildNavigationGrid()
        {
            var bounds = tilemap.cellBounds;
            var cells = tilemap.GetTilesBlock(bounds);

            var walkableGrid = new bool[bounds.size.x, bounds.size.y];

            for (int x = 0; x < bounds.size.x; x++)
            {
                for (int y = 0; y < bounds.size.y; y++)
                {
                    var cell = cells[x + y * bounds.size.x];


                    if (cell is BuildTile)
                    {
                        buildLocations.Add(new Vector2Int(x + bounds.xMin, y + bounds.yMin));
                    }
                    else if (cell is WalkableTile)
                    {
                        walkableGrid[x, y] = true;
                    }
                }
            }

            return walkableGrid;
        }
        #endregion

        #region Enemy Spawning
        private void SpawnEnemy()
        {
            if (enemiesPerSpawn > 0 && enemiesAlive < totalEnemies)
            {
                for (var i = 0; i < enemiesPerSpawn; i++)
                {
                    if (enemiesAlive < maxEnemiesAlive)
                    {
                        enemiesAlive++;

                        var enemy = AddEnemyToMap();
                        enemy.SetPath(navigationPath);
                    }
                }
            }
        }

        private EnemyAI AddEnemyToMap()
        {
            var enemyInstance = Instantiate(enemies[1]);

            // move to spawn point
            enemyInstance.transform.position = navigationPath.First.Value;
            return enemyInstance.GetComponent<EnemyAI>();
        }
        #endregion

        #region Listeners
        private void RemoveEnemy()
        {
            Debug.Log("Remove enemy!");
            if (enemiesAlive > 0) enemiesAlive--;
        }
        #endregion
    }

}