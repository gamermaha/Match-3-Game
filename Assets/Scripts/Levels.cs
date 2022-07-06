using eeGames.Widget;
using UnityEngine;
using UnityEngine.UI;

public class Levels : Widget
{
    private ScrollRect _scrollRect;
    #region UNity Methods
    
    public void SetPosition(int currentLevel)
    {
        _scrollRect = GetComponent<ScrollRect>();
        if (_scrollRect != null)
        {
            _scrollRect.verticalNormalizedPosition = currentLevel * 0.05f;
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
    
    public void OnLevel5ButtonClick()
    {
        GameManager.Instance.SetLevelValue(5);
    }
    
    public void OnLevel6ButtonClick()
    {
        GameManager.Instance.SetLevelValue(6);
    }
    
    public void OnLevel7ButtonClick()
    {
        GameManager.Instance.SetLevelValue(7);
    }
    
    public void OnLevel8ButtonClick()
    {
        GameManager.Instance.SetLevelValue(8);
    }

    public void LoadPlayGameScreen()
    {
        WidgetManager.Instance.Push(WidgetName.PlayGame);
    }
    #endregion
}
