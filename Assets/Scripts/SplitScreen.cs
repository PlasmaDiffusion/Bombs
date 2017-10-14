using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreen : MonoBehaviour {

    public Camera cam1;
    public Camera cam2;

    public int numLocalPlayers = 1;
    public bool horizontal = false;

	// Use this for initialization
	void Start () {
	
        //1 screen for 1 player
        if (numLocalPlayers <= 1)
        {
            //Destroy P2
            Destroy(cam2.transform.parent.gameObject);
            
            //Resize scren
            cam1.rect = new Rect(0, 0, 1, 1);

            //Remove P2 Hud
            Destroy(GameObject.Find("HUDPrefab2").gameObject);
        }

        ChangeSplitScreen();
        	
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    void ChangeSplitScreen()
    {
        if (horizontal)
        {
            cam1.rect = new Rect(0, 0, 1, 0.5f);
            cam2.rect = new Rect(0, 0.5f, 1, 0.5f);
        }
        else //vertical
        {
            cam1.rect = new Rect(0, 0, 0.5f, 1);
            cam2.rect = new Rect(0.5f, 0, 0.5f, 1);
        }
    }
}
