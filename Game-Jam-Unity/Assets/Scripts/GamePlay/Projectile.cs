using System.Collections.Generic;
using ExitGames.Client.Photon;
using GG.Constants;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using System;

public class Projectile : MonoBehaviour 
{
    public float speed;

    private float t = 0;

    private Transform player;
    private Vector3 target;

    private Vector3 start;

    void Start(){
        player = GameObject.FindWithTag("Player").transform;

        target = player.position;
        start = transform.position;
        
    }

    void Update() {
        t += Time.deltaTime * speed * 0.1f;

        transform.position = Projectile.Parabola(start, target, 3, t);
        
        if(transform.position.x == target.x && transform.position.y == target.y){
            DestroyProjectile();
        }
    }

    public static Vector3 Parabola(Vector3 start, Vector3 end, float height, float t)
    {
        Func<float, float> f = x => -4 * height * x * x + 4 * height * x;

        var mid = Vector3.Lerp(start, end, t);

        return new Vector3(mid.x, f(t) + Mathf.Lerp(start.y, end.y, t), mid.z);
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GetComponent<PhotonView>().IsMine)
            {
                collision.gameObject.GetComponent<PhotonView>().RPC("LoseLife", RpcTarget.All);
                DestroyProjectile();
            }
        }
    }

    void DestroyProjectile(){
        Destroy(gameObject);
    }
}