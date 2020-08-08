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
        
        public Transform raycastStart;
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
        }

        private void OnComplete()
        {
            GlobalFader.Instance.ActionFade(.3f, 1, () => GameController.Instance.OnCompletedLevel?.Invoke(true));
        }

        private bool CheckForWin()
        {
            if (Physics.Raycast(raycastStart.position, Vector3.forward, out RaycastHit hit, raycastLength))
            {
                if (hit.collider)
                {
                    Debug.Log("Check for win :: hit collider :: " + hit.collider.name);
                    return false;
                }
            }
            
            return true;
        }
    }
}
