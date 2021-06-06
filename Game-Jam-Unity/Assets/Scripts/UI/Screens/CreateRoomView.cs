using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class CreateRoomView: MonoBehaviourPunCallbacks, IView
{
    #region Attributes

    public Button _back;
    
    [SerializeField] private Button _createRoom;
    
    [SerializeField] private TMP_InputField RoomNameInputField;
    
    [SerializeField] private GameObject Loading;

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

        Loading.SetActive(false);
        EnableInteraction();
        
        _createRoom.onClick.RemoveAllListeners();
        _createRoom.onClick.AddListener(OnCreateRoomButtonClicked);
        
        _back.onClick.RemoveAllListeners();
        _back.onClick.AddListener(delegate
        {
            if (PhotonNetwork.InLobby)
            {
                PhotonNetwork.LeaveLobby();
            }
            
            var selectionView = _viewManager.GetScreen<SelectionView>();
            selectionView.SetupDependency(_session, _viewController, _viewManager);
            _viewController.OpenSingleView(selectionView, _viewManager.ViewData);
        });
    }
    

    public void OnCreateRoomButtonClicked()
    {
        DisableInteraction();
        
        Loading.SetActive(true);
        
        string roomName = RoomNameInputField.text;
        roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

        byte maxPlayers = 2;
        
        RoomOptions options = new RoomOptions {MaxPlayers = maxPlayers, PlayerTtl = 10000 };

        PhotonNetwork.CreateRoom(roomName, options, null);
    }
    #region PUN CALLBACKS
    public override void OnJoinedRoom()
    {
        // joining (or entering) a room invalidates any cached lobby room list (even if LeaveLobby was not called due to just joining a room)


        var selectionView = _viewManager.GetScreen<InsideRoomView>();
        selectionView.SetupDependency(_session, _viewController, _viewManager);
        _viewController.OpenSingleView(selectionView, _viewManager.ViewData);
    }
    #endregion

    private void DisableInteraction()
    {
        _createRoom.interactable = false;
    }

    private void EnableInteraction()
    {
        _createRoom.interactable = true;
    }
    public void DisposeMemory()
    {

    }
}
