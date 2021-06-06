
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayUI :  MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public class ElementVisual
    {
        public ElementType element;
        public Image Visual;
    }
    
    public List<ElementVisual> VisualElements;
    
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
        if (propertiesThatChanged.ContainsKey(AsteroidsGame.PLAYER_ELEMENTS))
        {
            var elementsActives = (ElementType)propertiesThatChanged[AsteroidsGame.PLAYER_ELEMENTS];
            foreach (var element in VisualElements)
            {
                element.Visual.gameObject.SetActive((elementsActives & element.element) != 0);
            }
        }
    }


    #endregion
}
