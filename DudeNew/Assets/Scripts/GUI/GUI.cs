﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUI : MonoBehaviour {

    public GameObject guiCharge;
    public GameObject guiLife;
    public GameObject guiKey;
    public GameObject hand;

    GameObject fuelBarFill;
    RectTransform fuelBar;
    float fuelBarFilledSize;

    // Use this for initialization
    void Start()
    {
        guiCharge = gameObject.transform.FindChild("gui_charge").gameObject;
        fuelBarFill = guiCharge.transform.FindChild("fuelBarFill").gameObject;
        fuelBar = fuelBarFill.GetComponent<RectTransform>();
        fuelBarFilledSize = fuelBar.sizeDelta.x;

        hand= gameObject.transform.FindChild("hand").gameObject;
        //UpdateFuel(0.5f);
    }

   


    public void UpdateLives()
    {

    }

    public void UpdateFuel(float fuel)
    {
        print("FUEL" + fuel);
        fuel *= fuelBarFilledSize;
        print("Fuel:" + fuel);
        fuelBar.sizeDelta = new Vector2(fuel, 16f);
    }
    public void UpdateHand(GameObject itemInHand)
    {
        PickableItem pickableItem = itemInHand.GetComponent<PickableItem>();
        Sprite itemToInventoryPic = pickableItem.itemInventorySprite;
        Image handImage= hand.GetComponent<Image>();
        handImage.sprite = itemToInventoryPic;
    }

	
	// Update is called once per frame
	void Update () {
	
	}
}
