using System.Collections;
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
    
        public GameObject[] ObstaclesPrefabs;

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

        private IEnumerator SpawnObstacles()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(K.GamePlay.ASTEROIDS_MIN_SPAWN_TIME, K.GamePlay.ASTEROIDS_MAX_SPAWN_TIME));
                //
                // Vector2 direction = Random.insideUnitCircle;
                // Vector3 position = Vector3.zero;
                //
                // if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                // {
                //     // Make it appear on the left/right side
                //     position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, direction.y * Camera.main.orthographicSize);
                // }
                // else
                // {
                //     // Make it appear on the top/bottom
                //     position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0, Mathf.Sign(direction.y) * Camera.main.orthographicSize);
                // }
                //
                // // Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
                // position -= position.normalized * 0.1f;
                //
                //
                // Vector3 force = -position.normalized * 1000.0f;
                // Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                // object[] instantiationData = {force, torque, true};
                //
                // PhotonNetwork.InstantiateRoomObject("BigAsteroid", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
            }
        }

        private IEnumerator EndOfGame(string winner, int score)
        {
            float timer = 5.0f;

            while (timer > 0.0f)
            {
                InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));

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

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(K.GamePlay.PLAYER_LIVES))
            {
                CheckEndOfGame();
                return;
            }

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

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object lives;
                if (p.CustomProperties.TryGetValue(K.GamePlay.PLAYER_LIVES, out lives))
                {
                    if ((int) lives > 0)
                    {
                        allDestroyed = false;
                        break;
                    }
                }
            }

            if (allDestroyed)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    StopAllCoroutines();
                }

                string winner = "";
                int score = -1;

                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.GetScore() > score)
                    {
                        winner = p.NickName;
                        score = p.GetScore();
                    }
                }

                StartCoroutine(EndOfGame(winner, score));
            }
        }

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }
    
    }
}