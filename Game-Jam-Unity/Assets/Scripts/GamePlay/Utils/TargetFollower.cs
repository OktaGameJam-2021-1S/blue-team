using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{

    public bool FollowPlayer = false;
    public Transform Target { get; set; }

    public void Start()
    {
        if (FollowPlayer)
        {
            Target = FindObjectOfType<PlayerRunner>().transform;
        }
    }

    void Update()
    {
        if(Target == null)
        {
            return;
        }
        this.transform.position = Target.position;
    }
}
