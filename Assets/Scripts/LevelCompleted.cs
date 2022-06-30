using eeGames.Widget;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleted : Widget
{
    [SerializeField] private Button crossButton;
    #region UNity Methods
    protected override void Awake()
    {
        crossButton.onClick.AddListener(OnCrossButtonClick);
    }

    void OnDestroy()
    {
        crossButton.onClick.RemoveListener(OnCrossButtonClick);
        base.DestroyWidget();
    }

    #endregion

    #region Helper Method
    void OnCrossButtonClick()
    {
        WidgetManager.Instance.Push(WidgetName.ScrollView);
    }
    #endregion
}
