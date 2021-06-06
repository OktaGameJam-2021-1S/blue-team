using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageWhenInsideCollider : MonoBehaviour
{
    public int m_iDamage = 1;
    public float m_fDamageCooldown = 0.25f;
    public bool CanDamagePlayer = true;
    private PhotonView photonView;
    List<Enemy> enemiesToTakeDamage = new List<Enemy>();
    PlayerRunner runner = null;
    private Coroutine damagePerTimeCoroutine;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        damagePerTimeCoroutine = StartCoroutine(DealDamagePerSecond());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {

                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                if (enemy != null && !enemiesToTakeDamage.Contains(enemy))
                {
                    if (damagePerTimeCoroutine != null)
                    {
                        StopCoroutine(damagePerTimeCoroutine);
                    }

                    enemiesToTakeDamage.Add(enemy);

                    damagePerTimeCoroutine = StartCoroutine(DealDamagePerSecond());
                }

            }
            if (other.gameObject.CompareTag("Player") && CanDamagePlayer)
            {
                runner = other.GetComponent<PlayerRunner>();
            }
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.IsMine)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    if (damagePerTimeCoroutine != null)
                    {
                        StopCoroutine(damagePerTimeCoroutine);
                    }

                    enemiesToTakeDamage.Remove(enemy);

                    if (damagePerTimeCoroutine == null)
                    {
                        damagePerTimeCoroutine = StartCoroutine(DealDamagePerSecond());
                    }
                }

            }
            if (other.gameObject.CompareTag("Player"))
            {
                runner = null;
            }
        }
    }

    private IEnumerator DealDamagePerSecond()
    {
        while(true)
        {
            for (int i = 0; i < enemiesToTakeDamage.Count; i++)
            {
                Enemy enemy = enemiesToTakeDamage[i];
                if(enemy.Lives > 0)
                {
                    enemy.GetComponent<PhotonView>().RPC("LoseLife", RpcTarget.All, m_iDamage);
                }
            }
            if(runner != null)
            {
                runner.GetComponent<PhotonView>().RPC("LoseLife", RpcTarget.All); //runner always take 1 damage per tick
            }

            yield return new WaitForSeconds(m_fDamageCooldown);
        }
    }
}
