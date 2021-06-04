using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootController : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(RunGameBoot());
    }
    public class BootOutput
    {
        public GameSessionManager session;
    }
    IEnumerator RunGameBoot()
    {
        List<IEnumerator> BootProcessSteps = new List<IEnumerator>();
        BootOutput tOutput = new BootOutput();
        
        BootProcessSteps.Add(Initialize(tOutput));
        BootProcessSteps.Add(AutoLogin());
        BootProcessSteps.Add(DownloadStaticData(tOutput));
        BootProcessSteps.Add(DownloadResources());
        BootProcessSteps.Add(StartGame(tOutput));

        for (int i = 0; i < BootProcessSteps.Count; i++)
        {
            yield return StartCoroutine(BootProcessSteps[i]);
        }
    }

    IEnumerator Initialize(BootOutput pOutput)
    {
        LeanTween.reset();
        LeanTween.init();
        
        pOutput.session = FindObjectOfType<GameSessionManager>();
        
        pOutput.session.ServerState = new ServerState();
        pOutput.session.BackendController = new BackendController();
        
        pOutput.session.Initialize();

        yield return null;
    }
    IEnumerator AutoLogin()
    {
        yield return null;
    }

    IEnumerator DownloadResources()
    {
        yield return null;
    }

    IEnumerator DownloadStaticData(BootOutput pOutput)
    {
        yield return pOutput.session.BackendController.GetStaticData(pOutput.session);
    }
    
    IEnumerator StartGame(BootOutput pOutput)
    {
        var initialView = pOutput.session.UIViewManager.GetScreen<LoginView>();
        
        initialView.SetupDependency(pOutput.session, pOutput.session.UIViewController, pOutput.session.UIViewManager);
        
        yield return StartCoroutine(pOutput.session.UIViewController.OpenSingleViewRoutine(initialView, pOutput.session.UIViewManager.ViewData));
    }
}
