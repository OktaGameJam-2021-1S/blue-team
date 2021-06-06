using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class ListRoomView : MonoBehaviourPunCallbacks, IView
{
    #region Attributes

    public Button _back;
    public Transform Root;
    public GameObject RoomEntryUIPrefab;
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

    private GameSessionManager _session;
    private UIViewController _viewController;
    private UIViewManager _viewManager;
    
    public Dictionary<string, RoomInfo> cachedRoomList = new Dictionary<string, RoomInfo>();
    public void SetupDependency(GameSessionManager session, UIViewController uiViewController, UIViewManager uiViewManager)
    {
        this._session = session;
        this._viewController = uiViewController;
   
        
        EnableInteraction();
        
        _back.onClick.RemoveAllListeners();
        _back.onClick.AddListener(delegate
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveRoom();
            }
            
            var selectionView = _viewManager.GetScreen<SelectionView>();
            selectionView.SetupDependency(_session, _viewController, _viewManager);
            _viewController.OpenSingleView(selectionView, _viewManager.ViewData);
        });
        
    }


    private void DisableInteraction()
    {

    }

    private void EnableInteraction()
    {

    }
    public void DisposeMemory()
    {

    }
}
