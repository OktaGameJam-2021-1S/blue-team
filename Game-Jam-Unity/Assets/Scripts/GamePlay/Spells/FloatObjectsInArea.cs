using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatObjectsInArea : MonoBehaviour
{
    private Dictionary<GameObject, float> objectsHeights = new Dictionary<GameObject, float>();
    // Start is called before the first frame update
    public float GroundHeight = 1f;
    public float FlyHeight = 3f;
    public float TimeToGetInTheAir = 1f;

    public void FloatObjects(Vector3 worldPosition, float area)
    {
        Collider[] hitColliders = Physics.OverlapSphere(worldPosition, area);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            Collider col = hitColliders[i];
            if(col.transform.position.y <= GroundHeight)
            {
                if (!objectsHeights.ContainsKey(col.gameObject))
                {
                    objectsHeights.Add(col.gameObject, col.transform.position.y);
                    StartCoroutine(FloatObject(col.transform));
                }
            }
        }
    }

    private IEnumerator FloatObject(Transform pTransform)
    {
        float timeLapsed = 0f;
        float starterYPosition = pTransform.position.y;
        float endYPosition = FlyHeight;
        while (timeLapsed < TimeToGetInTheAir)
        {
            timeLapsed += Time.deltaTime;
            float fraction = timeLapsed / TimeToGetInTheAir;

            pTransform.position = new Vector3(pTransform.position.x, Mathf.Lerp(starterYPosition, endYPosition, fraction), pTransform.position.z);
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    private IEnumerator DefloatObject(Transform pTransform, float pOriginalHeight)
    {
        float timeLapsed = 0f;
        float starterYPosition = pTransform.position.y;
        float endYPosition = pOriginalHeight;
        while (timeLapsed < TimeToGetInTheAir)
        {
            timeLapsed += Time.deltaTime;
            float fraction = timeLapsed / TimeToGetInTheAir;

            pTransform.position = new Vector3(pTransform.position.x, Mathf.Lerp(starterYPosition, endYPosition, fraction), pTransform.position.z);
            yield return new WaitForEndOfFrame();
        }

        yield break;
    }

    public void DefloatObjects()
    {
        foreach (var valuePair in objectsHeights)
        {
            StartCoroutine(DefloatObject(valuePair.Key.transform, valuePair.Value));
        }
    }

}
