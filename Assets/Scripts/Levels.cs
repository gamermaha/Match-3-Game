using System;
using eeGames.Widget;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Levels : Widget
{
    
    [SerializeField] private Button level1Button;
    private int current_level = 17;
    private ScrollRect _scrollRect;
    #region UNity Methods
    protected override void Awake()
    {
        level1Button.onClick.AddListener(OnLevel1ButtonClick);
        //scrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        if (_scrollRect != null)
        {
            if (current_level >= 1 && current_level <= 6)
                _scrollRect.verticalNormalizedPosition = 0f;
            else if (current_level >= 7 && current_level <= 11)
                _scrollRect.verticalNormalizedPosition = 0.4925f;
            else if (current_level >= 12 && current_level <= 18)
                _scrollRect.verticalNormalizedPosition = 0.97f;
            
        }
    }

    void OnDestroy()
    {
        level1Button.onClick.RemoveListener(OnLevel1ButtonClick);
        base.DestroyWidget();
    }

    #endregion

    #region Helper Method
    void OnLevel1ButtonClick()
    {
        WidgetManager.Instance.Push(WidgetName.PlayGame);
        GameManager.Instance.StartGamePlay();
    }
    #endregion
}
