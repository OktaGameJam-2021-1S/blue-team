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
        position = ground.TransformPoint(Random.Range(0.40f, 0.6f), 0, 0);
    }
    
    public override void Movement()
    {
        if (position.x >= transform.position.x)
        {
            rigidbody.velocity = Vector3.zero;
            return;
        }
        base.Movement();
        
    }
}
