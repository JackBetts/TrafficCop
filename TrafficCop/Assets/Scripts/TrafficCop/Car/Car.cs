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
        public void CarMoveAttempt(SwipeInput input)
        {
            Vector2 direction = FinalSwipeValue(input.SwipeDirection); 
            Debug.Log("Car Received swipe: X: " + direction.x + " Y: " + direction.y);

            if (CheckCanMove(direction))
            {
                StartCoroutine(MoveCar(new Vector3(direction.x * lateralMoveDistance, 0, direction.y * forwardMoveDistance)));
            }
        }

        private IEnumerator MoveCar(Vector3 direction)
        {
            Vector3 targetPosition = transform.position + direction;
            float step = moveSpeed * Time.deltaTime; 
            
            while (transform.position != targetPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                yield return null; 
            }
        }

        private bool CheckCanMove(Vector2 direction)
        {
            Vector3 rayDirection = new Vector3(direction.x, 0, direction.y);
            RaycastHit hit;
            
            Debug.DrawLine(transform.position, transform.position + rayDirection * 10);
            if (Physics.Raycast(transform.position, transform.position + rayDirection, out hit, 10))
            {
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
