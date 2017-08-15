using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour {

    /// <summary>
    /// Jetpack stuff
    /// </summary>
    [SerializeField]
    private float fuelInJetpack;

    public float FuelInJetpack
    {
        get { return fuelInJetpack; }
        set
        {
            float jetpackFilledPercent;
            fuelInJetpack = value;
            jetpackFilledPercent = fuelInJetpack / FuelInJetpackMax;
            guiScript.UpdateFuel(jetpackFilledPercent);
        }
    }

    public float FuelInJetpackMax { get; set; }


    /// <summary>
    /// Inventory stuff
    /// </summary>
    [SerializeField]
    private GameObject itemInHand;
    public GameObject ItemInHand
    {
        get { return itemInHand; }
        set
        {
            itemInHand = value;                      
            if (itemInHand !=null)
            {
                print("neco mam!");
                itemInHand.SetActive(false);
                itemInHand.transform.SetParent(gameObject.transform);               //hang it to a Player but invisible   
                guiScript.UpdateHand(itemInHand);
            }
        }
    }

    

    // Locks
    public bool JumpRestricted { get; set; }

    // States
    public bool IsWalking { get; set; }
    public bool IsJumping { get; set; }
    public bool IsFalling { get; set; }

  
    ControllerStates controllerStates;
    Player player;
    GUI guiScript;

    public enum State
    {       
        standing,
        walking,
        jumping,
        falling,       
    }
    

    public State currentState=State.standing;


    void Start()
    {
        controllerStates = GetComponent<ControllerStates>();
        player = GetComponent<Player>();
        guiScript = GameObject.Find("IngameHud").gameObject.GetComponent<GUI>();
    }
}

