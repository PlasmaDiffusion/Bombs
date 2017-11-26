using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitScreen : MonoBehaviour {

    public Camera cam1;
    public Camera cam2;
    public Camera cam3;
    public Camera cam4;

    public int numLocalPlayers = 1;
    public bool horizontal = false;

	// Use this for initialization
	void Start () {

        numLocalPlayers = MenuBehavior.numPlayers;
	
        //1 screen for 1 player
        if (numLocalPlayers <= 1)
        {
            //Destroy P2
            Destroy(GameObject.Find("Player2"));
            Destroy(GameObject.Find("cameraControllerP2"));
            Destroy(GameObject.Find("Player3"));
            Destroy(GameObject.Find("cameraControllerP3"));
            Destroy(GameObject.Find("Player4"));
            Destroy(GameObject.Find("cameraControllerP4"));

            //Resize scren
            cam1.rect = new Rect(0, 0, 1, 1);

            //Remove P2 Hud
            Destroy(GameObject.Find("HUDPrefab2").gameObject);

            //Remove P3 Hud
            Destroy(GameObject.Find("HUDPrefab3").gameObject);

            //Remove P4 Hud
            Destroy(GameObject.Find("HUDPrefab4").gameObject);
        }
        else if (numLocalPlayers ==2)
        {
            //Destroy P
            Destroy(GameObject.Find("Player3"));
            Destroy(GameObject.Find("cameraControllerP3"));
            Destroy(GameObject.Find("Player4"));
            Destroy(GameObject.Find("cameraControllerP4"));

            //Resize scren
            cam1.rect = new Rect(0, 0, 0.5f, 1);
            cam2.rect = new Rect(0.5f, 0, 0.5f, 1);

            //Remove P3 Hud
            Destroy(GameObject.Find("HUDPrefab3").gameObject);

            //Remove P4 Hud
            Destroy(GameObject.Find("HUDPrefab4").gameObject);
        }
        else if (numLocalPlayers == 3)
        {
            Destroy(GameObject.Find("Player4"));
            Destroy(GameObject.Find("cameraControllerP4"));

            //Remove P4 Hud
            Destroy(GameObject.Find("HUDPrefab4").gameObject);
        }

        //Shrink hud and zoom out camera a little if 3 or more players
        if (numLocalPlayers >= 3)
        {
            shrinkHUD(GameObject.Find("HUDPrefab"));
            shrinkHUD(GameObject.Find("HUDPrefab2"));
            shrinkHUD(GameObject.Find("HUDPrefab3"));

            if (numLocalPlayers > 3)
                shrinkHUD(GameObject.Find("HUDPrefab4"));


            zoomOut(GameObject.Find("CameraP1"));
            zoomOut(GameObject.Find("CameraP2"));
            zoomOut(GameObject.Find("CameraP3"));

            if (numLocalPlayers > 3)
                zoomOut(GameObject.Find("CameraP4"));
        }

        //ChangeSplitScreen();
        	
	}

    void shrinkHUD(GameObject hud)
    {
        Transform child1 = hud.transform.GetChild(0).GetComponent<RectTransform>();
        child1.localScale =  new Vector3(child1.localScale.x * 0.75f, child1.localScale.y * 0.75f);


        Transform child2 = hud.transform.GetChild(1).GetComponent<RectTransform>();
        child2.localScale = new Vector3(child2.localScale.x * 0.75f, child2.localScale.y * 0.75f);
        child2.position = new Vector3(child2.position.x -80.0f, child2.position.y + 25.0f);

        Transform child3 = hud.transform.GetChild(2).GetComponent<RectTransform>();
        child3.localScale = new Vector3(child3.localScale.x * 0.75f, child3.localScale.y * 0.75f);
    }
	
    void zoomOut(GameObject cam)
    {
        cam.transform.position += new Vector3(0.0f, 5.0f, -5.0f);
    }

	// Update is called once per frame
	void Update () {
		
	}

    //Unused horizontal option
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
