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
        private bool _canMove = true; 
        private bool _isMoving = false;
        private Vector3 _targetPosition;

        private void OnEnable()
        {
            GameController.Instance.OnStopCars += SetCarEnabled; 
        }

        private void OnDisable()
        {
            GameController.Instance.OnStopCars -= SetCarEnabled; 
        }

        public void SetTargetPosition(Vector3 position)
        {
            if (!_canMove) return; 
            _targetPosition = position; 
        }

        public void SetCarMoving(bool active) => _isMoving = active; 
        public void SetCarEnabled(bool active) => _canMove = active; 
        
        private void Update()
        {
            if (!_canMove) return;
            
            //Move the car forwards a set speed
            if (GameController.Instance.isEndlessMode) 
            {
                float step = moveSpeed * Time.deltaTime;
                transform.position += Vector3.forward * step;   
            }
            
            if(_isMoving)
            {
                float distance = Vector3.Distance(transform.position, _targetPosition);
                if (distance >= 1)
                {
                    float step = moveSpeed * Time.deltaTime;
                    transform.position = Vector3.MoveTowards(transform.position, _targetPosition, step);
                }   
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!_canMove) return; 
            if (other.collider.GetComponent<Car>())
            {
                SetCarMoving(false);
                StopAllCoroutines();

                //Explode
                if (explosionPrefab)
                {
                    Instantiate(explosionPrefab, other.GetContact(0).point, Quaternion.identity); 
                }

                GameController.Instance.shouldCheckForWin = false; 
                GameController.Instance.OnStopCars?.Invoke(false);
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
