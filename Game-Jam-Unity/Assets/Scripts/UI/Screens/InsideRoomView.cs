
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class InsideRoomView : MonoBehaviourPunCallbacks, IView
{
    #region Attributes

    public Button _back;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _readyButton;

    [SerializeField] private GameObject Loading;

    [SerializeField] private Transform Root;
    
    private Dictionary<int, GameObject> playerListEntries;
    
    public GameObject PlayerListEntryPrefab;
    
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

        OnJoinedRoom();
        
        _startButton.onClick.RemoveAllListeners();
        _startButton.onClick.AddListener(OnStartGameButtonClicked);
        
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
    public void LocalPlayerPropertiesUpdated()
    {
        _startButton.gameObject.SetActive(CheckPlayersReady());
    }
    public void OnJoinedRoom()
    {

        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            GameObject entry = Instantiate(PlayerListEntryPrefab);
            entry.transform.SetParent(Root);
            entry.transform.localScale = Vector3.one;
            entry.SetActive(true);
            entry.GetComponent<PlayerRoomUI>().Initialize(p.ActorNumber, p.NickName);

            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerRoomUI>().SetPlayerReady((bool) isPlayerReady);
            }

            playerListEntries.Add(p.ActorNumber, entry);
        }

        _startButton.gameObject.SetActive(CheckPlayersReady());
        _readyButton.gameObject.SetActive(!CheckPlayersReady());

        Hashtable props = new Hashtable
        {
            {AsteroidsGame.PLAYER_LOADED_LEVEL, false}
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
    }
    private bool CheckPlayersReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return false;
        }

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            object isPlayerReady;
            if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
            {
                if (!(bool) isPlayerReady)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    public void OnStartGameButtonClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;

        PhotonNetwork.LoadLevel("GamePlay");
    }
    #region PUN CALLBACKS
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        GameObject entry = Instantiate(PlayerListEntryPrefab);
        entry.transform.SetParent(Root);
        entry.transform.localScale = Vector3.one;
        entry.SetActive(true);
        entry.GetComponent<PlayerRoomUI>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

        playerListEntries.Add(newPlayer.ActorNumber, entry);

        _startButton.gameObject.SetActive(CheckPlayersReady());
        _readyButton.gameObject.SetActive(!CheckPlayersReady());
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
        playerListEntries.Remove(otherPlayer.ActorNumber);

        _startButton.gameObject.SetActive(CheckPlayersReady());
        _readyButton.gameObject.SetActive(!CheckPlayersReady());
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {
            _startButton.gameObject.SetActive(CheckPlayersReady());
            _readyButton.gameObject.SetActive(!CheckPlayersReady());
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (playerListEntries == null)
        {
            playerListEntries = new Dictionary<int, GameObject>();
        }

        GameObject entry;
        if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
        {
            object isPlayerReady;
            if (changedProps.TryGetValue(AsteroidsGame.PLAYER_READY, out isPlayerReady))
            {
                entry.GetComponent<PlayerRoomUI>().SetPlayerReady((bool) isPlayerReady);
            }
        }

        _startButton.gameObject.SetActive(CheckPlayersReady());
        _readyButton.gameObject.SetActive(!CheckPlayersReady());
    }

    #endregion

    private void DisableInteraction()
    {
        _startButton.interactable = false;
    }

    private void EnableInteraction()
    {
        _startButton.interactable = true;
    }
    public void DisposeMemory()
    {

    }
}
