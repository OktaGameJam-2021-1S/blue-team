
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
    public float BuffMultiplier = 1f;

    public ParticleSystem Destruction;
    public GameObject EngineTrail;
    public GameObject BulletPrefab;
    public bool hasShield = false;
    public ParticleSystem OnTakeHit;
    public ParticleSystem OnLoseShield;

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
        rigidbody.velocity = new Vector3(horizontal, 0, vertical) * MovementSpeed * Time.fixedDeltaTime * diagonalMultiplier * BuffMultiplier;

   
    }
    [PunRPC]
    public void LoseLife()
    {
        if (photonView.IsMine)
        {
            if (hasShield)
            {
                hasShield = false;
                OnLoseShield.Play();
                return;
            }
            object lives;
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LIVES, out lives))
            {
                OnTakeHit.Play();
                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable {{AsteroidsGame.PLAYER_LIVES, ((int) lives <= 1) ? 0 : ((int) lives - 1)}});
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

    [PunRPC]
    public void SetSpeedBuff(float value)
    {
        if (photonView.IsMine)
        {
            BuffMultiplier = value;
        }
    }
    #endregion


}