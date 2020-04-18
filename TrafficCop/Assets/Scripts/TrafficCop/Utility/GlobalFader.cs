using System;
using System.Collections;
using DG.Tweening;
using InputSamples.Demo.Rolling;
using UnityEngine;
using UnityEngine.Events;
using Sisus.Attributes;

using UnityEngine.UI; 

namespace TrafficCop.Utility
{
    public class GlobalFader : MonoBehaviour
    {
        public static GlobalFader Instance;
        
        [Header("Fade Settings")]
        public Image fadeImage;

        public float fadeLength;


        private void Awake()
        {
            DOTween.Init(); 
            if (Instance)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this; 
            }
        }

        private void Start()
        {
            FadeToClear();
        }
        
        [ShowInInspector]
        public void FadeToBlack()
        {
            fadeImage.DOFade(1, fadeLength);
        }

        [ShowInInspector]
        public void FadeToClear()
        {
            fadeImage.DOFade(0, fadeLength);
        }

        public void HalfActionFade(UnityAction action)
        {
            StartCoroutine(CoHalfActionFade(action)); 
        }

        IEnumerator CoHalfActionFade(UnityAction action)
        {
            FadeToBlack();
            yield return new WaitForSeconds(fadeLength);
            action.Invoke();
        }

        public void ActionFade(float fadeTime, float fadePause, UnityAction action)
        {
            StartCoroutine(CoFadeAndPerformAction(fadeTime, fadePause, action)); 
        }
        
        IEnumerator CoFadeAndPerformAction(float fadeTime, float fadePause, UnityAction action)
        {
            FadeToBlack();

            yield return new WaitForSeconds(fadeTime + .5f);  
            
            action.Invoke();

            yield return new WaitForSeconds(fadePause);

            FadeToClear();
        }
    }
}
