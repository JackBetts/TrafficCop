using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TrafficCop.Cameras
{
    public class HelicopterCamera : MonoBehaviour
    {
        public Transform lookTarget;

        public float cameraMoveSpeed; 
        public float maxX;
        public float minX;
        public float maxZ;
        public float minZ;
        public float minDistToTarget; 
        
        private Vector3 currentTargetPos;

        private void Start()
        {
            PickNewRandomPosition();
        }

        private void LateUpdate()
        {
            //Look at the desired location
            transform.LookAt(lookTarget);
        }

        private Vector3 RandomPosition()
        {
            float x = NewXPosition();
            float y = transform.position.y;
            float z = NewZPosition(); 
            
            return new Vector3(x, y, z);
        }

        private float NewXPosition()
        {
            return Random.Range(minX, maxX); 
        }

        private float NewZPosition()
        {
            return Random.Range(minZ, maxZ); 
        }

        IEnumerator MoveToPosition(Vector3 pos)
        {
            while (true)
            {
                Vector3 position = transform.position;
                float dist = Vector3.Distance(position, pos);
                if (dist > minDistToTarget)
                {
                    transform.position = Vector3.MoveTowards(position, pos, cameraMoveSpeed * Time.deltaTime);   
                }
                else
                {
                    break; 
                }

                yield return null; 
            }
            
            yield return new WaitForEndOfFrame();
            PickNewRandomPosition();
        }
        
        private void PickNewRandomPosition() => StartCoroutine(MoveToPosition(RandomPosition()));
    }
}
