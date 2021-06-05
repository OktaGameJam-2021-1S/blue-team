using System.Collections;
using System.Collections.Generic;
using GG.Constants;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace GamePlay
{
    public class GamePlayNetworkManager : MonoBehaviourPunCallbacks
    {
        public static GamePlayNetworkManager Instance = null;

        public Text InfoText;

        public Transform SpawnPointWizard;
        public Transform SpawnPointRunner;

        public Transform[] SpawnPointsObstacle;
        
        public GameObject[] ObstaclesPrefabs;

        public GameObject[] GroundsPrefabs;

        public List<ElementType> ElementsToSpawn = new List<ElementType>();

        public ElementSpawn[] ElementsPrefabs;

        public List<ElementSpawn> PoolElements;
        
        public List<Ground> Grounds = new List<Ground>();

        public int startAmountGround = 5;

        public int Score;
        #region UNITY

        public void Awake()
        {
            Instance = this;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }

        public void Start()
        {
            Hashtable props = new Hashtable
            {
                {K.GamePlay.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            
        }

        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }

        #endregion

        #region COROUTINES

        T GetRandomObject<T>(T[] objs)
        {
            return objs[Random.Range(0, objs.Length)];
        }

        private IEnumerator CountPoints()
        {
            while (true)
            {
                yield return new WaitForSeconds(0.1f);

                PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable{{K.GamePlay.SCORE, Score}});
                Score++;
            }
        }
        private IEnumerator SpawnElements()
        {
            while (true)
            {
                if (ElementsToSpawn.Count > 0)
                {
                    yield return new WaitForSeconds(3);
                    var type = ElementsToSpawn[0];
                    bool spawnedFromPool = false;
                    Vector3 position = GetRandomObject(SpawnPointsObstacle).transform.position;
                    position.y = 2;
                    for (int i = 0; i < PoolElements.Count; i++)
                    {
                        if (PoolElements[i].ElementType == type)
                        {
                            PoolElements[i].transform.position = position;
                            spawnedFromPool = true;
                            break;
                        }
                    }

                    if (!spawnedFromPool)
                    {
                        for (int i = 0; i < ElementsPrefabs.Length; i++)
                        {
                            if (ElementsPrefabs[i].ElementType == type)
                            {
                                GameObject obj = PhotonNetwork.InstantiateRoomObject(
                                    ElementsPrefabs[i].name,
                                    position, Quaternion.identity, 0, null);
                                PoolElements.Add(obj.GetComponent<ElementSpawn>());
                                break;
                            }
                        }
                   
                    }
                    ElementsToSpawn.RemoveAt(0);
                }

                yield return null;
            }
        }
        private IEnumerator SpawnGrounds()
        {
            Vector3 GetPositionToSpawn()
            {
                if (Grounds.Count == 0)
                    return Vector3.left * 15;
                return Grounds[Grounds.Count - 1].rightLink.position;
            }
            for (int i = 0; i < startAmountGround; i++)
            {
                GameObject obj = PhotonNetwork.InstantiateRoomObject(GetRandomObject(GroundsPrefabs).name, GetPositionToSpawn(), Quaternion.identity, 0, null);
                Grounds.Add(obj.GetComponent<Ground>());
            }

            while (true)
            {
                if (Grounds[1].rightLink.position.x < 0)
                {
                    Grounds[0].transform.position = GetPositionToSpawn();
                    Grounds.Add(Grounds[0]);
                    Grounds.RemoveAt(0);
                }

                yield return null;
            }
        }
        private IEnumerator SpawnObstacles()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(K.GamePlay.ASTEROIDS_MIN_SPAWN_TIME, K.GamePlay.ASTEROIDS_MAX_SPAWN_TIME));

                GameObject randomObstacle = ObstaclesPrefabs[Random.Range(0, ObstaclesPrefabs.Length)];
                Vector3 position = SpawnPointsObstacle[Random.Range(0, SpawnPointsObstacle.Length)].position;
                Vector3 force = Vector3.zero;
                Vector3 torque = Vector3.zero;
                object[] instantiationData = {force, torque, true};
                
                PhotonNetwork.InstantiateRoomObject(randomObstacle.name, position, Quaternion.identity, 0, instantiationData);
            }
        }

        private IEnumerator EndOfGame(int score)
        {
            float timer = 5.0f;

            while (timer > 0.0f)
            {
                InfoText.text = "FIM DE JOGO SCORE: "+ score ;

                yield return new WaitForEndOfFrame();

                timer -= Time.deltaTime;
            }
        
            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnDisconnected(DisconnectCause cause)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("DemoAsteroids-LobbyScene");
        }

        public override void OnLeftRoom()
        {
            PhotonNetwork.Disconnect();
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                StartCoroutine(SpawnObstacles());
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            CheckEndOfGame();
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            if (propertiesThatChanged.ContainsKey(K.GamePlay.PLAYER_LIVES))
            {
                CheckEndOfGame();
                return;
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }


            // if there was no countdown yet, the master client (this one) waits until everyone loaded the level and sets a timer start
            int startTimestamp;
            bool startTimeIsSet = CountdownTimer.TryGetStartTime(out startTimestamp);

            if (changedProps.ContainsKey(K.GamePlay.PLAYER_LOADED_LEVEL))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        StartCoroutine(SpawnGrounds());
                    }
                    if (!startTimeIsSet)
                    {
                        CountdownTimer.SetStartTime();
                    }
                }
                else
                {
                    // not all players loaded yet. wait:
                    Debug.Log("setting text waiting for players! ",this.InfoText);
                    InfoText.text = "Waiting for other players...";
                }
            }
        
        }

        #endregion

        
        // called by OnCountdownTimerIsExpired() when the timer ended
        private void StartGame()
        {
            Debug.Log("StartGame!");

            // on rejoin, we have to figure out if the spaceship exists or not
            // if this is a rejoin (the ship is already network instantiated and will be setup via event) we don't need to call PN.Instantiate
        


            if (PhotonNetwork.IsMasterClient)
            {
                Vector3 position = SpawnPointWizard.position;
                Quaternion rotation = Quaternion.Euler(0.0f, 0, 0.0f);
                PhotonNetwork.Instantiate("Wizard", position, rotation, 0);      // avoid this call on rejoin (ship was network instantiated before)

                
                StartCoroutine(SpawnObstacles());
                StartCoroutine(SpawnElements());
                StartCoroutine(CountPoints());
            }
            else
            {
                Vector3 position = SpawnPointRunner.position;
                Quaternion rotation = Quaternion.Euler(0.0f, 0, 0.0f);
                PhotonNetwork.Instantiate("Runner", position, rotation, 0);      // avoid this call on rejoin ( was network instantiated before)
            }
        }

        private bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                if (p.CustomProperties.TryGetValue(K.GamePlay.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool) playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        private void CheckEndOfGame()
        {
            bool allDestroyed = true;

            if (PhotonNetwork.IsMasterClient)
            {
                object lives;
                if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(K.GamePlay.PLAYER_LIVES, out lives))
                {
                    if ((int) lives <= 0)
                    {
                        Debug.Log("FIM DO JOGO");
                        
                        StopAllCoroutines();
                        StartCoroutine(EndOfGame(Score));
                    }
                }


            }
        }

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }
    
    }
}