using GG.Constants;
using GG.StaticData;
using UnityEngine;

public static class TempDataController 
{
    public static void MakePairArrayStagesDataAndState(GameSessionManager pSessionManager)
    {
        var stageData = pSessionManager.ServerState.StaticData.Stages;
        var stageState = pSessionManager.ServerState.GameState.StageStates;
        var tempStageState = new StageState[stageData.Length];
        
        for (int i = 0; i < stageData.Length; i++)
        {
            bool findFlag = false;
            for (int j = 0; j < stageState.Length; j++)
            {
                if (stageData[i].SDID == stageState[j].SDID)
                {
                    tempStageState[i] = stageState[j];
                    findFlag = true;
                    break;
                }
            }
            if(!findFlag)
                tempStageState[i] = new StageState()
                {
                    SDID = stageData[i].SDID
                };
        }

        pSessionManager.ServerState.GameState.StageStates = tempStageState;
    }
    public static void MakePairArrayHeroesDataAndState(GameSessionManager pSessionManager)
    {
        var heroesData = pSessionManager.ServerState.StaticData.Heroes;
        var heroesState = pSessionManager.ServerState.GameState.HeroesState;
        var tempHeroesState = new HeroState[heroesData.Length];
        
        for (int i = 0; i < heroesData.Length; i++)
        {
            bool findFlag = false;
            for (int j = 0; j < heroesState.Length; j++)
            {
                if (heroesData[i].SDID == heroesState[j].SDID)
                {
                    tempHeroesState[i] = heroesState[j];
                    findFlag = true;
                    break;
                }
            }
            if(!findFlag)
                tempHeroesState[i] = new HeroState
                {
                    SDID = heroesData[i].SDID
                };
        }

        pSessionManager.ServerState.GameState.HeroesState = tempHeroesState;
    }
    public static HeroData GetSelectedCharacter(ServerState pServerState)
    {
        string characterId = PlayerPrefs.GetString(K.Prefs.kLastCharacterSelected, pServerState.StaticData.Heroes[0].SDID);
        
        return GetCharacterData(characterId, pServerState);
    }
    public static HeroData GetCharacterData(string pCharacterId, ServerState pServerState)
    {
        for (int i = 0; i < pServerState.StaticData.Heroes.Length; i++)
        {
            if (pCharacterId == pServerState.StaticData.Heroes[i].SDID)
            {
                return pServerState.StaticData.Heroes[i];
            }
        }
        return new HeroData();
    }
}
