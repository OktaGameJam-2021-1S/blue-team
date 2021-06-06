﻿using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SelectionView : MonoBehaviourPunCallbacks, IView
{
    #region Attributes

    [SerializeField] private Button _createRoom;
    [SerializeField] private Button _roomList;

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
    public void SetupDependency(GameSessionManager session, UIViewController uiViewController, UIViewManager uiViewManager)
    {
        this._session = session;
        this._viewController = uiViewController;
        this._viewManager = uiViewManager;

        EnableInteraction();
        
        _createRoom.onClick.RemoveAllListeners();
        _createRoom.onClick.AddListener(CreateRoom);
        
        
        _roomList.onClick.RemoveAllListeners();
        _roomList.onClick.AddListener(ListRoom);
        

    }

    private void CreateRoom()
    {
        var selectionView = _viewManager.GetScreen<CreateRoomView>();
        selectionView.SetupDependency(_session, _viewController, _viewManager);
        _viewController.OpenSingleView(selectionView, _viewManager.ViewData);
    }
    private void ListRoom()
    {
        var selectionView = _viewManager.GetScreen<ListRoomView>();
        selectionView.SetupDependency(_session, _viewController, _viewManager);
        _viewController.OpenSingleView(selectionView, _viewManager.ViewData);
    }
    
    #region PUN CALLBACKS


    #endregion

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
