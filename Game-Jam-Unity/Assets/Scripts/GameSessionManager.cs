using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TransitionType
{
    None,
    TimeLine,
    Animator,
}
public interface IView
{
    TransitionType ShowType { get; }
    Object ShowData { get; }
    
    TransitionType HideType { get; }
    Object HideData { get; }

    void DisposeMemory();
}

public class ViewData
{
    public Stack<IView> StackViews = new Stack<IView>();
    public Coroutine HandleTransitionCoroutine;
}

public class GameSessionManager : MonoBehaviour
{
    public UIViewController UIViewController;
    public UIViewManager UIViewManager;

    public BackendController BackendController;

    public ServerState ServerState;
    
    public void Initialize()
    {
        UIViewManager.RegisterAllViews();
        
        UIViewManager.ViewData = new ViewData();
        UIViewManager.PopUpViewData = new ViewData();
    }
}
