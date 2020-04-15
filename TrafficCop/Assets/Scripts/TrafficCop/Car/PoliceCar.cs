using System;
using UnityEngine; 

namespace TrafficCop.Car
{
    public class PoliceCar : Car
    {
        public Transform raycastStart;
        public float raycastLength;

        private bool hasWon;
        private void Start()
        {
            //InvokeRepeating(nameof(CheckForWin), 1, 1);
        }

        private void Update()
        {
            if (CheckForWin() && !hasWon)
            {
                hasWon = true;
                Debug.Log("Check for win :: WE WIN WOOOOO"); 
            }
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
