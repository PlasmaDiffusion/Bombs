﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombCraftingHandler : MonoBehaviour {

  //Every special bomb can go here
  public GameObject[] bombs;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public GameObject getSpecificBomb(bool[] usingMaterial)
    {
        int bombToUse = 0;
        string ingredientString = "";

        for (int i = 0; i < usingMaterial.Length; i++)
        {
            if (usingMaterial[i]) ingredientString += "+";
            else ingredientString += "-";

        }

        if (usingMaterial[0] && usingMaterial[1]) bombToUse = 1;

        if (usingMaterial[2] && usingMaterial[3]) bombToUse = 2;

        if (usingMaterial[2] && usingMaterial[1]) bombToUse = 1;


        return bombs[bombToUse];
    }
}
