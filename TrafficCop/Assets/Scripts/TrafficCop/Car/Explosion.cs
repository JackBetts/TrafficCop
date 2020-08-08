using UnityEngine;

namespace TrafficCop.Car
{
    public class Explosion : MonoBehaviour
    {
        [Header("Explosion Settings")] public ParticleSystem explosionFx;
        public float radius;
        public float force; 
        
        
        public void Awake()
        {
            Explode(); 
        }

        private void Explode()
        {
            explosionFx.Play();
            RaycastHit[] hit = Physics.SphereCastAll(transform.position, radius, Vector3.forward);
            foreach (RaycastHit raycastHit in hit)
            {
                if (raycastHit.collider.GetComponent<Car>())
                {
                    Rigidbody carRb = raycastHit.collider.GetComponent<Rigidbody>(); 
                    carRb.AddExplosionForce(force, transform.position, radius);
                }
            }
        }
    }
}
