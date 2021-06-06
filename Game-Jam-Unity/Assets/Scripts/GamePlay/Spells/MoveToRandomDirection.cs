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
        transform.Translate(transform.forward * Time.deltaTime * velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("InvisibleWalls"))
        {
            if (photonView.IsMine)
            {
                Vector3 newDirection = Vector3.Reflect(transform.forward.normalized, collision.contacts[0].normal);
                transform.localEulerAngles = newDirection ;
            }
        }
    }
}
