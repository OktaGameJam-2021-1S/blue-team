﻿
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerRunner : MonoBehaviour
{
    public float RotationSpeed = 90.0f;
    public float MovementSpeed = 2.0f;
    public float MaxSpeed = 0.2f;

    public ParticleSystem Destruction;
    public GameObject EngineTrail;
    public GameObject BulletPrefab;
    public bool hasShield = false;

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
    
    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!controllable)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float diagonalMultiplier = horizontal != 0 && vertical != 0 ? 0.75f: 1f;
        rigidbody.velocity = new Vector3(horizontal, 0, vertical) * MovementSpeed * Time.fixedDeltaTime * diagonalMultiplier;

   
    }
    [PunRPC]
    public void LoseLife()
    {
        if (photonView.IsMine)
        {
            if (hasShield)
            {
                hasShield = false;
                return;
            }
            object lives;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LIVES, out lives))
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable {{AsteroidsGame.PLAYER_LIVES, ((int) lives <= 1) ? 0 : ((int) lives - 1)}});
            }
        }
    }

    [PunRPC]
    public void SetShieldAndGainLife(bool value)
    {
        if (photonView.IsMine)
        {
            hasShield = value;
            object lives;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LIVES, out lives))
            {
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable {{AsteroidsGame.PLAYER_LIVES, ((int) lives <= 2) ? ((int) lives + 1 ) : ((int) lives + 0)}});
            }
        }
    }

    [PunRPC]
    public void SetShield(bool value)
    {
        if (photonView.IsMine)
        {
            hasShield = value;
        }
    }
    #endregion
    
    
}