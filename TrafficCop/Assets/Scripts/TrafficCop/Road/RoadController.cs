using System;
using UnityEngine;

namespace TrafficCop.Road
{
    public class RoadController : MonoBehaviour
    {
        public GameObject[] roadPieces;
        public float roadLength = 5f;
        public float roadSpeed = 20f;

        private void Update()
        {
            foreach (GameObject road in roadPieces)
            {
                Vector3 newPos = road.transform.position;
                newPos.z -= roadSpeed * Time.deltaTime;

                if (newPos.z < -roadLength / 2)
                {
                    newPos.z += roadLength;
                }

                road.transform.position = newPos; 
            }
        }
    }
}
