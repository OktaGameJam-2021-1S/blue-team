using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{

    public Transform Target { get; set; }
    private Rigidbody rigid;
    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if(Target == null)
        {
            return;
        }
        rigid.position = Target.position;
    }
}
