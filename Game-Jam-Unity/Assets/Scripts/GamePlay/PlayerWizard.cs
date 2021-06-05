using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using UnityEngine;

public class PlayerWizard : MonoBehaviour
{
    public float RotationSpeed = 90.0f;
    public float MovementSpeed = 2.0f;
    public float MaxSpeed = 0.2f;
    public UIWizardView m_pUIWizardView;

    public ParticleSystem Destruction;
    public GameObject EngineTrail;
    public GameObject BulletPrefab;

    private PhotonView photonView;

#pragma warning disable 0109
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;
#pragma warning restore 0109

    private bool controllable = true;
    private bool m_bIsCasting = false;
    private MagicType m_eCurrentMagicType = MagicType.None;


    #region UNITY

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
    }

    public void Start()
    {
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            r.material.color = AsteroidsGame.GetColor(photonView.Owner.GetPlayerNumber());
        }
    }

  
    public void FixedUpdate()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        if (!controllable || m_bIsCasting)
        {
            return;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        rigidbody.velocity = new Vector3(horizontal, 0, 0) * MovementSpeed * Time.fixedDeltaTime;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CastMagic(m_eCurrentMagicType);
            rigidbody.velocity = Vector3.zero;
        }
    }

    public void CastMagic(MagicType eMagicType)
    {
        if (m_bIsCasting)
        {
            return;
        }
        m_bIsCasting = true;
        StartCoroutine(CastMagicCoroutine(eMagicType));
    }


    public IEnumerator CastMagicCoroutine(MagicType eMagicType)
    {
        switch (eMagicType)
        {
            case MagicType.None: //for debug purpose
            case MagicType.Firebolt:
                yield return StartCoroutine(m_pUIWizardView.ShowVerticalTargetSelector());
                m_bIsCasting = false;
                break;
        }
        yield break;
    }

    #endregion

    #region COROUTINES

    private IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(AsteroidsGame.PLAYER_RESPAWN_TIME);

        photonView.RPC("RespawnSpaceship", RpcTarget.AllViaServer);
    }

    #endregion
    
}

public enum MagicType
{
    None,
    Firebolt,
    Fireball,

}