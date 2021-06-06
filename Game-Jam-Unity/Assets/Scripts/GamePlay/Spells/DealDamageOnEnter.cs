using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageOnEnter : MonoBehaviour
{
    public int m_iDamage = 1;
    private PhotonView photonView;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                other.GetComponent<PhotonView>().RPC("LoseLife", RpcTarget.All, m_iDamage);
            }

            if (other.gameObject.CompareTag("Player"))
            {
                other.GetComponent<PhotonView>().RPC("LoseLife", RpcTarget.All); //runner always take 1 damage per effect
            }
        }

    }
}
