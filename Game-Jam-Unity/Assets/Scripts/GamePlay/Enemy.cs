﻿
using System.Collections.Generic;
using ExitGames.Client.Photon;
using GG.Constants;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float velocityMultiplier = 1;
    public int Lives = 1;
    protected PhotonView photonView;

#pragma warning disable 0109
    public new Rigidbody rigidbody;
#pragma warning restore 0109

    private float rotation = 0.0f;
    private float acceleration = 0.0f;
    private float shootingTimer = 0.0f;

    protected bool controllable = true;

    #region UNITY

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody>();
    }

    public void Start()
    {
    
    }

    public void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!controllable)
        {
            return;
        }
        Movement();
    }

    public virtual void Movement()
    {
        rigidbody.velocity = Vector3.left * Ground.MovementSpeed * Time.fixedDeltaTime * velocityMultiplier;
    }
    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (photonView.IsMine)
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("LoseLife", RpcTarget.All);
                
            }
        }
    }

    #endregion

    #region PHOTON
    [PunRPC]
    public void LoseLife(int iDamage)
    {
        if (photonView.IsMine)
        {
            Lives -= iDamage;

            if(Lives <= 0)
            {
                object score;
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(K.GamePlay.SCORE, out score))
                {
                    int newScore = (int)score +1;
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { K.GamePlay.SCORE, newScore } });
                }
                transform.position = Vector3.down * 1000;
            }
        }
    }

    #endregion
}
