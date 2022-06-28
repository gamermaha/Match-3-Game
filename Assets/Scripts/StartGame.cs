using eeGames.Widget;
using System.Collections;
using UnityEngine;

public class StartGame : Widget
{
    #region UNity Methods
    protected override void Awake()
    {
        StartCoroutine(GotoLevelsScreen());
    }
    void OnDestroy()
    {
        base.DestroyWidget();
    }
    #endregion

    #region Helper Method
    IEnumerator GotoLevelsScreen()
    {
        yield return new WaitForSeconds(0.5f);
        WidgetManager.Instance.Push(WidgetName.Levels);
    }
    #endregion
    
}
