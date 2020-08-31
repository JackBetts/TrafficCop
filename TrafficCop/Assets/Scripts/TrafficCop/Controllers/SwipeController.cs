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

        public Camera mainCam;
        public Vector3 touchOffset; 
        
        private Car.Car _currentCarClicked;
        private SwipeInput _lastSwipeInput;  
        private bool _isMovingCar = false; 

        private void OnEnable()
        {
            gestureController.Pressed += GestureControllerOnPressed;
            gestureController.Dragged += GestureControllerOnDragged;
            gestureController.Released += GestureControllerOnReleased;
        }

        private void OnDisable()
        {
            gestureController.Pressed -= GestureControllerOnPressed;
            gestureController.Dragged -= GestureControllerOnDragged;
            gestureController.Released -= GestureControllerOnReleased; 
        }

        private void Update()
        {
            if (!_currentCarClicked && !_isMovingCar) return; 
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(_lastSwipeInput.EndPosition);
                
            if (!Physics.Raycast(ray, out hit, 500f)) return;

            Vector3 point = hit.point;
            point.y = 0;
            _currentCarClicked.SetTargetPosition(point + touchOffset);
        }

        private void GestureControllerOnPressed(SwipeInput obj)
        {
            _lastSwipeInput = obj;

            Ray ray = mainCam.ScreenPointToRay(obj.EndPosition);
                
            if (!Physics.Raycast(ray, out RaycastHit hit, 500f)) return;
            if (hit.collider.GetComponent<Car.Car>())
            {
                _isMovingCar = true;
                Car.Car hitCar = hit.collider.GetComponent<Car.Car>();
                _currentCarClicked = hitCar; 
                Vector3 point = hit.point;
                point.y = 0;
                _currentCarClicked.SetTargetPosition(point + touchOffset);
                _currentCarClicked.SetCarMoving(true);
            }
        }
        
        private void GestureControllerOnReleased(SwipeInput obj)
        {
            if (!_currentCarClicked) return;
            _currentCarClicked.SetCarMoving(false);
            _currentCarClicked = null;
            _isMovingCar = false;  
        }
        
        private void GestureControllerOnDragged(SwipeInput obj)
        {
            if (!_currentCarClicked) return;
            _lastSwipeInput = obj; 
        }
    }
}
