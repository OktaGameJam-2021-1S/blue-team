using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnReachTarget : MonoBehaviour
{
    public float speed = 30f;
    public float closestDistanceBetweenTarget = 2f;
    private PhotonView photonView;
    public bool IsDestroyed { get; set ; }

    public Vector3 Target { get; set; }


    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, Target, speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, Target) <= closestDistanceBetweenTarget)
        {
            IsDestroyed = true;
        }
    }

}
