using UnityEngine;

namespace TrafficCop.Road
{
    public class RoadSegment : MonoBehaviour
    {
        public float speed;
        public Vector3 roadStart;
    
    
        void Update()
        {
            Transform _transform = transform;
            Vector3 pos = _transform.position;
        
            _transform.Translate(speed * Time.deltaTime * Vector3.right);
            if (pos.z <= -45)
            {
                _transform.position = roadStart; 
            }
        }
    }
}
