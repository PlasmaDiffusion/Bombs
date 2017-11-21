using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void LoadScene(int scene)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(scene);
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
