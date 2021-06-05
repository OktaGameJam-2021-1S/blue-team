using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float velocityMultiplier = 1;
    public int Lives = 1;
    private PhotonView photonView;

#pragma warning disable 0109
    public new Rigidbody rigidbody;
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
}
