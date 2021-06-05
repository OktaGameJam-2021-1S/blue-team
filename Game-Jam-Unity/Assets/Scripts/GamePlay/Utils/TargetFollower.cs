using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollower : MonoBehaviour
{

    public Transform Target { get; set; }

    void Update()
    {
        if(Target == null)
        {
            return;
        }
        this.transform.position = Target.position;
    }
}
