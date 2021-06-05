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
    public GameObject m_gVerticalSpellRoot;

    public WizardCastMagic WizardCastMagic;
    
    private PhotonView photonView;

#pragma warning disable 0109
    private new Rigidbody rigidbody;
    private new Collider collider;
    private new Renderer renderer;
#pragma warning restore 0109

    private bool controllable = true;
    private bool m_bIsCasting = false;
    private GameObject m_gRunner;
    public SpellType m_eCurrentMagicType = SpellType.None;

    [Header("Spell Related")]
    public float m_fCastTimeInSeconds = 1f;
    public float m_fflamethrowerTimeInSeconds = 2f;


    #region UNITY

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        m_gRunner = GameObject.FindGameObjectWithTag("Player");
    }




    private void Update()
    {

        if (!photonView.IsMine)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CastMagic(m_eCurrentMagicType);
            rigidbody.velocity = Vector3.zero;
        }
        
        if (!controllable || m_bIsCasting)
        {
            return;
        }
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        rigidbody.velocity = new Vector3(horizontal, 0, 0) * MovementSpeed * Time.fixedDeltaTime;
    }

    public void CastMagic(SpellType eMagicType)
    {
        if (m_bIsCasting)
        {
            return;
        }
        m_bIsCasting = true;
        StartCoroutine(CastMagicCoroutine(eMagicType));
    }


    public IEnumerator CastMagicCoroutine(SpellType eMagicType)
    {
        IEnumerator coroutineView = null;
        switch (eMagicType)
        {
            case SpellType.Firebolt:
                {
                    coroutineView = m_pUIWizardView.ShowVerticalInputSelector();
                    yield return StartCoroutine(coroutineView);
                    float? verticalRotationValue = (float)coroutineView.Current;
                    if (verticalRotationValue.HasValue)
                    {
                        m_gVerticalSpellRoot.transform.localEulerAngles = new Vector3(m_gVerticalSpellRoot.transform.localEulerAngles.x, verticalRotationValue.Value, m_gVerticalSpellRoot.transform.localEulerAngles.x);
                    }
                    m_bIsCasting = false;
                    break;
                }

            case SpellType.Fireball:
                {
                    coroutineView = m_pUIWizardView.ShowAreaOfEffect(m_gVerticalSpellRoot.transform.position, 3);
                    yield return StartCoroutine(coroutineView);
                    Vector3? targetPosition = (Vector3)coroutineView.Current;
                    if (targetPosition.HasValue)
                    {
                        //CAST SPELL IN THE AREA
                    }
                    m_bIsCasting = false;
                    break;
                }

            case SpellType.Flamethrower:
                {
                    yield return StartCoroutine(CastFlamethrower(GetRunner().transform));
                    m_bIsCasting = false;
                    break;
                }

            default:
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

    #region SPELLS
    public IEnumerator CastFlamethrower(Transform pParentTransform)
    {
        //CASTING
        yield return new WaitForSeconds(m_fCastTimeInSeconds);

        GameObject obj = PhotonNetwork.InstantiateRoomObject("FlameThrower", pParentTransform.position, Quaternion.identity, 0, null);

        obj.GetComponent<TargetFollower>().Target = pParentTransform;
        yield return new WaitForSeconds(m_fflamethrowerTimeInSeconds);
        Destroy(obj);
        yield break;
    }
    #endregion
    private GameObject GetRunner()
    {
        if(m_gRunner == null)
        {
            m_gRunner = GameObject.FindGameObjectWithTag("Player");
        }
        return m_gRunner;
    }
}

public enum SpellType
{
    None,
    Firebolt,
    Fireball,
    Flamethrower,

}