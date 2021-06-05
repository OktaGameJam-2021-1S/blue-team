using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWizardView : MonoBehaviour
{
    public GameObject m_gVerticalRotationStart;
    public TargetMovement m_gTarget;
    public float m_fMaxRotationValue = 30f;
    public float m_fMinRotationValue = -30f;
    public float m_fRotationSpeed = 30f;

    private bool valueSelected = false;
    private bool waitingForInput = false;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
    }

    public IEnumerator ShowVerticalInputSelector()
    {
        m_gVerticalRotationStart.SetActive(true);
        var verticalRotation = SelectVerticalRotation(m_gVerticalRotationStart.transform);
        yield return StartCoroutine(verticalRotation);
        yield return verticalRotation.Current;
        m_gVerticalRotationStart.SetActive(false);
        yield break;
    }

    public IEnumerator ShowAreaOfEffect(Vector3 targetPosition, float fArea)
    {
        m_gTarget.gameObject.SetActive(true);
        m_gTarget.transform.localScale = new Vector3(fArea, 0, fArea);
        m_gTarget.transform.position = targetPosition;
        m_gTarget.CanMove = true;
        var areaOfEffect = SelectAreaOfEffect();
        yield return StartCoroutine(areaOfEffect);
        yield return m_gTarget.transform.position;
        m_gTarget.CanMove = false;
        m_gTarget.gameObject.SetActive(false);
        yield break;
    }

    private IEnumerator SelectVerticalRotation(Transform pViewTransformToRotate)
    {
        Transform startTransform = pViewTransformToRotate;
        startTransform.rotation = Quaternion.identity;
        bool rotationSelected = false;
        yield return null;
        waitingForInput = true;
        valueSelected = false;
        while (!rotationSelected)
        {
            startTransform.localEulerAngles = new Vector3(startTransform.localEulerAngles.x, Mathf.PingPong(Time.time * m_fRotationSpeed , m_fMaxRotationValue*2) +m_fMinRotationValue);
            Debug.Log(startTransform.localEulerAngles.y);
            Debug.DrawLine(startTransform.position, startTransform.position + (startTransform.forward * -100f), Color.red, 1f);

            if (valueSelected)
            {
                rotationSelected = true;
                waitingForInput = false;
            }
            yield return null;
        }
        
        yield return startTransform.localEulerAngles.y;
        yield break;
    }

    private IEnumerator SelectAreaOfEffect()
    {
        valueSelected = false;
        yield return null;
        waitingForInput = true;
        while (!valueSelected)
        {
            yield return null;
        }
        waitingForInput = false;
        yield break;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && waitingForInput)
        {
            valueSelected = true;
        }
    }
}
