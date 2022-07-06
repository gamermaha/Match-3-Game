using eeGames.Widget;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleted : Widget
{
    [SerializeField] private Button crossButton;
    [SerializeField] private Button nextLevelButton;
    #region UNity Methods
    protected override void Awake()
    {
        crossButton.onClick.AddListener(OnCrossButtonClick);
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
    }
    
    void OnDestroy()
    {
        crossButton.onClick.RemoveListener(OnCrossButtonClick);
        nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClick);
        base.DestroyWidget();
    }

    #endregion

    #region Helper Method
    void OnCrossButtonClick()
    {
        WidgetManager.Instance.Push(WidgetName.ScrollView);
        GameManager.Instance.CallLevelManagerToUpdateLevelOnLevelsMenu();
    }

    void OnNextLevelButtonClick()
    {
        GameManager.Instance.SetLevelValue(0);
        WidgetManager.Instance.Push(WidgetName.PlayGame);
    }
    #endregion
}
