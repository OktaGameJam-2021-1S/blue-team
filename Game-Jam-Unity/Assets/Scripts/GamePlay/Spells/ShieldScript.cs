using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class ShieldScript: MonoBehaviour
{
    public bool IsDestroyed { get; set ; }
    public PlayerRunner Runner {get; set ; }
    
    public bool timeEnd {get; set; }

    private PhotonView _photonView;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();
        Runner = FindObjectOfType<PlayerRunner>();
    }

    private void Update()
    {
        if (_photonView.IsMine)
        {
            if (!Runner.hasShield)
            {
                IsDestroyed = true;
            }
        }
    }

    public IEnumerator CDEnd(float durationTime)
    {
        yield return new WaitForSeconds(durationTime);
        timeEnd = true;
    }
}