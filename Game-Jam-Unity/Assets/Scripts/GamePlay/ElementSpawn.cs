
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public enum ElementType
{
    Air,
    Fire,
    Water,
    Earth
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


    #endregion

}
