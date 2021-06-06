
using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class SelectedUI : MonoBehaviourPunCallbacks
{

    public List<GameObject> VisualElements;

    private void Start()
    {
        for (int i = 0; i < VisualElements.Count; i++)
        {
            if (VisualElements[i] != null)
            {
                VisualElements[i].SetActive(false);
            }
        }
    }

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
        if (propertiesThatChanged.ContainsKey(AsteroidsGame.PLAYER_SELECTELEMENT))
        {
            var Element = (int) propertiesThatChanged[AsteroidsGame.PLAYER_SELECTELEMENT];

            if (Element == 0)
            {
                for (int i = 0; i < VisualElements.Count; i++)
                {
                    if (VisualElements[i] != null)
                    {
                        VisualElements[i].SetActive(false);
                    }
                }
            }
            else
            {
                VisualElements[Element].SetActive(true);
            }

        }
    }
}

#endregion
