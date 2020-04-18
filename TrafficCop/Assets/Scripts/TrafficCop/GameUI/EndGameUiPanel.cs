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
        
        [Space]
        
        public Image starImage_1;
        public Image starImage_2;
        public Image starImage_3;

        public Sprite emptyStarSprite;
        public Sprite fullStarSprite;
        public Color starGoldColor;

        public void Init()
        {
            GameController controller = GameController.Instance;

            levelNumberText.text = "LEVEL " + controller.levelNumber; 
            responseTimeText.text = controller.GetResponseTime() + "s";

            starImage_1.sprite = controller.StarsWon() >= 1 ? fullStarSprite : emptyStarSprite;
            starImage_2.sprite = controller.StarsWon() >= 2 ? fullStarSprite : emptyStarSprite;
            starImage_3.sprite = controller.StarsWon() >= 3 ? fullStarSprite : emptyStarSprite;
            
            starImage_1.color = controller.StarsWon() >= 1 ? starGoldColor : Color.white;
            starImage_2.color = controller.StarsWon() >= 2 ? starGoldColor : Color.white;
            starImage_3.color = controller.StarsWon() >= 3 ? starGoldColor : Color.white;
        }
    }
}
