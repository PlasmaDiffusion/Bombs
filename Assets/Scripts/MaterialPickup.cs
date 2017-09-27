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

			//First see if the player has the material id in the inventory
			for (int i = 0; i < 4; i++)
			{

				//If they do add onto it
				if (player.materialID[i] == materialNo)
				{
					player.materialCount[i] += 1;
					player.setMaterialCountText(i);

					break;
				}



			}


			//If not then find an empty slot to add it to
			for (int i = 0; i < 4; i++) 
			{
				if (player.materialID[i] == 0)
				{
					player.materialID [i] = materialNo;
					player.materialCount[i] += 1;
					player.setMaterialCountText(i);

					break;
				}
			}


            Destroy(gameObject);
        }

    }
}
