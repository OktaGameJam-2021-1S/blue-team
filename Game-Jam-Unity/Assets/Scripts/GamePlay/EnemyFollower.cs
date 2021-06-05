using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : Enemy
{
    private Transform player;

    private int RangeZ = 5;

    private bool Upper;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public override void Movement()
    {
        base.Movement();
        
        if (transform.position.z > RangeZ && Upper || transform.position.z < -RangeZ && !Upper)
            Upper = !Upper;
        
        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, Time.fixedDeltaTime * velocityMultiplier  * (Upper ?  10 : -10) );

        
    }
}
