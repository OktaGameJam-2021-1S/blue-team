using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public static class UITransition
{
    public static IEnumerator ExecuteTransitionView(TransitionType pType, Object pData, float pPercent = 1f)
    {
        switch (pType)
        {
            case TransitionType.None:
                
                break;
            case TransitionType.TimeLine:
                if (pData is PlayableDirector pTimeLine)
                {
                    pTimeLine.Play();
                    while (pTimeLine.state == PlayState.Playing && pTimeLine.time/pTimeLine.duration < pPercent)
                        yield return null;
                }
                break;
        }
    }
    public static IEnumerator TransitionView(IView pSource, IView pTarget, float pPercent = 1f)
    {
        if (pSource != null)
        {
            yield return ExecuteTransitionView(pSource.HideType, pSource.HideData, pPercent);
            ((MonoBehaviour)pSource).gameObject.SetActive(false);
        }

        if (pTarget != null)
        {
            ((MonoBehaviour) pTarget).gameObject.SetActive(true);
            yield return ExecuteTransitionView(pTarget.ShowType, pTarget.ShowData, pPercent);
        }
    }
}
