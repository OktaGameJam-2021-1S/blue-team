using System;
using System.Collections;
using System.Collections.Generic;
using GG.StaticData;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.UI;

public partial class UIViewController : MonoBehaviour
{
    public void OpenSingleView(IView view, ViewData viewData, float percent = 1f)
    {
        StartCoroutine(OpenSingleViewRoutine(view, viewData, percent));
    }
    public IEnumerator OpenSingleViewRoutine(IView pView, ViewData pViewData, float pPercent = 1f)
    {
        if(pViewData.HandleTransitionCoroutine != null)
            StopCoroutine(pViewData.HandleTransitionCoroutine);
        
        IView currentView = null;
        while(pViewData.StackViews.Count > 0){
            currentView = pViewData.StackViews.Pop();
            HandleStateWhenCloseView(currentView);
            // Don't make the transition between the last one and null 
            if(pViewData.StackViews.Count > 1)
                yield return pViewData.HandleTransitionCoroutine = StartCoroutine(UITransition.TransitionView(currentView, null, pPercent));
        }
        
        yield return pViewData.HandleTransitionCoroutine = StartCoroutine(UITransition.TransitionView(currentView, pView, pPercent));

        pViewData.StackViews.Push(pView);
    }
    public IEnumerator OpenAdditiveViewRoutine(IView pView, ViewData pViewData, float pPercent = 1f)
    {
        if(pViewData.HandleTransitionCoroutine != null)
            StopCoroutine(pViewData.HandleTransitionCoroutine);
        
        yield return pViewData.HandleTransitionCoroutine = StartCoroutine(UITransition.TransitionView(null, pView, pPercent));
        
        pViewData.StackViews.Push(pView);
    }
    public IEnumerator CloseLastAdditiveViewRoutine(ViewData pViewData, float pPercent = 1f)
    {
        if(pViewData.HandleTransitionCoroutine != null)
            StopCoroutine(pViewData.HandleTransitionCoroutine);
        
        var closeView = pViewData.StackViews.Pop();
        
        HandleStateWhenCloseView(closeView);
        
        yield return pViewData.HandleTransitionCoroutine = StartCoroutine(UITransition.TransitionView(closeView, null, pPercent));
    }
    public void HandleStateWhenCloseView(IView pView)
    {
        pView.DisposeMemory();
    }
}