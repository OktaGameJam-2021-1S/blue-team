using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class LoginView : MonoBehaviourPunCallbacks, IView
{
    #region Attributes

    [SerializeField] private Button _loginButton;
    [SerializeField] private TMP_InputField PlayerNameInput;
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
        
        PhotonNetwork.AutomaticallySyncScene = true;

            
        PlayerNameInput.text = "Noob " + Random.Range(1000, 10000);
        
        Loading.SetActive(false);
        EnableInteraction();
        
        _loginButton.onClick.RemoveAllListeners();
        _loginButton.onClick.AddListener(OnLoginButtonClicked);
    }
    
    public void OnLoginButtonClicked()
    {
        string playerName = PlayerNameInput.text;

        if (!playerName.Equals(""))
        {
            DisableInteraction();
            Loading.SetActive(true);
            PhotonNetwork.LocalPlayer.NickName = playerName;
            PhotonNetwork.ConnectUsingSettings();
        }
        else
        {
            Debug.LogError("Player Name is invalid.");
        }

      
    }
    
    #region PUN CALLBACKS

    public override void OnConnectedToMaster()
    {
        var selectionView = _viewManager.GetScreen<SelectionView>();
        selectionView.SetupDependency(_session, _viewController, _viewManager);
        _viewController.OpenSingleView(selectionView, _viewManager.ViewData);
    }
    
    #endregion

    private void DisableInteraction()
    {
        _loginButton.interactable = false;
        PlayerNameInput.interactable = false;
    }

    private void EnableInteraction()
    {
        _loginButton.interactable = true;
        PlayerNameInput.interactable = true;
    }
    public void DisposeMemory()
    {

    }
}
