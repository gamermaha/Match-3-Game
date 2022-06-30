using eeGames.Widget;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : Widget
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
        WidgetManager.Instance.Pop(WidgetName.GameOver);
        WidgetManager.Instance.Push(WidgetName.ScrollView);
    }
    #endregion
}
