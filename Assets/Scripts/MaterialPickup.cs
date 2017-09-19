using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPickup : MonoBehaviour {

    private Movemnet player;
    public int materialNo;

    
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
       //Give player item to craft with

        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject.GetComponent<Movemnet>();
            player.materialCount[materialNo] += 1;
            player.setMaterialCountText(materialNo);

            Destroy(gameObject);
        }

    }
}
