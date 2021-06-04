using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviour, IConnectionCallbacks, IInRoomCallbacks, ILobbyCallbacks,  IMatchmakingCallbacks, IWebRpcCallback, IErrorInfoCallback
{
    public void JoinLobby()
    {
        
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateLobby()
    {

        PhotonNetwork.JoinLobby();
        
    }
    public void ConnectRandomRoom()
    {
        PhotonNetwork.LocalPlayer.NickName = Random.Range(1, 41) + "";
        PhotonNetwork.ConnectUsingSettings();
   
    }

    public void DisconnectRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
    }

    public void OnConnected()
    {
        Debug.Log("Connected");
    }

    public void OnConnectedToMaster()
    {
        Debug.Log("Connected Master");

    }

    public void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnect");

    }

    public void OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("Region List");

    }

    public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("Auth");

    }

    public void OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("Auth Filed");

    }

    public void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("Entry a room");
    }

    public void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Left a room");

    }

    public void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
        Debug.Log("On Room Change");

    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        Debug.Log("On Player Change");

    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log("On Master Switched");

    }

    public void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby");
    }

    public void OnLeftLobby()
    {
        Debug.Log("OnLeftLobby");
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate");
    }

    public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    {
        Debug.Log("OnLobbyStatisticsUpdate");

    }

    public void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        throw new NotImplementedException();
    }

    public void OnCreatedRoom()
    {
        throw new NotImplementedException();
    }

    public void OnCreateRoomFailed(short returnCode, string message)
    {
        throw new NotImplementedException();
    }

    public void OnJoinedRoom()
    {
        throw new NotImplementedException();
    }

    public void OnJoinRoomFailed(short returnCode, string message)
    {
        throw new NotImplementedException();
    }

    public void OnJoinRandomFailed(short returnCode, string message)
    {
        throw new NotImplementedException();
    }

    public void OnLeftRoom()
    {
        throw new NotImplementedException();
    }

    public void OnWebRpcResponse(OperationResponse response)
    {
        throw new NotImplementedException();
    }

    public void OnErrorInfo(ErrorInfo errorInfo)
    {
        throw new NotImplementedException();
    }
}
