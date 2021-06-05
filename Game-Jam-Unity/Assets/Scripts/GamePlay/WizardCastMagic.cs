using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardCastMagic : MonoBehaviour
{
    [System.Serializable]
    public class CastMagic
    {
        public List<ElementType> ElementsRequired;
        public SpellType CastType;
    }
    
    public List<CastMagic> ValidCasts = new List<CastMagic>();

    public SpellType CastSpell(params ElementType[] elementTypes)
    {
        for (int i = 0; i < ValidCasts.Count; i++)
        {
            bool contains = true;
            for (int j = 0; j < elementTypes.Length; j++)
            {
                contains &= ValidCasts[i].ElementsRequired.Contains(elementTypes[j]);
            }

            if (contains)
                return ValidCasts[i].CastType;
        }
        return SpellType.None;
    }
}
