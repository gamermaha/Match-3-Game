using eeGames.Widget;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : Widget
{
    [SerializeField] private Button crossButton;
    [SerializeField] private Button retryButton;
    #region UNity Methods
    protected override void Awake()
    {
        crossButton.onClick.AddListener(OnCrossButtonClick);
        retryButton.onClick.AddListener(OnRetryButtonClick);
    }
    
    void OnDestroy()
    {
        crossButton.onClick.RemoveListener(OnCrossButtonClick);
        retryButton.onClick.AddListener(OnRetryButtonClick);
        base.DestroyWidget();
    }
    
    #endregion

    #region Helper Method
    void OnCrossButtonClick()
    {
        WidgetManager.Instance.Push(WidgetName.ScrollView);
        GameManager.Instance.CallLevelManagerToUpdateLevelOnLevelsMenu();
    }

    void OnRetryButtonClick()
    {
        GameManager.Instance.SetLevelValue(-1);
        WidgetManager.Instance.Push(WidgetName.PlayGame);
    }
    #endregion
}
