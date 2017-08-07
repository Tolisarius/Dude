using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [HideInInspector]
    public string itemHeld;
    [HideInInspector]
    //public string itemToDrop, itemToPick;
    GameObject itemToPick, itemToDrop;
    GameObject itemOnHotspot;

    PlayerStates playerStates;



    // Use this for initialization
    void Start()
    {
        playerStates = gameObject.GetComponent<PlayerStates>();
    }


    public void ItemPick(GameObject hotspot)
    {
        HotspotHandler hotspotHandler = hotspot.GetComponent<HotspotHandler>();                     //script of the activated hotspot      
        Transform[] ts = hotspot.GetComponentsInChildren<Transform>();       
        foreach (Transform t in ts)
        {
                       
            if (t.tag=="Item")
            {
                itemOnHotspot = t.gameObject;                          
            }
        }

        if (itemOnHotspot != null)
        {
            itemToPick = itemOnHotspot;
        }

        if (itemToPick != null)
        {
            print("Neco jsem sebral! A je to" + itemToPick.tag);
            itemToPick.SetActive(false);
            itemToPick.transform.SetParent(gameObject.transform);               //hang it to a Player but invisible           
        }
    }

}

    

