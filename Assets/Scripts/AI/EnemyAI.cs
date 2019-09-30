using System.Collections;
using System.Collections.Generic;
using TowerDefense.Controllers;
using UnityEngine;

namespace TowerDefense.AI
{
    public class EnemyAI : MonoBehaviour
    {
        private GlideController controller;

        private LinkedListNode<Vector3> head;

        public void SetPath(LinkedList<Vector3> path)
        {
            head = path.First;
        }

        // Start is called before the first frame update
        void Start()
        {
            controller = GetComponent<GlideController>();
        }

        // Update is called once per frame
        void Update()
        {
            if (head == null || controller == null) return;

            controller.Destination = head.Value;

            // TODO: is there  cleaner way to do this?

            // if not arrived yet, then just return
            if ((Mathf.Abs(head.Value.x - transform.position.x) >= float.Epsilon) || (Mathf.Abs(head.Value.y - transform.position.y) >= float.Epsilon)) return;

            // move to next node
            head = head.Next;

            if(head != null)
            {
                controller.Destination = head.Value;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.CompareTag("Finish"))
            {
                EventManager.TriggerEvent("RemoveEnemy");
                Destroy(gameObject);
            }
        }
    }
}