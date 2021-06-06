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
        public Text Amount;
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
        if (propertiesThatChanged.ContainsKey(AsteroidsGame.PLAYER_ELEMENTINFO))
        {
            var Element = (ElementType)propertiesThatChanged[AsteroidsGame.PLAYER_ELEMENTINFO];
            var Amount = (int)propertiesThatChanged[AsteroidsGame.PLAYER_ELEMENTAMOUNT];
            
            var ElementVisual = VisualElements.Find((element)=> element.element == Element);
            ElementVisual.Amount.text = Amount.ToString();
            ElementVisual.Visual.gameObject.SetActive(Amount > 0);
        }
    }


    #endregion
}