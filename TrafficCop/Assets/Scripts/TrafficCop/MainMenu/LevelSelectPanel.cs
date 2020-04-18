using System.Collections;
using UnityEngine;

namespace TrafficCop.MainMenu
{
    public class LevelSelectPanel : MonoBehaviour
    {
        public GameObject[] levelButtons;

        public void SlowActivateLevelButtons()
        {
            StartCoroutine(CoSlowActivateLevelButtons()); 
        }

        IEnumerator CoSlowActivateLevelButtons()
        {
            for (int i = 0; i < levelButtons.Length; i++)
            {
                levelButtons[i].SetActive(true);
                yield return new WaitForSeconds(.05f);
            }
        }
    }
}
