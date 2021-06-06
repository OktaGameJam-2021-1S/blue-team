
using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GamePlay;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using UnityEngine;

[Flags]
public enum ElementType
{
    None = 0,
    Air = 1 << 1,
    Fire = 1 << 2,
    Water = 1 << 3,
    Earth = 1 << 4,
}
public class ElementSpawn : MonoBehaviour
{
    public ElementType ElementType;
    
    private PhotonView photonView;

#pragma warning disable 0109
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;
#pragma warning restore 0109

    private float rotation = 0.0f;
    private float acceleration = 0.0f;
    private float shootingTimer = 0.0f;

    private bool controllable = true;

    #region UNITY

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }
    
    public void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        

        rigidbody.velocity = Vector3.left * Ground.MovementSpeed * Time.deltaTime ;
    }

    [PunRPC]
    public void DisableElement()
    {
        transform.position = Vector3.down * 1000;
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{AsteroidsGame.PLAYER_ELEMENTS, ElementType}});
                photonView.RPC("DisableElement", RpcTarget.All);
            }
        }

        if (collision.gameObject.CompareTag("DestroyElement"))
        {
            if (photonView.IsMine)
            {
                GamePlayNetworkManager.Instance.ElementsToSpawn.Add(ElementType);
                photonView.RPC("DisableElement", RpcTarget.All);
          
            }
        }
    }

    #endregion

}
