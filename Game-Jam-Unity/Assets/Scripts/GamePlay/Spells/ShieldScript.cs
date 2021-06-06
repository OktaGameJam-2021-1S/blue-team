using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript: MonoBehaviour
{
    public bool IsDestroyed { get; set ; }
    public PlayerRunner Runner {get; set ; }
    
    private void Update()
    {
        if(!Runner.hasShield)
        {
            IsDestroyed = true;
        }
    }

    public IEnumerator CDEnd(float durationTime)
    {
        yield return new WaitForSeconds(durationTime);
        IsDestroyed = true;
    }
}