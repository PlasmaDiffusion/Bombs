using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialPickup : MonoBehaviour {

    private Player player;
    public int materialNo;

    //sound effects
    public AudioClip pickup1;
    public AudioClip pickup2;
    public AudioClip pickup3;
    public AudioClip pickup4;
    public AudioClip pickup5;
    public AudioClip pickup6;
    public AudioClip pickup7;
    public AudioClip pickup8;
    int randPickupSound;

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
                        player.materialCount[i] = 1;
                        player.setMaterialCountText(i);


                        //And also add the image
                        setMaterialImageByType(player, i);
                        break;
                    }
                }
            }

            //pickup sound effects
            randPickupSound = Random.RandomRange(0, 7);

            switch (randPickupSound)
            {
                case 0:
                    AudioSource.PlayClipAtPoint(pickup1, transform.position);
                    break;

                case 1:
                    AudioSource.PlayClipAtPoint(pickup2, transform.position);
                    break;
                case 2:
                    AudioSource.PlayClipAtPoint(pickup3, transform.position);
                    break;
                case 3:
                    AudioSource.PlayClipAtPoint(pickup4, transform.position);
                    break;
                case 4:
                    AudioSource.PlayClipAtPoint(pickup5, transform.position);
                    break;
                case 5:
                    AudioSource.PlayClipAtPoint(pickup6, transform.position);
                    break;
                case 6:
                    AudioSource.PlayClipAtPoint(pickup7, transform.position);
                    break;
                case 7:
                    AudioSource.PlayClipAtPoint(pickup8, transform.position);
                    break;
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
