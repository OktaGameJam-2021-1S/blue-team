using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToRandomDirection : MonoBehaviour
{
    public float velocity = 500f;
    Rigidbody rigi;
    private PhotonView photonView;

    private void Awake()
    {
        rigi = GetComponent<Rigidbody>();
        photonView = GetComponent<PhotonView>();
        gameObject.transform.Rotate(0f, Random.Range(0f, 360f), 0f);
    }

    private void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        transform.Translate(transform.forward * Time.deltaTime * velocity);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("InvisibleWalls"))
        {
            if (photonView.IsMine)
            {
                Vector3 newDirection = Vector3.Reflect(transform.forward, collision.transform.forward);
                transform.rotation = Quaternion.LookRotation(newDirection, Vector3.up) ;
            }
        }
    }
}
