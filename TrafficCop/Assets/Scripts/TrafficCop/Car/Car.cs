using System;
using System.Collections;
using InputSamples.Gestures;
using TrafficCop.Controllers;
using UnityEngine;

namespace TrafficCop.Car
{
    public class Car : MonoBehaviour
    {
        public float moveSpeed;

        [Header("Explosion Effect")]
        public GameObject explosionPrefab;

        [Header("Audio")] public AudioSource aud;
        public AudioClip movedCarSfx;
        public AudioClip cannotMoveSfx;
        
        //PRIVATE FIELDS
        private bool _isMoving = false;
        private Vector3 _targetPosition;


        public void SetTargetPosition(Vector3 position)
        {
            _targetPosition = position; 
        }

        public void SetCarActive(bool active) => _isMoving = active; 
        
        private void Update()
        {
            if (!_isMoving) return;

            float distance = Vector3.Distance(transform.position, _targetPosition);
            if (distance >= 1)
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.GetComponent<Car>())
            {
                SetCarActive(false);
                StopAllCoroutines();

                //Explode
                if (explosionPrefab)
                {
                    Instantiate(explosionPrefab, other.GetContact(0).point, Quaternion.identity); 
                }

                GameController.Instance.shouldCheckForWin = false; 
                GameController.Instance.OnCompletedLevel?.Invoke(false);
            }
        }

        private void PlayAudioClip(AudioClip clip)
        {
            aud.clip = clip;
            aud.Play();
        }
    }
}
