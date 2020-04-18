using UnityEngine;
using UnityEngine.SceneManagement;

namespace TrafficCop.Utility
{
    public class LevelLoader : MonoBehaviour
    {
        public int sceneId;

        public void StartLoadScene()
        {
            GlobalFader.Instance.HalfActionFade(() => SceneManager.LoadScene(sceneId));
        }

        public void LoadNextLevel()
        {
            int nextBuildIndex = SceneManager.GetActiveScene().buildIndex + 1;
            if (SceneManager.sceneCountInBuildSettings > nextBuildIndex)
            {
                GlobalFader.Instance.HalfActionFade(() => SceneManager.LoadScene(nextBuildIndex));
            }
            else //There is no next level - Go back to main menu
            {
                GlobalFader.Instance.HalfActionFade(() => SceneManager.LoadScene(0));
            }
        }

        public void RetryCurrentLevel()
        {
            int thisBuildIndex = SceneManager.GetActiveScene().buildIndex; 
            GlobalFader.Instance.HalfActionFade(() => SceneManager.LoadScene(thisBuildIndex));
        }
    }
}
