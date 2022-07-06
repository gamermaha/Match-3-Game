using System;
using System.Collections;
using System.Collections.Generic;
using eeGames.Widget;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Levels : Widget
{
    
    [SerializeField] private Button level1Button;
    [SerializeField] private Button level2Button;
    [SerializeField] private Button level3Button;
    [SerializeField] private Button level4Button;

    [SerializeField] private TextMeshProUGUI text;
    

    private int _currentLevel = 1;
    private ScrollRect _scrollRect;
    #region UNity Methods
    protected override void Awake()
    {
        // level1Button.onClick.AddListener(OnLevel1ButtonClick);
        // level2Button.onClick.AddListener(OnLevel2ButtonClick);
        // level3Button.onClick.AddListener(OnLevel3ButtonClick);
        // level4Button.onClick.AddListener(OnLevel4ButtonClick);
        //scrollRect = GetComponent<ScrollRect>();
    }

    public void SetPosition(int currentLevel)
    {
        _scrollRect = GetComponent<ScrollRect>();
        if (_scrollRect != null)
        {
            _scrollRect.verticalNormalizedPosition = currentLevel * 0.04f;
            Debug.Log(_scrollRect.verticalNormalizedPosition);
        }
        Transform level = transform.GetChild(0);
        for (int i = 1; i <= currentLevel; i++)
        {
            if(!level.Find("Level " + i).GetComponentInChildren<Button>().interactable)
                level.Find("Level " + i).GetComponentInChildren<Button>().interactable = true;
        }
    }
    
    void OnDestroy()
    {
        // level1Button.onClick.RemoveListener(OnLevel1ButtonClick);
        // level2Button.onClick.RemoveListener(OnLevel2ButtonClick);
        // level3Button.onClick.RemoveListener(OnLevel3ButtonClick);
        // level4Button.onClick.RemoveListener(OnLevel4ButtonClick);
        base.DestroyWidget();
    }

    #endregion

    #region Helper Methods
    public void OnLevel1ButtonClick()
    {
        GameManager.Instance.SetLevelValue(1);
    }
    
    public void OnLevel2ButtonClick()
    {
        GameManager.Instance.SetLevelValue(2);
    }
    
    public void OnLevel3ButtonClick()
    {
        GameManager.Instance.SetLevelValue(3);
    }
    
    public void OnLevel4ButtonClick()
    {
        GameManager.Instance.SetLevelValue(4);
    }

    public void LoadPlayGameScreen()
    {
        WidgetManager.Instance.Push(WidgetName.PlayGame);
    }

    public void ShowNotLoadingText()
    {
        text.color = Color.black;
    }
    #endregion
}
