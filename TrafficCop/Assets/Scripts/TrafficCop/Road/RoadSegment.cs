using UnityEngine;

namespace TrafficCop.Road
{
    public class RoadSegment : MonoBehaviour
    {
        public float speed;


        void Update()
        {
            Transform _transform = transform;
            Vector3 pos = _transform.position;
        
            _transform.Translate(speed * Time.deltaTime * Vector3.back);

            if (pos.z < -100)
            {
                Destroy(gameObject);
            }
        }
    }
}
