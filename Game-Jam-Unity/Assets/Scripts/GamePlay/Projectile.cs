using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour 
{
    public float speed;
    private Transform player;
    private Vector2 target;

    void Start(){
        player = GameObject.FindWithTag("Player").transform;

        target = new Vector2(player.position.x, player.position.y);
    }

    void Update() {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        
        if(transform.position.x == target.x && transform.position.y == target.y){
            DestroyProjectile();
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            DestroyProjectile();
        }
    }
    void DestroyProjectile(){
        Destroy(gameObject);
    }
}