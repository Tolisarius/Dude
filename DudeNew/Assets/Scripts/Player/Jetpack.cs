using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jetpack : MonoBehaviour {
    public float jetpackFuelMax;
    float fuelSpend=1f;
    public float jetpackPush;
    bool jetpacking;

    Player player;
    PlayerStates playerStates;
    Vector2 directionalInput;
    bool directionLeft, directionRight, directionDown, directionUp;

    public float horizontalInputMinimum, verticalInputMinimum;
   

    void Start()
    {
        player = GetComponent<Player>();
        playerStates = GetComponent<PlayerStates>();
        playerStates.FuelInJetpackMax = jetpackFuelMax;
        playerStates.FuelInJetpack = 0f;
    }



    private void Update()
    {
        if (jetpacking && playerStates.FuelInJetpack <= 0)
        {
            JetpackOff();
        }
        else if(jetpacking)
        {
            playerStates.FuelInJetpack -= fuelSpend*Time.deltaTime;      //spends x unit of fuel per second
        }
    }

    public void JetpackOn()
    {
        if (playerStates.FuelInJetpack > 0)
        {
            jetpacking = true;
            player.verticalPush = jetpackPush;           
        }
        else
        {
            JetpackOff();
        }
    }

    public void JetpackOff()
    {
        jetpacking = false;
        playerStates.FuelInJetpack = 0f;
        player.verticalPush = 0;
    }    
}
