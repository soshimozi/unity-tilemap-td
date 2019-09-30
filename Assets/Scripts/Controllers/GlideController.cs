using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Controllers
{

    public class GlideController : MonoBehaviour
    {
        public float speed;

        [HideInInspector]
        public Vector3 Destination
        {
            get;
            set;
        }

        // Start is called before the first frame update
        void Start()
        {
            // start at our current position
            Destination = gameObject.transform.position;
        }

        private void MoveNext()
        {
            // calculate the next position
            float delta = speed * Time.deltaTime;

            var currentPosition = gameObject.transform.position;
            var nextPosition = Vector3.MoveTowards(currentPosition, Destination, delta);

            gameObject.transform.position = nextPosition;
        }

        // Update is called once per frame
        void Update()
        {
            if(Destination != gameObject.transform.position)
            {
                MoveNext();
            }
        }
    }
}
