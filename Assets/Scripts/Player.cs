﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public string playerInputString = ""; //String to be added onto local input
    private bool player1;

    public GameObject cameraHandler;

    [HideInInspector]
    public CharacterController controller;


    public float m_Speed;   //Public stuffs go in the inspector
    public float m_MaxSpeed;
    public float m_JumpSpeed;
    public float m_JumpLimit;
    public float m_RotationSpeed;
    public float m_Gravity;
    public float m_GravityLimit;

    private float forwardFaceVector;
    private float rightFaceVector;

    bool canThrow;
    [HideInInspector]
    public bool firstThrow = true;
    public float throwRechargeTime;
    private float throwRecharge;
    private float throwingPower;

    private float jumpVelocity = 0.0f;
    private float jumpOffset = 0.0f;
    private float velocityUp;

    [HideInInspector]
    public Vector3 explosionForce;

    private bool[] dpadReleased = new bool[4]; //In case of accidental crafting from holding the button more than a frame


    private float health;
    private bool dyingAnimation;

    private GameObject throwBar;

    //Status effects
    private float stunned;
    private float burning;


    //Array for inventory.
    BombAttributes.BombData[] craftedBombs = new BombAttributes.BombData[5];

    //Current bomb being crafted.
    BombAttributes.BombData newerBomb;

    //Material variables for crafting
    public int[] materialCount;
    public int[] materialID;
    public bool[] activeCraftingMaterial;

    public GameObject hudReference;

     Text[] textForHUD;
     Text[] textForInventory;
     Text healthText;
     Image selectedBombImage;
    [HideInInspector]
    public Image[] materialImages;
     Image[] inventoryImages;


    public GameObject bombHandlerReference;


    private GameObject bomb;
    private BombCraftingHandler bombHandler;

    int selectedBomb;


    public GameObject fireEmitterReference;
    private GameObject childFireEmitter;

    //sound effects
    public AudioClip craftComplete;
    public AudioClip addStack;


    private ReadAndWriteStats statManager;

    // Use this for initialization
    void Start() {

        findHUD();

        //Determine if player 1 for key input purposes. Reverse the values to give keyboard input to player 2.
        if (playerInputString == "") player1 = true;
        else player1 = false;

        //Initialize some stuff
        //m_Rigidbody = gameObject.GetComponent<Rigidbody>(); //Get rigid body for movemnt stuff below
        controller = gameObject.GetComponent<CharacterController>();
        canThrow = true;
        bombHandlerReference = GameObject.FindGameObjectsWithTag("BombList")[0];
        bombHandler = bombHandlerReference.GetComponent<BombCraftingHandler>();
        selectedBomb = 0;

        statManager = GameObject.Find("GameManager").GetComponent<ReadAndWriteStats>();

        explosionForce = new Vector3(0.0f, 0.0f, 0.0f);

        throwBar = cameraHandler.transform.GetChild(0).gameObject;

        //Inventory stuff
        for (int i = 0; i < 4; i++)
        {
            materialCount[i] = 0;
            materialID[i] = 0;
            textForHUD[0].text = materialCount[i].ToString();
            activeCraftingMaterial[i] = false;

            dpadReleased[i] = false;
        }

        //Make sure bomb inventory structure stuff are at default values.
        for (int i = 0; i < 5; i++)
        {
            makeBombDefaults(ref craftedBombs[i]);
        }
        makeBombDefaults(ref newerBomb);
        setInventoryText();

        dyingAnimation = false;

        throwingPower = -0.1f;
        health = 100.0f;
        //Status effect stuff
        stunned = 0.0f;
        burning = 0.0f;
    }

    // Update is called once per frame
    void Update() {

        manageStatusEffects();



        //Don't have any input if stunned
        if (stunned > 0.0f) {
            
            //Still make explosions do stuff though
            applyExplosionForce();
            controller.Move(new Vector3(0.0f, -0.1f, 0.0f));

            return;
                }


        //Also don't have any input if in the middle of dying :P
        if (dyingAnimation)
        {
            //Scale the player down
            Transform mesh = transform.GetChild(0).transform;
           mesh.localScale = mesh.localScale * 0.9f;

            //Once the player is scaled down detach the camera
            if (mesh.localScale.y < 0.1f)
            {
                cameraHandler.transform.GetChild(1).transform.parent = null;
                Destroy(gameObject);
            }

            return;
        }

        //Player dies if fallen
        if (transform.position.y < -50.0f)
        {
            damage(100.0f);
            checkIfDead();
        }

        //Movement input
        float velocityForward = (Input.GetAxis("Vertical" + playerInputString) * m_Speed) * Time.deltaTime;
        float velocityRight = (Input.GetAxis("Horizontal" + playerInputString) * m_Speed) * Time.deltaTime;

        if (velocityForward != 0.0f || velocityRight != 0.0f) { forwardFaceVector = velocityForward; rightFaceVector = velocityRight; }

        /* transform.rotation = new Quaternion(transform.rotation.x,
             Mathf.LerpAngle(transform.rotation.y, cameraHandler.transform.rotation.y, Time.deltaTime),
             transform.rotation.z,
             transform.rotation.w);*/

        /* if ((Input.GetAxis("Vertical" + playerInputString) == 0.0f && Input.GetAxis("Horizontal" + playerInputString) == 0.0f))
                     m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x / 2.0f, m_Rigidbody.velocity.y, m_Rigidbody.velocity.z / 2.0f);
 */


        //float oldYVel = m_Rigidbody.velocity.y;

        if (!firstThrow) //Lock movement that isn't related to turning if yet to throw spawn bomb
        {
            //m_Rigidbody.velocity += (transform.forward * velocityForward);
            //m_Rigidbody.velocity += (transform.right * velocityRight);
            //m_Rigidbody.velocity = Vector3.ClampMagnitude(m_Rigidbody.velocity, m_MaxSpeed);


            //Don't clamp y velocity
            //m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, oldYVel, m_Rigidbody.velocity.z);

            if (velocityUp > -m_Gravity)
            velocityUp -= (m_Gravity * Time.deltaTime);


            //Jump pressed
            if (((Input.GetKeyDown(KeyCode.LeftControl) && player1) || Input.GetButtonDown("Confirm" + playerInputString)) && controller.isGrounded)
            {


                jumpVelocity = m_JumpSpeed;
                velocityUp = 0.0f;
                

            }
            
            //Jump release
            if ((Input.GetKeyUp(KeyCode.LeftControl) && player1) || Input.GetButtonUp("Confirm" + playerInputString))
            {

                if (jumpVelocity > 0) jumpVelocity /= 2.0f;
            }

            if (jumpVelocity > 0.0f) jumpVelocity -= (1.0f);
            else jumpVelocity = 0.0f;

            if (velocityUp < m_JumpLimit)
            velocityUp += (jumpVelocity * Time.deltaTime);





            //Debug.Log(jumpVelocity);

            //controller.SimpleMove(new Vector3(velocityRight, 0.0f, velocityForward));
            controller.Move(cameraHandler.transform.forward * velocityForward);
            controller.Move(cameraHandler.transform.right * velocityRight);
            applyExplosionForce();
            controller.Move(new Vector3(0.0f,velocityUp, 0.0f));


         


        }

        Vector3 movement = (cameraHandler.transform.forward * forwardFaceVector) + (cameraHandler.transform.right * rightFaceVector);
        transform.rotation = Quaternion.LookRotation(movement);


        /*/Rotation/camera input
        float rotate = Input.GetAxis("RightStickH" + playerInputString);

		if (rotate > 0.6f || rotate < -0.6f) transform.Rotate(0, rotate * m_RotationSpeed, 0);//m_Rigidbody.AddTorque(transform.up * rotate * m_RotationSpeed);


        if (Input.GetKey(KeyCode.L) && player1)
		{
            transform.Rotate(0, m_RotationSpeed, 0);
            //m_Rigidbody.AddTorque(transform.up * m_RotationSpeed);
        }


		if (Input.GetKey(KeyCode.J) && player1)
		{
            transform.Rotate(0, -m_RotationSpeed, 0);
            //m_Rigidbody.AddTorque(transform.up * -m_RotationSpeed);
        }*/


        //------------------------------------------------------------------------------

        //Cycle through inventory
        if ((Input.GetKeyUp(KeyCode.E) && player1) || (Input.GetButtonUp("CycleRight" + playerInputString)))
        {
            selectedBomb++;

            if (selectedBomb > 4)
            {
                selectedBomb = 0;
            }
            //Debug.Log(selectedBomb.ToString() + " " + playerInputString);

            selectedBombImage.rectTransform.position = inventoryImages[selectedBomb].rectTransform.position;
            
        }
        //Cycle through inventory
        else if ((Input.GetKeyUp(KeyCode.Q) && player1) || (Input.GetButtonUp("CycleLeft" + playerInputString)))
        {
            selectedBomb--;

            if (selectedBomb < 0)
            {
                selectedBomb = 4;
            }
            //Debug.Log(selectedBomb);

            selectedBombImage.rectTransform.position = inventoryImages[selectedBomb].rectTransform.position;
        }

        //Craft a bomb
        if ((Input.GetKey(KeyCode.LeftShift) && player1) || Input.GetButtonUp("Craft" + playerInputString))
			{
            //Debug.Log(" " + playerInputString);

				if (newerBomb.hasIngredient)
				{

					//Find an empty slot to add the new bomb into
					for (int i = 0; i < craftedBombs.Length; i++)
					{
						if (craftedBombs[i].count == 0)
						{
						craftedBombs[i] = newerBomb;
						craftedBombs[i].count++;

                        AudioSource.PlayClipAtPoint(craftComplete, transform.position);

                        setInventoryText();

                        //Erase current bomb
                        makeBombDefaults(ref newerBomb);
                        //Debug.Log(craftedBombs[i].count);

                        statManager.bombsCrafted++;
						break;

                        }
					}
				
				}

			}
        //------------------------------------------------------------------------------

        //Prepare to throw bomb
        if ((Input.GetKey(KeyCode.Space) && player1) || Input.GetAxis("Throw" + playerInputString) > 0.5)
        {

            if (throwingPower < 2.0f) throwingPower += (2.0f * Time.deltaTime);
            
            throwBar.transform.localScale = new Vector3(throwingPower, 0.1f, 0.1f);
            

        }
        //Debug.Log(Input.GetAxis("Throw" + playerInputString).ToString());
        //Throw a bomb
        if (((Input.GetKeyUp(KeyCode.Space) && player1) || (Input.GetAxis("Throw" + playerInputString) < 0.5 && Input.GetAxis("Throw" + playerInputString) > 0.0)) && canThrow && throwingPower > 0.0f)
        {

            throwBar.transform.localScale = new Vector3(0.0f, 0.1f, 0.1f);

            //Special warp bomb for first throw
            if (firstThrow)
            {
                canThrow = false;
                throwRecharge = throwRechargeTime;
                throwBomb(true);

                throwBar.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f));
            }
            else
            {
                throwBomb();

                //Now prevent more from being thrown for a moment
                throwRecharge = throwRechargeTime;
                canThrow = false;

                throwBar.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.2f, 0.2f, 0.2f));
            }

            

        }

        //Check if able to throw again
        if (throwRecharge > 0)
        {
            throwRecharge -= 1 * Time.deltaTime;
            //Debug.Log(throwRecharge);

            if (throwRecharge <= 0)
            {
                canThrow = true;
                throwBar.GetComponent<Renderer>().material.SetColor("_Color", new Color(1.0f, 1.0f, 1.0f));
            }
        }

        //Dpad/arrow keys crafting input -------------------------------------------------

        //Toggle crafting material 0
        if ((Input.GetKeyUp(KeyCode.UpArrow) && player1) || (dpadReleased[0] && Input.GetAxis("D-PadVertical" + playerInputString) == 1))
        {
            addMaterial(materialID[0], 0);
            dpadReleased[0] = false;
        }

        //Toggle crafting material 1
        if ((Input.GetKeyUp(KeyCode.DownArrow) && player1) || (dpadReleased[1] && Input.GetAxis("D-PadVertical" + playerInputString) == -1))
        {
            addMaterial(materialID[1], 1);
            dpadReleased[1] = false;
        }

        //Toggle crafting material 2
        if ((Input.GetKeyUp(KeyCode.RightArrow) && player1) || (dpadReleased[2] && Input.GetAxis("D-PadHorizontal" + playerInputString) == 1))
        {
            addMaterial(materialID[2], 2);
            dpadReleased[2] = false;
        }
        //Toggle crafting material 3
        if ((Input.GetKeyUp(KeyCode.LeftArrow) && player1) || (dpadReleased[3] && Input.GetAxis("D-PadHorizontal" + playerInputString) == -1))
        {
            addMaterial(materialID[3], 3);
            dpadReleased[3] = false;
        }

        //Dpad input doesn't detect if a button was released. These if statements will emulate that. This prevents all materials of the same type from being added immediately.
        if (Input.GetAxis("D-PadVertical" + playerInputString) != 1) dpadReleased[0] = true;
        if (Input.GetAxis("D-PadVertical" + playerInputString) != -1) dpadReleased[1] = true;
        if (Input.GetAxis("D-PadHorizontal" + playerInputString) != 1) dpadReleased[2] = true;
        if (Input.GetAxis("D-PadHorizontal" + playerInputString) != -1) dpadReleased[3] = true;
    }

    //Toggling isn't a thing anymore, so this will be unused.
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
            //Debug.Log("Collision");
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

    void addMaterial(int newMaterialID, int materialSlot)
    {
        //Don't do anything if the material slot is actually empty OR the bomb already has 4 materials added to it
        if (materialCount[materialSlot] <= 0 || newerBomb.materialsAdded > 3) return;

        AudioSource.PlayClipAtPoint(addStack, transform.position);

        newerBomb.materialsAdded += 1;
        //Every material's added effect happens here!
        switch (newMaterialID)
        {
            
            default:
                break;
            case 1:
                newerBomb.freeze += 1;
                statManager.iceMaterialsUsed++;

                break;
            case 2:
                newerBomb.fire += 1;
                statManager.fireMaterialsUsed++;
                
                break;
            case 3:
                newerBomb.smoke += 1;
                statManager.smokeMaterialsUsed++;

                break;
            case 4:
                newerBomb.explosionScaleLimit += 2.0f;
                statManager.explosionMaterialsUsed++;
                break;
            case 5:
                newerBomb.blackhole += 1;
                statManager.blackholeMaterialsUsed++;

                break;
            case 6:
                newerBomb.scatter += 1;
                statManager.scatterMaterialsUsed++;

                break;
        }
        
        //Add in id to display image later
        newerBomb.materialIDs[newerBomb.materialsAdded - 1] = (newMaterialID - 1);

        Debug.Log(newerBomb.materialIDs[newerBomb.materialsAdded - 1]);

        //Set the flag for the new bomb to now be craftable
        newerBomb.hasIngredient = true;

        //Remove material
        materialCount[materialSlot]--;

        //Remove material type from HUD and inventory if all out
        if (materialCount[materialSlot] == 0) materialID[materialSlot] = 0;
        

        setInventoryText();

        //Update text
        setMaterialCountText(materialSlot);
        

    }

    //Make a bomb
    void throwBomb(bool first = false)
    {
        statManager.bombsThrown++;

        //First get a few transformations ready for spawning the bomb
        Transform bombTransform = transform;
        

        Vector3 forwardOffset = transform.forward;

        bombTransform.position.Set(bombTransform.position.x + forwardOffset.x, bombTransform.position.y + 5.0f, bombTransform.position.z + forwardOffset.z);

        
        
       
        //Now see what bomb the player is going to craft
        bomb = bombHandler.getSpecificBomb(activeCraftingMaterial);

        //The new bomb is ready to be spawned!
        GameObject newBomb = Instantiate(bomb, forwardOffset + transform.position, transform.rotation);

        Bomb newBombClass = newBomb.GetComponent<Bomb>();
        newBombClass.ThrowingPlayer = gameObject;

        if (craftedBombs[selectedBomb].count > 0)
        { 

        //Transfer all bomb attributes here from the player's inventory data to the new bomb
        newBombClass.attributes = craftedBombs[selectedBomb];

        //Reset bomb now that it's been used

        craftedBombs[selectedBomb].count--;

            makeBombDefaults(ref craftedBombs[selectedBomb]);

            setInventoryText();
        }
        else
        {
            //If an empty inventory slot was selected, throw in a weak regular bomb
            newBombClass.attributes = default(BombAttributes.BombData);
            makeBombDefaults(ref newBombClass.attributes);
        }

        if (first)
        {
            newBombClass.First = true;
            newBombClass.attributes.MaxRange = 5.0f;
            newBombClass.time = throwingPower * newBombClass.attributes.MaxRange * 0.5f;
        }
        Vector3 newVelocity = forwardOffset.normalized * ((throwingPower * newBombClass.attributes.MaxRange) + 0.5f);
        newVelocity.y += 5.0f; //Make it arc upwards a little

        //Influence throw based on how long the button was held
        newVelocity.y += throwingPower * newBombClass.attributes.MaxRange;

        newBomb.GetComponent<Rigidbody>().velocity = newVelocity;

        throwingPower = -0.1f;
    }

    void makeBombDefaults(ref BombAttributes.BombData bombToReset)
    {
        //Set some stuff to 0 and some specific stuff to 
        bombToReset = default(BombAttributes.BombData);
        bombToReset.explosionScaleSpeed = new Vector3(8.0f, 8.0f, 8.0f);
        bombToReset.explosionScaleLimit = 15.0f;
        bombToReset.explosionLifetime = 3.0f;
        bombToReset.fire = 0;
        bombToReset.freeze = 0;
        bombToReset.blackhole = 0;
        bombToReset.scatter = 0;
        bombToReset.materialsAdded = 0;
        bombToReset.damage = 25.0f;
        bombToReset.MaxRange = 1.0f;

        bombToReset.materialIDs = new int[4];
        bombToReset.materialIDs[0] = -1;
        bombToReset.materialIDs[1] = -1;
        bombToReset.materialIDs[2] = -1;
        bombToReset.materialIDs[3] = -1;
    }

    void setInventoryText()
    {
        for (int i =0; i < 5; i++)
        {
           
                textForInventory[i].text = craftedBombs[i].count.ToString();


                  BombCraftingHandler imagesReference;

                imagesReference = GameObject.Find("BombCraftingHandler").GetComponent<BombCraftingHandler>();



             if (craftedBombs[i].materialsAdded > 0)
            {
               inventoryImages[i].color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

               inventoryImages[i].sprite = imagesReference.bombSprites[craftedBombs[i].materialsAdded - 1];

            }
             else
            {
                inventoryImages[i].color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }

                //Put materials the bomb has too
                for (int j = 0; j < 4; j++)
                {
                    if (craftedBombs[i].materialIDs.Length != 4) break;

                    Image materialImage = inventoryImages[i].rectTransform.GetChild(j).GetComponent<Image>();

                    if (craftedBombs[i].materialIDs[j] != -1)
                    {
                        materialImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                        materialImage.sprite = imagesReference.matTextures[craftedBombs[i].materialIDs[j]];
                    }
                    else
                    {
                        //Make invisible if nothings there
                        materialImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                        materialImage.sprite = null;
                    }
                }

            
            

        }

    }

    public void damage(float damageAmount)
    {
        health -= damageAmount;
        healthText.text = health.ToString();
    }

    public void checkIfDead()
    {
        //If the player did die...
        if (health <= 0)
        {
            //Play a dying animation (shrinking)
            dyingAnimation = true;

            //And check if there's a winner now
            GameManager manager = GameObject.Find("GameManager").GetComponent<GameManager>();

            manager.checkForWinner();
        }
    }

    void manageStatusEffects()
    {
        if (burning > 0.0f)
        {
            
            damage(1.0f * Time.deltaTime);
            burning -= 1.0f * Time.deltaTime;
            childFireEmitter.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
            //Get rid of fire particles
            if (burning <= 0.0f)
            {
                Destroy(childFireEmitter);
            }
        }

        if (stunned > 0.0f)
        {
            stunned -= 1.0f * Time.deltaTime;

            //Recolour if run out
            if (stunned <= 0.0f)
            {

                

                //Renderer rend = GetComponent<Renderer>();
                Renderer rend = transform.GetChild(2).GetComponent<Renderer>();
                rend.material.SetColor("_Color", new Color(0.309f, 0.0f, 0.0f));
                if (!player1)
                {
                    rend.material.SetColor("_Color", new Color(1.0f, 1.0f, 0.317f));
                }

            }
        }
    }

    //Add a status effect based on a number. Duration is based on ticks * delta time
    public void addStatusEffect(int statusID, float duration)
    {
        switch (statusID)
        {
            case 1:

                burning = duration;

                if (childFireEmitter == null)
                childFireEmitter = Instantiate(fireEmitterReference, transform);
   

                break;

            case 2:

                stunned = duration;
                //Renderer rend = GetComponent<Renderer>();
                Renderer rend = transform.GetChild(2).GetComponent<Renderer>();
                rend.material.SetColor("_Color", Color.cyan);

                break;


        }
    }

    void applyExplosionForce()
    {

        if (explosionForce.x != 0.0f) explosionForce.x *= 0.9f;

        if (explosionForce.y != 0.0f) explosionForce.y *= 0.9f;

        if (explosionForce.z != 0.0f) explosionForce.z *= 0.9f;

        controller.Move(explosionForce);
    }

    void findHUD()
    {
        Transform wheel = hudReference.transform.Find("HUD_WheelSprite");
        Transform inventory = hudReference.transform.Find("HUD_InventorySprite");
        Transform healthBar = hudReference.transform.Find("HealthBarImage");

        textForHUD = new Text[4];

        //Wheel stuff
        textForHUD[0] = wheel.GetChild(0).GetComponent<Text>();
        textForHUD[1] = wheel.GetChild(1).GetComponent<Text>();
        textForHUD[2] = wheel.GetChild(2).GetComponent<Text>();
        textForHUD[3] = wheel.GetChild(3).GetComponent<Text>();

        materialImages = new Image[4];

        materialImages[0] = wheel.GetChild(4).GetComponent<Image>();
        materialImages[1] = wheel.GetChild(5).GetComponent<Image>();
        materialImages[2] = wheel.GetChild(6).GetComponent<Image>();
        materialImages[3] = wheel.GetChild(7).GetComponent<Image>();


        //Health stuff
        healthText = healthBar.GetChild(0).GetChild(0).GetComponent<Text>();

        //Dreaded Inventory stuff
        selectedBombImage = inventory.GetChild(0).GetComponent<Image>();

        textForInventory = new Text[5];

        textForInventory[0] = inventory.GetChild(1).GetComponent<Text>();
        textForInventory[1] = inventory.GetChild(2).GetComponent<Text>();
        textForInventory[2] = inventory.GetChild(3).GetComponent<Text>();
        textForInventory[3] = inventory.GetChild(4).GetComponent<Text>();
        textForInventory[4] = inventory.GetChild(5).GetComponent<Text>();

        inventoryImages = new Image[5];

        inventoryImages[0] = inventory.GetChild(6).GetComponent<Image>();
        inventoryImages[1] = inventory.GetChild(7).GetComponent<Image>();
        inventoryImages[2] = inventory.GetChild(8).GetComponent<Image>();
        inventoryImages[3] = inventory.GetChild(9).GetComponent<Image>();
        inventoryImages[4] = inventory.GetChild(10).GetComponent<Image>();


    }

    //Getters
    public float getHealth() { return health; }
}