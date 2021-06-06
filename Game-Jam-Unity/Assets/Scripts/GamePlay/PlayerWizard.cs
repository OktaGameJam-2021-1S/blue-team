﻿using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerWizard : MonoBehaviourPunCallbacks
{
    public float RotationSpeed = 90.0f;
    public float MovementSpeed = 2.0f;
    public float MaxSpeed = 0.2f;
    public UIWizardView m_pUIWizardView;
    public Transform m_gVerticalSpellRoot;
    public Transform m_gAerialSpellRoot; // used to cast meteor from sky
    public Transform m_gHorizontalSpellRoot; // used to cast meteor from sky

    public WizardCastMagic WizardCastMagic;

    public List<ElementType> Elements;
    public List<ElementType> SelectedElements;
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
    public float m_fFlamethrowerTimeInSeconds = 2f;
    public float m_fFlameTornadoifeSpanTimeInSeconds = 5f;


    #region UNITY

    public void Awake()
    {
        photonView = GetComponent<PhotonView>();

        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
        m_gRunner = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            SyncElement();
        }
    }


    private void Update()
    {

        if (!photonView.IsMine)
        {
            return;
        }
        SelectElement();
   
        if (Input.GetKeyDown(KeyCode.Space))
        {
         
            // CastMagic(m_eCurrentMagicType);
            if(SelectedElements.Count == 0)
                return;
            List<ElementType> respawn = new List<ElementType>(SelectedElements);
            if(SelectedElements.Count == 1)
                SelectedElements.Add(ElementType.None);
            
            CastMagic(WizardCastMagic.CastSpell(SelectedElements.ToArray()));
            
            GamePlayNetworkManager.Instance.ElementsToSpawn.AddRange(respawn);
            SelectedElements.Clear();
            rigidbody.velocity = Vector3.zero;
        }
        
        if (!controllable || m_bIsCasting)
        {
            return;
        }
        
        float horizontal = Input.GetAxisRaw("Horizontal");
        rigidbody.velocity = new Vector3(horizontal, 0, 0) * MovementSpeed * Time.fixedDeltaTime;
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (photonView.IsMine){
            if (changedProps.ContainsKey(AsteroidsGame.PLAYER_ELEMENTS))
            {
                var elementType = (ElementType)changedProps[AsteroidsGame.PLAYER_ELEMENTS];
                Elements.Add(elementType);
                SyncElement();
            }
        }
    }

    private void SelectElement()
    {
        ElementType element = GetElementType();
        if (element != ElementType.None)
        {
            if (Elements.Contains(element) && SelectedElements.Count < 2)
            {
                SelectedElements.Add(element);
                Elements.Remove(element);
                
                SyncElement();
            }
        }
    }

    private void SyncElement()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable{{AsteroidsGame.PLAYER_ELEMENTINFO, ElementType.Fire}, {AsteroidsGame.PLAYER_ELEMENTAMOUNT, Elements.FindAll((element) => element == ElementType.Fire).Count}});
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable{{AsteroidsGame.PLAYER_ELEMENTINFO, ElementType.Air}, {AsteroidsGame.PLAYER_ELEMENTAMOUNT, Elements.FindAll((element) => element == ElementType.Air).Count}});
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable{{AsteroidsGame.PLAYER_ELEMENTINFO, ElementType.Earth}, {AsteroidsGame.PLAYER_ELEMENTAMOUNT, Elements.FindAll((element) => element == ElementType.Earth).Count}});
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable{{AsteroidsGame.PLAYER_ELEMENTINFO, ElementType.Water}, {AsteroidsGame.PLAYER_ELEMENTAMOUNT, Elements.FindAll((element) => element == ElementType.Water).Count}});
   
    }
    private ElementType GetElementType()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
        {
            return ElementType.Fire;
        }
        if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
        {
            return ElementType.Water;
        }
        if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
        {
            return ElementType.Air;
        }
        if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
        {
            return ElementType.Earth;
        }

        return ElementType.None;
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

            case SpellType.Meteor:
                {
                    coroutineView = m_pUIWizardView.ShowAreaOfEffect(m_gVerticalSpellRoot.transform.position, 5);
                    yield return StartCoroutine(coroutineView);
                    Vector3? targetPosition = (Vector3)coroutineView.Current;
                    if (targetPosition.HasValue)
                    {
                        yield return StartCoroutine(CastMeteor(targetPosition.Value));
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

            case SpellType.FireTornado:
                {
                    coroutineView = m_pUIWizardView.ShowAreaOfEffect(m_gVerticalSpellRoot.transform.position, 2);
                    yield return StartCoroutine(coroutineView);
                    Vector3? targetPosition = (Vector3)coroutineView.Current;
                    if (targetPosition.HasValue)
                    {
                        yield return StartCoroutine(CastFlameTornado(targetPosition.Value));
                    }
                    m_bIsCasting = false;
                    break;
                }

            case SpellType.Tsunami:
                {
                    coroutineView = m_pUIWizardView.ShowAreaOfEffect(m_gVerticalSpellRoot.transform.position, 3, true);
                    yield return StartCoroutine(coroutineView);
                    Vector3? targetPosition = (Vector3)coroutineView.Current;
                    if (targetPosition.HasValue)
                    {
                        yield return StartCoroutine(CastTsunami(targetPosition.Value));
                    }
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
        yield return new WaitForSeconds(m_fFlamethrowerTimeInSeconds);
        Destroy(obj);
        yield break;
    }

    public IEnumerator CastFlameTornado(Vector3 pPostition)
    {
        yield return new WaitForSeconds(m_fCastTimeInSeconds);

        GameObject obj = PhotonNetwork.InstantiateRoomObject("FlameTornado", pPostition, Quaternion.identity, 0, null);

        yield return new WaitForSeconds(m_fFlameTornadoifeSpanTimeInSeconds);
        Destroy(obj);
    }

    public IEnumerator CastMeteor(Vector3 pPosition)
    {
        yield return new WaitForSeconds(m_fCastTimeInSeconds);

        GameObject obj = PhotonNetwork.InstantiateRoomObject("Meteor", m_gAerialSpellRoot.transform.position, Quaternion.identity, 0, null);
        MeteorScript meteor = obj.GetComponent<MeteorScript>();
        meteor.Target = pPosition;
        obj.transform.LookAt(pPosition);

        while (!meteor.IsDestroyed)
        {
            yield return null; //wait until meteor falls
        }
        Destroy(obj);
        yield break;
    }

    public IEnumerator CastTsunami(Vector3 pPosition)
    {
        Vector3 horizontalPosition = new Vector3(m_gHorizontalSpellRoot.position.x, m_gHorizontalSpellRoot.position.y, pPosition.z);
        GameObject obj = PhotonNetwork.InstantiateRoomObject("Tsunami", horizontalPosition, Quaternion.identity, 0, null);
        obj.transform.LookAt(pPosition);
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
    Meteor,
    Flamethrower,
    FireTornado,
    Tsunami,

}