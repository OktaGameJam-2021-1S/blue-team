using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SelectionView : MonoBehaviour, IView
{
    #region View Data
    
    public TransitionType ShowType => TransitionType.TimeLine;
    public Object ShowData => Show;
    public TransitionType HideType => TransitionType.TimeLine;
    public Object HideData => Hide;
    
    #endregion
    #region Animation
    
    [Header("Show Animation")]
    public PlayableDirector Show;
    [Header("Hide Animation")]
    public PlayableDirector Hide;

    #endregion

    public void SetupDependency(GameSessionManager session, UIViewController uiViewController, UIViewManager uiViewManager)
    {
        
    }
    
    public void DisposeMemory()
    {

    }
}
