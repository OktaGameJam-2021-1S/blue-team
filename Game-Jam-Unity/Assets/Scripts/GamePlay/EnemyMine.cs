using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMine : Enemy
{
    private Transform ground;

    private bool ArriveTarget;

    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        ground = GameObject.FindWithTag("Ground").transform;
        position = ground.InverseTransformPoint(Random.Range(0, 0.5f), 0, 0);
    }

    public override void Movement()
    {
        if(position.x >= transform.position.x)
            return;
        base.Movement();
        
    }
}
