using TMPro;
using TrafficCop.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace TrafficCop.GameUI
{
    public class EndGameUiPanel : MonoBehaviour
    {
        public TextMeshProUGUI responseTimeText;
        public TextMeshProUGUI levelNumberText;
        public TextMeshProUGUI levelCompletedText;
        public Button nextLevelButton;

        [Space]
        
        public Image starImage_1;
        public Image starImage_2;
        public Image starImage_3;

        public Sprite emptyStarSprite;
        public Sprite fullStarSprite;
        public Color starGoldColor;

        public void Init(bool levelCompleted)
        {
            GameController controller = GameController.Instance;
            
            levelNumberText.text = "LEVEL " + controller.levelNumber;
            if (levelCompleted)
            {
                responseTimeText.text = controller.GetResponseTime() + "s";
                levelCompletedText.text = "COMPLETE!";
                nextLevelButton.interactable = true;
            }
            else
            {
                levelCompletedText.text = "FAILED";
                responseTimeText.text = "FAILED";
                nextLevelButton.interactable = false; 
            }

            starImage_1.sprite = controller.StarsWon() >= 1 ? fullStarSprite : emptyStarSprite;
            starImage_2.sprite = controller.StarsWon() >= 2 ? fullStarSprite : emptyStarSprite;
            starImage_3.sprite = controller.StarsWon() >= 3 ? fullStarSprite : emptyStarSprite;
            
            starImage_1.color = controller.StarsWon() >= 1 ? starGoldColor : Color.white;
            starImage_2.color = controller.StarsWon() >= 2 ? starGoldColor : Color.white;
            starImage_3.color = controller.StarsWon() >= 3 ? starGoldColor : Color.white;
        }
    }
}
