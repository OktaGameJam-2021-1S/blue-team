using System.Collections;
using System.Collections.Generic;
using GG.StaticData;
using UnityEngine;

public class BackendController
{
    public virtual IEnumerator GetStaticData(GameSessionManager pSessionManager)
    {
        pSessionManager.ServerState.StaticData.Heroes = Resources.Load<StaticDataScriptable>("StaticData/MockStaticData").staticDataInfoPackage.Heroes;
        pSessionManager.ServerState.StaticData.Stages = Resources.Load<StaticDataScriptable>("StaticData/MockStaticData").staticDataInfoPackage.Stages;
        
        yield return new WaitForSeconds(1f);
    }

    public virtual IEnumerator GetGameState(GameSessionManager pSessionManager)
    {
        var staticData = Resources.Load<StaticDataScriptable>("StaticData/MockStaticData").staticDataInfoPackage;
      
        var heroesData = staticData.Heroes;
        
        var heroesState = new HeroState[heroesData.Length];
        
        for (int i = heroesData.Length - 1; i >= 0; i--)
        {
            int levelIndex = UnityEngine.Random.Range(0, heroesData[i].RequiredExperienceToLevelUp.Length);
            int experience = 0;
            if (levelIndex > 0)
                experience = UnityEngine.Random.Range(heroesData[i].RequiredExperienceToLevelUp[levelIndex - 1], heroesData[i].RequiredExperienceToLevelUp[levelIndex]);
            else
                experience = UnityEngine.Random.Range(0, heroesData[i].RequiredExperienceToLevelUp[levelIndex]);
                
            
            heroesState[heroesData.Length - 1 - i] = new HeroState
            {
                SDID = heroesData[i].SDID,
                LevelIndex = levelIndex,
                Experience = experience,
            };
        }

        pSessionManager.ServerState.GameState.HeroesState = heroesState;
        
        var stageData = staticData.Stages;
        
        var stageState = new StageState[stageData.Length];
        
        for (int i = stageData.Length - 1; i >= 0; i--)
        {
            stageState[stageData.Length - 1 - i] = new StageState
            {
                SDID = stageData[i].SDID,
            };
        }

        pSessionManager.ServerState.GameState.StageStates = stageState;
        
        // Link each hero data index with a hero state index
        TempDataController.MakePairArrayHeroesDataAndState(pSessionManager);
        
        // Link each stage data index with a hero state index
        TempDataController.MakePairArrayStagesDataAndState(pSessionManager);
        
        yield return new WaitForSeconds(0.5f);
    }
    public virtual IEnumerator Login(string pNickName, string pPwd, GameSessionManager pSessionDependency)
    {
        var serverState = pSessionDependency.ServerState;

        serverState.LoggedIn = true;

        serverState.UserData.NickName = pNickName;
        
        serverState.UserData.Inventory = new Dictionary<string, int>
        {
            {"Earth_Jack", 1},
            {"Shori", 1}
        };
        
        yield return new WaitForSeconds(0.8f);
    }
}

public class ServerState
{
    public bool LoggedIn;

    public UserDataInfo UserData = new UserDataInfo();
    
    public UserGameState GameState = new UserGameState();
    
    public StaticDataInfo StaticData = new StaticDataInfo();
}
public class UserDataInfo
{
    public string NickName;

    public Dictionary<string, int> Inventory = new Dictionary<string, int>();
}

public class UserGameState
{
    public HeroState[] HeroesState;

    public StageState[] StageStates;
}

public struct HeroState
{
    public string SDID;
    [Tooltip("The level index represents the index of required experience, to get current level increase 1 to that value")]
    public int LevelIndex;
    public int Experience;
}

public struct StageState
{
    public string SDID;

    public long Completition;
}
