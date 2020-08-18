using UnityEngine;

namespace TrafficCop.Road
{
    public class EndlessRoadSpawner : MonoBehaviour
    {
        public GameObject roadPrefab;
        public Transform spawnTarget; 
        
        public float roadDistance;
        public float spawnAmount; 
        void Start()
        {
            SpawnRoads();
        }

        private void SpawnRoads()
        {
            for (int i = 0; i < spawnAmount; i++)
            {
                Vector3 pos = spawnTarget.position;
                Instantiate(roadPrefab, pos, Quaternion.identity);
                pos.z += roadDistance;
                spawnTarget.position = pos; 
            }
        }
    }
}
