using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense2D
{
    public class EnemyAI : MonoBehaviour
    {
        public int target = 0;
        public Transform exitPoint;
        public Transform[] wayPoints;
        public float navigationUpdate;

        private Transform enemy;
        private float navigationTime = 0;

        // Start is called before the first frame update
        void Start()
        {
            enemy = GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            if (wayPoints == null) return;

            navigationTime += Time.deltaTime;
            if (navigationTime > navigationUpdate)
            {
                //Debug.Log("Updating position");

                if (target < wayPoints.Length)
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, wayPoints[target].position, navigationTime);
                    Debug.Log("moving to: ");
                    Debug.Log(enemy.position);

                }
                else
                {
                    enemy.position = Vector2.MoveTowards(enemy.position, exitPoint.position, navigationTime);
                }

                navigationTime = 0;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Checkpoint"))
            {
                Debug.Log("Updating checkpoint");

                target += 1;
            }
            else if (other.CompareTag("Finish"))
            {
                Debug.Log("Removing Enemy");
                GameManager.instance.removeEnemy();
                Destroy(gameObject);
            }
        }
    }
}
