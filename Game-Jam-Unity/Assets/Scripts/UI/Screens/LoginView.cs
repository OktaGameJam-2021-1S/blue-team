using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class LoginView : MonoBehaviour, IView
{
    #region Attributes

    [SerializeField] private Button _loginButton;
    

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
        _loginButton.onClick.RemoveAllListeners();
        _loginButton.onClick.AddListener(delegate
        {
            uiViewController.OpenSingleView(uiViewManager.GetScreen<SelectionView>(), uiViewManager.ViewData);
        });
    }
    
    public void DisposeMemory()
    {

    }
}
