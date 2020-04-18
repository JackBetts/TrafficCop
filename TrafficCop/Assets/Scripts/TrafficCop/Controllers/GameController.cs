using System;
using TrafficCop.GameUI;
using UnityEngine;

namespace TrafficCop.Controllers
{
    public class GameController : MonoBehaviour
    { 
        public static GameController Instance;

        public delegate void CompletedLevel();
        public CompletedLevel OnCompletedLevel;

        [Header("Level Settings")] public int levelNumber; 
        public float oneStarTime;
        public float twoStarTime;
        public float threeStarTime; 

        public EndGameUiPanel endGameUi; 
        
        private float startTime;
        private float responseTime; 
        
        
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this; 
            }

            startTime = Time.time; 
        }

        private void OnEnable()
        {
            OnCompletedLevel += OnLevelComplete; 
        }

        private void OnDisable()
        {
            OnCompletedLevel -= OnLevelComplete; 
        }

        public void OnLevelComplete()
        {
            responseTime = Time.time - startTime; 
            
            endGameUi.gameObject.SetActive(true);
            endGameUi.Init();
        }

        public int StarsWon()
        {
            if (responseTime > oneStarTime)
            {
                return 0; 
            }
            
            if (responseTime > twoStarTime)
            {
                return 1; 
            }
            
            if (responseTime > threeStarTime)
            {
                return 2;
            }
            
            return 3; 
        }

        public float GetResponseTime()
        {
            return responseTime; 
        }
    }
}
