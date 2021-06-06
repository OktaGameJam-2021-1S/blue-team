using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript: MonoBehaviour
{
    public bool IsDestroyed { get; set ; }
    public PlayerRunner Runner {get; set ; }
    private PhotonView photonView;
    public bool timeEnd {get; set; }


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
        if (!Runner.hasShield)
        {
            IsDestroyed = true;
        }
    }

    public IEnumerator CDEnd(float durationTime)
    {
        yield return new WaitForSeconds(durationTime);
        timeEnd = true;
    }
}