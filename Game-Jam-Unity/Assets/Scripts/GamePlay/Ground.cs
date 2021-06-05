
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Ground : MonoBehaviour
{
    public static float MovementSpeed = 500f;
    
    public Transform rightLink;
    
    private PhotonView photonView;

#pragma warning disable 0109
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;
#pragma warning restore 0109
    

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }
    
    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        rigidbody.velocity = Vector3.left * MovementSpeed * Time.fixedDeltaTime;
    }
}
