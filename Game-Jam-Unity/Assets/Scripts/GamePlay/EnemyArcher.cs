using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class EnemyArcher : Enemy
{
    private Transform player;
    
    private Transform ground;

    private bool ArriveTarget;

    private Vector3 position;
    // Start is called before the first frame update

    private float timeBtwShots;

    public float startTimeBtwShots;

    public GameObject project;
    void Start()
    {
        ground = GameObject.FindWithTag("Ground").transform;
        position = ground.TransformPoint(Random.Range(0.40f, 0.6f), 0, 0);
        player = GameObject.FindWithTag("Player").transform;
        timeBtwShots = startTimeBtwShots;
    }

    void Update()
    {
        if(timeBtwShots <= 0)
        {
            PhotonNetwork.Instantiate(project.name, transform.position, Quaternion.identity);
            timeBtwShots = startTimeBtwShots;

        } else
        {
            timeBtwShots -= Time.deltaTime;
        }
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