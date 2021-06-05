using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageWhenInsideCollider : MonoBehaviour
{
    public int m_iDamage = 1;
    public float m_fDamageCooldown = 0.25f;
    private PhotonView photonView;
    List<Enemy> enemiesToTakeDamage = new List<Enemy>();
    private Coroutine damagePerTimeCoroutine;

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();
        damagePerTimeCoroutine = StartCoroutine(DealDamagePerSecond());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (photonView.IsMine)
            {
                Enemy enemy = other.gameObject.GetComponent<Enemy>();
                if (enemy != null && !enemiesToTakeDamage.Contains(enemy))
                {
                    if (damagePerTimeCoroutine != null)
                    {
                        StopCoroutine(damagePerTimeCoroutine);
                    }

                    enemiesToTakeDamage.Add(enemy);
                    if (damagePerTimeCoroutine == null)
                    {
                        StartCoroutine(DealDamagePerSecond());
                    }

                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (photonView.IsMine)
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
                        StartCoroutine(DealDamagePerSecond());
                    }
                }

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
            yield return new WaitForSeconds(m_fDamageCooldown);
        }
    }
}
