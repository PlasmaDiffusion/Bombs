﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPickup : MonoBehaviour {

    private Player player;
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
            bool found = false;

            player = other.gameObject.GetComponent<Player>();

			//First see if the player has the material id in the inventory
			for (int i = 0; i < 4; i++)
			{

				//If they do add onto it
				if (player.materialID[i] == materialNo)
				{
					player.materialCount[i] += 1;
					player.setMaterialCountText(i);
                    found = true;

					break;
				}



			}
            //If not then find an empty slot to add it to
            if (!found)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (player.materialID[i] == 0)
                    {
                        player.materialID[i] = materialNo;
                        player.materialCount[i] += 1;
                        player.setMaterialCountText(i);


                        //And also add the image
                        setMaterialImageByType(player, i);
                        break;
                    }
                }
            }

            Destroy(gameObject);
        }

    }

    void setMaterialImageByType(Player p, int index)
    {
        BombCraftingHandler imagesReference;

        imagesReference = GameObject.Find("BombCraftingHandler").GetComponent<BombCraftingHandler>();

        p.materialImages[index].sprite = imagesReference.matTextures[materialNo - 1];


        
    }
}
