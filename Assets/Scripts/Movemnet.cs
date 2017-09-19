﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Movemnet : MonoBehaviour {

    Rigidbody m_Rigidbody;
    public float m_Speed;   //Public stuffs go in the inspector
    public float m_MaxSpeed;
    public float m_JumpSpeed;
    public float m_RotationSpeed;
    bool canThrow;
    public float throwRechargeTime;
    private float throwRecharge;
    //AudioSource m_audioSource;

    //Material variables for crafting
    public int[] materialCount;
    public bool[] activeCraftingMaterial;
    public Text[] textForHUD;

    //Material Pickup Count for the HUD. (Can't be made into array...?)

    public GameObject bombHandlerReference;

    private GameObject bomb;
    private BombCraftingHandler bombHandler;
    


    // Use this for initialization
    void Start () {

        m_Rigidbody = gameObject.GetComponent<Rigidbody>(); //Get rigid body for movemnt stuff below
        canThrow = true;
       bombHandlerReference = GameObject.FindGameObjectsWithTag("BombList")[0];
       bombHandler  = bombHandlerReference.GetComponent<BombCraftingHandler>();


        //Inventory stuff
        for (int i = 0; i < 4; i++)
        {
            materialCount[i] = 0;
            textForHUD[0].text = materialCount[i].ToString();
            activeCraftingMaterial[i] = false;

        }
       
    }
	
	// Update is called once per frame
	void Update () {
		
        if(Input.GetKey(KeyCode.W))
        {
            m_Rigidbody.velocity += transform.forward * m_Speed * Time.deltaTime;
            float l_y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_MaxSpeed);
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, l_y, m_Rigidbody.velocity.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            m_Rigidbody.velocity += -transform.forward * m_Speed * Time.deltaTime;
            float l_y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_MaxSpeed);
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, l_y, m_Rigidbody.velocity.z);
        }

        if (Input.GetKey(KeyCode.D))
        {
            m_Rigidbody.AddTorque(transform.up * m_RotationSpeed);
        }


        if (Input.GetKey(KeyCode.A))
        {
            m_Rigidbody.AddTorque(transform.up * -m_RotationSpeed);
        }

        //Throw a bomb
        if (Input.GetKey(KeyCode.Space) && canThrow)
        {
            //Add crafting modifications here!


            craftBomb();

            //Now prevent more from being thrown for a moment
            throwRecharge = throwRechargeTime;
            canThrow = false;

        }

        //Check if able to throw again
        if (throwRecharge > 0)
        {
            throwRecharge -= 1 * Time.deltaTime;
            //Debug.Log(throwRecharge);

            if (throwRecharge <= 0) canThrow = true;
        }

        //Toggle crafting material 0
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            toggleCraftingMaterials(0);
        }

        //Toggle crafting material 1
        if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            toggleCraftingMaterials(1);
        }

        //Toggle crafting material 2
        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            toggleCraftingMaterials(2);
        }
        //Toggle crafting material 3
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            toggleCraftingMaterials(3);
        }
    }

    void toggleCraftingMaterials(int materialNo)
    {

        //Toggle crafting item to On  (But not if the player has none of that material)
        if (!activeCraftingMaterial[materialNo] && materialCount[materialNo] > 0)
        {
            activeCraftingMaterial[materialNo] = true;

            //Colour text for active materials
            switch (materialNo)
            {
                case 0:
                    textForHUD[0].color = new Color(0.0f, 0.0f, 1.0f);
                    break;
                case 1:
                    textForHUD[1].color = new Color(1.0f, 0.0f, 0.0f);
                    break;
                case 2:
                    textForHUD[2].color = new Color(1.0f, 1.0f, 0.0f);
                    break;
                case 3:
                    textForHUD[3].color = new Color(0.0f, 1.0f, 0.0f);
                    break;
            }

        }
        else
        {
            activeCraftingMaterial[materialNo] = false;

            //Make text white for unactive materials
            textForHUD[materialNo].color = new Color(1.0f, 1.0f, 1.0f);
        }

        //Make sure number is correct on HUD
        setMaterialCountText(materialNo);
    }

    void OnCollisionEnter(Collision collision)
    {
      
        //Access all contact points from collision
        if (collision.contacts.Length > 0)
        {
            Debug.Log("Collision");
            if (Vector3.Dot(transform.up, collision.contacts[0].normal) > 0.5f)
            {
                Debug.Log("Grounded");
                
            }
        }


    }

    public void setMaterialCountText(int slotNumber)
    {
        textForHUD[slotNumber].text = materialCount[slotNumber].ToString();

    }

    //Make a bomb
    void craftBomb()
    {
        //First get a few transformations ready for spawning the bomb
        Transform bombTransform = transform;
        

        Vector3 forwardOffset = transform.forward;

        bombTransform.position.Set(bombTransform.position.x + forwardOffset.x, bombTransform.position.y + 5.0f, bombTransform.position.z + forwardOffset.z);

        
       
        //Now see what bomb the player is going to craft
        bomb = bombHandler.getSpecificBomb(activeCraftingMaterial);

        //The new bomb is ready to be spawned!
        GameObject newBomb = Instantiate(bomb, forwardOffset + transform.position, transform.rotation);


        Vector3 newVelocity = forwardOffset.normalized;
        newVelocity.y += 5.0f; //Make it arc upwards a little

        newBomb.GetComponent<Rigidbody>().velocity = newVelocity;


        Bomb craftedBomb = newBomb.GetComponent<Bomb>();

        //Basic parameters all bombs have are changed here, but not the type of bomb.
        if (activeCraftingMaterial[0])
        {
            Debug.Log("0");
            //Explode faster! Last slightly longer!
            craftedBomb.explosionScaleSpeed = new Vector3(6.0f, 6.0f, 6.0f);
            craftedBomb.explosionLifetime += 3.0f;
            craftedBomb.time -= 1.5f;
       
            //Use up material
            materialCount[0] -= 1;
            //If empty toggle material to no longer be crafted with
            if (materialCount[0] == 0) toggleCraftingMaterials(0);

            setMaterialCountText(0);
        }
        if (activeCraftingMaterial[1])
        {
            Debug.Log("1");
            //Double the radius, and double the speed!
            craftedBomb.explosionScaleLimit += 10.0f;
            craftedBomb.explosionScaleSpeed = new Vector3(8.0f, 8.0f, 8.0f);

            //Use up material
            materialCount[1] -= 1;
            //If empty toggle material to no longer be crafted with
            if (materialCount[1] == 0) toggleCraftingMaterials(1);

            setMaterialCountText(1);
        }
        if (activeCraftingMaterial[2])
        {
            Debug.Log("2");
            //Cover more of the ground!
            craftedBomb.explosionScaleSpeed += new Vector3(8.0f, 0.0f, 8.0f);
            craftedBomb.explosionScaleLimit += 6.0f;

            //Use up material
            materialCount[2] -= 1;
            //If empty toggle material to no longer be crafted with
            if (materialCount[2] == 0) toggleCraftingMaterials(2);

            setMaterialCountText(2);
        }
        if (activeCraftingMaterial[3])
        {

            Debug.Log("3");
            //Large lifetime boost
            craftedBomb.explosionLifetime += 10.0f;

            //Use up material
            materialCount[3] -= 1;
            //If empty toggle material to no longer be crafted with
            if (materialCount[3] == 0) toggleCraftingMaterials(3);

            setMaterialCountText(3);
        }
    }

}