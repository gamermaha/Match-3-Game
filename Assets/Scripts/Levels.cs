using eeGames.Widget;
using UnityEngine;
using UnityEngine.UI;

public class Levels : Widget
{
    
    [SerializeField] private Button level1Button;
    #region UNity Methods
    protected override void Awake()
    {
        level1Button.onClick.AddListener(OnLevel1ButtonClick);
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
