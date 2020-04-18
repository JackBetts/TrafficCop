using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanel : MonoBehaviour
{
    public GameObject levelSelectPanel;
    
    
    public void OpenLevelSelect()
    {
        levelSelectPanel.SetActive(true);
    }
}
