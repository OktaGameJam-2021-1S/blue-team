using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollower : Enemy
{
    private Transform player;


    private bool Upper;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    public override void Movement()
    {
        base.Movement();
        

        rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, Time.fixedDeltaTime * velocityMultiplier  * (transform.position.z < player.transform.position.z  ?  150 : -150) );

        
    }
}
