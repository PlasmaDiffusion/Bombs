using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChanceToDespawn : MonoBehaviour {


	// Use this for initialization
	void Start () {

        //If they're a child of the level nodes then this is required
        transform.parent = null;
        transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);


        //Pickups that have a chance to despawn
        if (Random.Range(0, 2) == 1) Destroy(gameObject);
	}

	
	// Update is called once per frame
	void Update () {
        
    }

}
