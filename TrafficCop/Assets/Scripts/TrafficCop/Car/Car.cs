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
        public float lateralMoveDistance;
        public float forwardMoveDistance;

        [Header("Audio")] public AudioSource aud;
        public AudioClip movedCarSfx;
        public AudioClip cannotMoveSfx;

        private bool canMove = true;
        private bool isMoving; 
        private Vector3 originPosition;


        private void Start()
        {
            originPosition = transform.position; 
        }

        public void CarMoveAttempt(SwipeInput input)
        {
            if (!canMove) return;
            canMove = false;  
            Vector2 direction = FinalSwipeValue(input.SwipeDirection);
            Vector3 movePos = new Vector3(direction.x * lateralMoveDistance, 0, direction.y * forwardMoveDistance);
            Vector3 checkPos = transform.position + movePos; 
            
            if (CheckCanMove(checkPos))
            {
                PlayAudioClip(movedCarSfx);
                StartCoroutine(MoveCar(movePos));
            }
            else
            {
                PlayAudioClip(cannotMoveSfx);
                canMove = true;
            }
        }

        private IEnumerator MoveCar(Vector3 direction)
        {
            isMoving = true; 
            
            originPosition = transform.position; 
            Vector3 targetPosition = originPosition + direction;
            float step = moveSpeed * Time.deltaTime; 
            
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                yield return null; 
            }
            canMove = true;
            isMoving = false;

            originPosition = transform.position; 
        }

        private IEnumerator MoveCarToDesiredLocation(Vector3 location)
        {
            isMoving = true;
            
            
            float step = moveSpeed * Time.deltaTime; 
            while (transform.position != location)
            {
                transform.position = Vector3.MoveTowards(transform.position, location, step);
                yield return null; 
            }
            
            canMove = true;
            isMoving = false; 
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.collider.GetComponent<Car>())
            {
                //Shake both the cars
                
                StopAllCoroutines();
                StartCoroutine(MoveCarToDesiredLocation(originPosition)); 
            }
        }

        public void PlayAudioClip(AudioClip clip)
        {
            aud.clip = clip;
            aud.Play();
        }

        private bool CheckCanMove(Vector3 finalPos)
        {
            Debug.Log("Check Can Move :: Final pos is : " + finalPos); 
            GameController controller = GameController.Instance;

            if (finalPos.x > controller.maxRightValue) return false;
            if (finalPos.x < controller.maxLeftValue) return false;

            if (finalPos.z > controller.maxForwardValue) return false;
            if (finalPos.z < controller.maxBackwardsValue) return false;

             return true; 
        }
        private Vector2 FinalSwipeValue(Vector2 swipeInput)
        {
            Vector2 tempInput = swipeInput;

            if (tempInput.x > -0.5f && tempInput.x < 0.5f)
            {
                tempInput.x = 0; 
            }
            if (tempInput.y > -0.5f && tempInput.y < 0.5f)
            {
                tempInput.y = 0; 
            }
            
            if (tempInput.x > 0.5)
            {
                tempInput.x = 1; 
            }
            else if (tempInput.x < -0.5)
            {
                tempInput.x = -1;
            }
            
            if (tempInput.y > 0.5)
            {
                tempInput.y = 1; 
            }
            else if (tempInput.y < -0.5)
            {
                tempInput.y = -1;
            }
            
            return tempInput; 
        }
    }
}
