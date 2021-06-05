using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWizardView : MonoBehaviour
{
    public GameObject m_gVerticalRotationStart;
    public float m_fMaxRotationValue = 30f;
    public float m_fMinRotationValue = -30f;
    public float m_fRotationSpeed = 30f;

    private bool valueSelected = false;
    private bool waitingForInput = false;

    public IEnumerator ShowVerticalTargetSelector()
    {
        m_gVerticalRotationStart.SetActive(true);
        var coroutine = SelectVerticalRotation();
        yield return StartCoroutine(coroutine);
        float? rotation = coroutine.Current;
        m_gVerticalRotationStart.SetActive(false);
        yield break;
    }

    private IEnumerator<float?> SelectVerticalRotation()
    {
        Transform startTransform = m_gVerticalRotationStart.transform;
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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && waitingForInput)
        {
            valueSelected = true;
        }
    }
}
