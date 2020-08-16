using System;
using UnityEngine; 
using DG.Tweening;
using TrafficCop.Controllers;
using TrafficCop.Utility;

namespace TrafficCop.Car
{
    public class PoliceCar : Car
    {
        public float endMoveDuration;
        public float endMovePos; 
        
        public Transform[] raycastStarts;
        public float raycastLength;
        public float checkWinDelay; 

        private float lastCheckTime = 0;
        private bool hasWon;

        private void Update()
        {
            if (GameController.Instance.shouldCheckForWin && Time.time > lastCheckTime + checkWinDelay)
            {
                if (CheckForWin() && !hasWon)
                {
                    OnWin();    
                }

                lastCheckTime = Time.time; 
            }
        }

        private void OnWin()
        {
            hasWon = true;
            transform.DOMoveZ(endMovePos, endMoveDuration).onComplete += OnComplete;
            GameController.Instance.OnStopCars?.Invoke(false);
        }

        private void OnComplete()
        {
            GlobalFader.Instance.ActionFade(.3f, 1, () => GameController.Instance.OnCompletedLevel?.Invoke(true));
        }

        private bool CheckForWin()
        {
            foreach (Transform raycastStart in raycastStarts)
            {
                if (Physics.Raycast(raycastStart.position, Vector3.forward, out RaycastHit hit, raycastLength))
                {
                    if (hit.collider) return false;
                }   
            }

            return true;
        }
    }
}
