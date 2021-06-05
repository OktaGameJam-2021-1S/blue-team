// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerNumbering.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player Overview Panel
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace Photon.Pun.Demo.Asteroids
{
    public class PlayerOverviewPanel : MonoBehaviourPunCallbacks
    {
        public GameObject PlayerOverviewEntryPrefab;

        private GameObject entry;

        private int Lives;
        private int Score;

        #region UNITY

        public void Awake()
        {
            entry = Instantiate(PlayerOverviewEntryPrefab);
            entry.transform.SetParent(gameObject.transform);
            entry.transform.localScale = Vector3.one;
            Lives = AsteroidsGame.PLAYER_MAX_LIVES;
            entry.GetComponent<Text>().text = string.Format("Score: {0}\nLives: {1}", 0, Lives);
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {

        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
       
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            base.OnRoomPropertiesUpdate(propertiesThatChanged);
            if (propertiesThatChanged.ContainsKey(AsteroidsGame.PLAYER_LIVES))
            {
                Lives = (int)propertiesThatChanged[AsteroidsGame.PLAYER_LIVES];
            }
            if (propertiesThatChanged.ContainsKey("PlayerScore"))
            {
                Score =(int)propertiesThatChanged["PlayerScore"];
            }

            entry.GetComponent<Text>().text = string.Format("Score: {0}\nLives: {1}", Score, Lives);
        }

        #endregion
    }
}