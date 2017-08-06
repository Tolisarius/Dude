using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStates : MonoBehaviour {

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

