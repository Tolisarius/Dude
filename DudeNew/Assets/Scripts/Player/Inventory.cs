using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [HideInInspector]
    //public string itemToDrop, itemToPick;
    GameObject itemHeld;
    GameObject itemToPick, itemToDrop;
    GameObject itemOnHotspot;

    PlayerStates playerStates;



    // Use this for initialization
    void Start()
    {
        playerStates = gameObject.GetComponent<PlayerStates>();
    }

   

    public void HotSpotItemHandler(GameObject hotspot)
    {
        ItemHeld();             //what do I have in hand?
        ItemOnHotspot(hotspot);

        HotspotHandler hotspotScript= hotspot.GetComponent<HotspotHandler>();

        if (itemOnHotspot != null)
        {

            playerStates.ItemInHand = itemOnHotspot;

            /*itemToPick = itemOnHotspot;
            itemOnHotspot = null;         
            print("Neco jsem sebral! A je to" + itemToPick.tag);
            */
            

            //itemToPick.SetActive(false);
            //itemToPick.transform.SetParent(gameObject.transform);               //hang it to a Player but invisible           
            
        }
        if (itemHeld != null)
        {
            itemToDrop = itemHeld;
            itemHeld = null;
            itemToDrop.SetActive(true);
            hotspotScript.itemOnHotspot = itemToDrop;
            hotspotScript.SpawnObject();
            Destroy(itemToDrop);
            //itemToDrop.transform.SetParent(hotspot.transform);
        }
    }


    public void ItemOnHotspot(GameObject hotspot)
    {
        Transform[] ts = hotspot.GetComponentsInChildren<Transform>();
        
        foreach (Transform t in ts)                     //is there any item on the hotspot?
        {                      
            if (t.tag=="Item")
            {
                itemOnHotspot = t.gameObject;                          
            }
        }             
    }

    public void ItemHeld()
    {
        foreach (Transform child in transform)
            if (child.CompareTag("Item"))
            {
                itemHeld = child.gameObject;
            }

        /*
        Transform[] th = gameObject.
        itemHeld= gameObject.transform.GetComponentsInChildren<PickableItem>().;
        

        foreach (Transform t in th)                                     //is there any item held by Player?           
        {
            print("Sem nasel:"+ t.tag);
            if (t.tag == "Item")
            {
                print("co maaaa hobitek v rusissce???");
                itemHeld = t.gameObject;
                print("Item v ruce" + itemHeld);

            }
        }
        */
    }

}

    

