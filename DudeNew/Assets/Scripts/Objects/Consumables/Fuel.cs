using UnityEngine;
using System.Collections;

public class Fuel : MonoBehaviour {
	public float reactivateTime;
    PlayerStates playerState;

    void OnTriggerEnter2D(Collider2D col){
        if (col.tag == "Player")
        {           
            Refill(col.gameObject);
		}
	}

	void Refill(GameObject col)
     {  
        playerState=col.GetComponent<PlayerStates>();
        if (playerState.FuelInJetpack <= 0)
        {
            playerState.FuelInJetpack = playerState.FuelInJetpackMax;
            HideObject();
            Invoke("UnhideObject", reactivateTime);
        }        
    }
    

//SWITCHING OBJECT OFF AND AFTER TIMEOUT ON AGAIN

	void HideObject(){
		gameObject.GetComponent<Renderer> ().enabled=false;
		gameObject.GetComponent<Collider2D>().enabled=false;
	}

	void UnhideObject(){
		gameObject.GetComponent<Renderer> ().enabled=true;
		gameObject.GetComponent<Collider2D>().enabled=true;
	}

}
