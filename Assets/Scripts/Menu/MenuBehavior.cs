using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehavior : MonoBehaviour {

    public static int numPlayers;

	// Use this for initialization
	void Start () {
		
	}

    public void LoadScene(int scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
    }

    public void ShowPlayerCountButtons()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);

        if (GameObject.Find("Options"))
        GameObject.Find("Options").SetActive(false);
 
    }

    public void SetPlayerCount(int playerCount)
    {
        numPlayers = playerCount;

        //Make black quad visible and remove buttons so the camera actually refreshes properly and doesnt leave an ugly p4 viewport when theres only 3 players >_>
        GameObject.Find("ClearScreen").GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        transform.parent.GetChild(0).gameObject.SetActive(false);
        transform.parent.GetChild(1).gameObject.SetActive(false);
        transform.parent.GetChild(2).gameObject.SetActive(false);
        GameObject.Find("Quit").gameObject.SetActive(false);

        LoadScene(1);
    }

    public void CloseScene()
    {
        Application.Quit();
    }


    void OnMouseUp()
    {
        
    }

	// Update is called once per frame
	void Update () {
		
	}
}
