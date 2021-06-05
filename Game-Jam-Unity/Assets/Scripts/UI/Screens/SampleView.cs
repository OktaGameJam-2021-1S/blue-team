﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SampleView : MonoBehaviour, IView
{
    #region Attributes


    #endregion
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
