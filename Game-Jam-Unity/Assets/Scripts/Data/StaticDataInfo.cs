using System;
using GG.StaticData;
using UnityEngine;

namespace GG.StaticData
{
    [System.Serializable]
    public class StaticDataInfo
    {
        public HeroData[] Heroes;

        public StageData[] Stages;
    }
    [System.Serializable]
    public struct HeroData
    {
        [Header("Hero Data")]
        public string SDID;
        public string Name;
        public int Rarity;
        
        public int[] RequiredExperienceToLevelUp;
        public int[] RequiredLevelsToUnlockAbilitieSlots;

        public string[] BlackListItemsCompatibility;

        [Header("Hero Visual")] 
        public HeroType HeroType;
        public string MainImagePath;

        public string BackgroundPath;
        public Color GlowColor;
    }

    public enum HeroType
    {
        Melee,
        Ranged,
        Tank,
        Assassin
    }

    [System.Serializable]
    public struct StageData
    {
        [Header("Stage Data")] 
        public string SDID;

        public string Title;
        public string MainImagePath;
        public int LevelRequired;

        public StageElementData RootStageElements;
    }

    [System.Serializable]
    public struct StageState
    {
        public string SDID;
        public eStageStateFlag StateFlag;
    }

    [Flags]
    public enum eStageStateFlag
    {
        None = 1,
        Locked = 2,
    }
    [System.Serializable]
    public struct StageElementData
    {
        public StageElementType ElementType;
        
        public StageElementData[] ConnectedElements;
    }

    public enum StageElementType
    {
        Regular,
        Gold,
        Diamond,
        Boss
    }
    
}
#if UNITY_EDITOR

#endif