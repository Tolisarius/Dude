using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotspotHandler : MonoBehaviour {
    public GameObject itemOnHotspot;
    GameObject prompt, spawner,client;
    GameObject spawned;
    bool readInput;



	// Use this for initialization
	void Start ()
    {
        prompt = gameObject.transform.FindChild("prompt").gameObject;
        spawner = gameObject.transform.FindChild("spawner").gameObject;
        SpawnObject();
    }

    // Update is called once per frame
    void Update()
    {
        if (readInput && (Input.GetKeyDown(KeyCode.E)|| Input.GetButtonDown("Fire3")))
        {
            
            client.SendMessageUpwards("HotSpotItemHandler", gameObject);
        }
	}

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {           
            prompt.active = true;
            readInput = true;
            client = col.gameObject;
        }
    } 

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            prompt.active = false;
            readInput = false;
            client = null;
        }
    }

    public void SpawnObject()
    {
        spawned=Instantiate(itemOnHotspot,gameObject.transform);
        spawned.transform.position = spawner.transform.position;
    }

    public void DestroySpawnedObject()
    {
        Destroy(spawned);
    }

}
