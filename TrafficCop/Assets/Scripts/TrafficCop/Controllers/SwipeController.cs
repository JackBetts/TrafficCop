using System;
using System.Collections.Generic;
using InputSamples.Gestures;
using UnityEngine;
using TrafficCop.Car; 

namespace TrafficCop.Controllers
{
    public class SwipeController : MonoBehaviour
    {
        [SerializeField] private GestureController gestureController; 
        private readonly Dictionary<int, Car.Car> swipeMapping = new Dictionary<int, Car.Car>();

        public Camera mainCam; 

        private void OnEnable()
        {
            gestureController.Pressed += GestureControllerOnPressed;
            gestureController.PotentiallySwiped += GestureControllerOnPotentiallySwiped;
            gestureController.Swiped += GestureControllerOnSwiped;
        }

        private void OnDisable()
        {
            gestureController.Pressed -= GestureControllerOnPressed;
            gestureController.PotentiallySwiped -= GestureControllerOnPotentiallySwiped;
            gestureController.Swiped -= GestureControllerOnSwiped;
        }

        private void GestureControllerOnSwiped(SwipeInput obj)
        {
            Car.Car swipedCar;
            if (!swipeMapping.TryGetValue(obj.InputId, out swipedCar))
            {
                return;
            }

            swipeMapping.Remove(obj.InputId);
            swipedCar.CarMoveAttempt(obj);
        }

        private void GestureControllerOnPotentiallySwiped(SwipeInput obj)
        {
           
        }

        private void GestureControllerOnPressed(SwipeInput obj)
        {
            swipeMapping.Remove(obj.InputId);

            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(obj.EndPosition);
            if (Physics.Raycast(ray, out hit, 500f))
            {
                if (hit.collider.GetComponent<Car.Car>())
                {
                    Car.Car hitCar = hit.collider.GetComponent<Car.Car>();
                    swipeMapping[obj.InputId] = hitCar; 
                }
            }
        }
    }
}
