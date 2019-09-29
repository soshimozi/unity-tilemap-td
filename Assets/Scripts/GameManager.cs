using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense2D
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance = null;

        public GameObject spawnPoint;
        public GameObject[] enemies;

        public int maxEnemiesAlive;
        public int totalEnemies;
        public int enemiesPerSpawn;

        private int enemiesAlive = 0;

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
            SpawnEnemy();
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
}
