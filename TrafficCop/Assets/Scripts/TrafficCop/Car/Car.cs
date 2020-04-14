using System;
using System.Collections;
using InputSamples.Gestures;
using UnityEngine;

namespace TrafficCop.Car
{
    public class Car : MonoBehaviour
    {
        public float moveSpeed; 
        public float lateralMoveDistance;
        public float forwardMoveDistance; 
        
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
            Debug.Log("Car Received swipe: X: " + direction.x + " Y: " + direction.y);

            StartCoroutine(MoveCar(new Vector3(direction.x * lateralMoveDistance, 0, direction.y * forwardMoveDistance)));
        }

        private IEnumerator MoveCar(Vector3 direction)
        {
            isMoving = true; 
            
            originPosition = transform.position; 
            Vector3 targetPosition = transform.position + direction;
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

        private bool CheckCanMove(Vector2 direction)
        {
            Vector3 rayDirection = new Vector3(direction.x, 0, direction.y);
            Vector3 rayOrigin = transform.position;
            rayOrigin.y += 2;

            Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * 10);
            if (Physics.Raycast(rayOrigin, rayOrigin + rayDirection, out RaycastHit hit, 10))
            {
                Debug.Log(hit.collider.name); 
                if (hit.collider.GetComponent<Car>())
                {
                    return false;
                }
            }

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
