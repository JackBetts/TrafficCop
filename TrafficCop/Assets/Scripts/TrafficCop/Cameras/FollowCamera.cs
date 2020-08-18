using UnityEngine;

namespace TrafficCop.Cameras
{
    public class FollowCamera : MonoBehaviour
    {
        public Transform target;
        public Vector3 offset;
        public float moveSpeed; 
    
        private void LateUpdate()
        {
            Vector3 position = target.position;
            position += offset;

            transform.position = Vector3.MoveTowards(transform.position, position, moveSpeed * Time.deltaTime); 
        }
    }
}
