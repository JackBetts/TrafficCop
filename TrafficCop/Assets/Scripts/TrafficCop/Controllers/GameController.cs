using System;
using System.Collections;
using TrafficCop.GameUI;
using TrafficCop.Utility;
using UnityEngine;

namespace TrafficCop.Controllers
{
    public class GameController : MonoBehaviour
    { 
        public static GameController Instance;

        public delegate void CompletedLevel(bool success);
        public CompletedLevel OnCompletedLevel;

        public bool shouldCheckForWin = true; 

        [Header("Level Settings")] public int levelNumber; 
        public float oneStarTime;
        public float twoStarTime;
        public float threeStarTime;

        [Space] public float maxForwardValue;
        public float maxBackwardsValue;
        public float maxLeftValue;
        public float maxRightValue; 

        public EndGameUiPanel endGameUi; 
        
        private float startTime;
        private float responseTime;
        private bool lostGame = false; 
        
        
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

        public void OnLevelComplete(bool success)
        {
            responseTime = Time.time - startTime;
            if (success)
            {
                endGameUi.gameObject.SetActive(true);
                endGameUi.Init(true);
            }
            else
            {
                StartCoroutine(GameLost());
            }
        }

        IEnumerator GameLost()
        {
            lostGame = true; 
            yield return new WaitForSeconds(1);
            GlobalFader.Instance.ActionFade(1, 1, () =>
            {
                endGameUi.gameObject.SetActive(true);
                endGameUi.Init(false); 
            });
        }

        public int StarsWon()
        {
            if (lostGame) return 0;
            
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
