using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRoomUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI PlayerNameText;
    public TextMeshProUGUI PlayerNumText;

    public Button PlayerReadyButton;
    public Image PlayerReadyImage;

    private int ownerId;
    private bool isPlayerReady;

    #region UNITY
    

    public void Start()
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber != ownerId)
        {

        }
        else
        {
            Hashtable initialProps = new Hashtable() {{AsteroidsGame.PLAYER_READY, isPlayerReady}};
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
            PhotonNetwork.LocalPlayer.SetScore(0);

            PlayerReadyButton.onClick.AddListener(() =>
            {
                isPlayerReady = !isPlayerReady;
                SetPlayerReady(isPlayerReady);

                Hashtable props = new Hashtable() {{AsteroidsGame.PLAYER_READY, isPlayerReady}};
                PhotonNetwork.LocalPlayer.SetCustomProperties(props);

                if (PhotonNetwork.IsMasterClient)
                {
                    FindObjectOfType<LobbyMainPanel>().LocalPlayerPropertiesUpdated();
                }
            });
        }

        if (PhotonNetwork.IsMasterClient)
        {
            Hashtable initialProps = new Hashtable() {{AsteroidsGame.PLAYER_LIVES, AsteroidsGame.PLAYER_MAX_LIVES}, {"PlayerScore", 0}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(initialProps);
        }
    }


    #endregion

    public void Initialize(int playerId, string playerName)
    {
        ownerId = playerId;
        PlayerNameText.text = playerName;
        PlayerNumText.text = playerId.ToString();
    }


    

    public void SetPlayerReady(bool playerReady)
    {
        PlayerReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = playerReady ? "Ready!" : "Ready?";
        PlayerReadyImage.enabled = playerReady;
        
    }
    
}