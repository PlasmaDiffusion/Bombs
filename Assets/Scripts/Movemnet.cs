using System.Collections;
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
    private bool firstThrow = true;
    public float throwRechargeTime;
    private float throwRecharge;

    private float health;

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
    public Text[] textForHUD;
    public Text[] textForInventory;
    public Text healthText;
    public Image selectedBombImage;
    public Image[] materialImages;
    

    public GameObject bombHandlerReference;

    private GameObject bomb;
    private BombCraftingHandler bombHandler;
    
	int selectedBomb;

    // Use this for initialization
    void Start () {

        m_Rigidbody = gameObject.GetComponent<Rigidbody>(); //Get rigid body for movemnt stuff below
        canThrow = true;
       bombHandlerReference = GameObject.FindGameObjectsWithTag("BombList")[0];
       bombHandler  = bombHandlerReference.GetComponent<BombCraftingHandler>();
		selectedBomb = 0;

        //Inventory stuff
        for (int i = 0; i < 4; i++)
        {
            materialCount[i] = 0;
			materialID[i] = 0;
            textForHUD[0].text = materialCount[i].ToString();
            activeCraftingMaterial[i] = false;

        }

        //Make sure bomb inventory structure stuff are at default values.
        for (int i = 0; i < 5; i++)
        {
            makeBombDefaults(ref craftedBombs[i]);
        }
        makeBombDefaults(ref newerBomb);
        setInventoryText();

        health = 100.0f;
        //Status effect stuff
        stunned = 0.0f;
        burning = 0.0f;

    }
	
	// Update is called once per frame
	void Update () {

        manageStatusEffects();

        /*if(Input.GetKey(KeyCode.W))
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
        }*/

        //Don't have any input if stunned
        if (stunned > 0.0f)
        {
            return;
        }

		//Movement input
        float velocityForward = Input.GetAxis("Vertical") * m_Speed * Time.deltaTime;
        float velocityRight = Input.GetAxis("Horizontal") * m_Speed * Time.deltaTime;

        m_Rigidbody.velocity += (transform.forward * velocityForward);
		m_Rigidbody.velocity += (transform.right * velocityRight);

		//Rotation/camera input
		float rotate = Input.GetAxis("RightStickH");

		if (rotate > 0.6f || rotate < -0.6f) m_Rigidbody.AddTorque(transform.up * rotate * m_RotationSpeed);


		if (Input.GetKey(KeyCode.L))
		{
			m_Rigidbody.AddTorque(transform.up * m_RotationSpeed);
		}


		if (Input.GetKey(KeyCode.J))
		{
			m_Rigidbody.AddTorque(transform.up * -m_RotationSpeed);
		}

		//------------------------------------------------------------------------------

        //Cycle through inventory
        if (Input.GetButtonUp("CycleRight"))
        {
            selectedBomb++;

            if (selectedBomb > 4)
            {
                selectedBomb = 0;
                selectedBombImage.rectTransform.Translate(-160.0f, 0.0f, 0.0f);
            }
            Debug.Log(selectedBomb);
            
            selectedBombImage.rectTransform.Translate(32.0f, 0.0f, 0.0f);
        }
        //Cycle through inventory
        else if (Input.GetButtonUp("CycleLeft"))
        {
            selectedBomb--;

            if (selectedBomb < 0)
            {
                selectedBomb = 4;
                selectedBombImage.rectTransform.Translate(160.0f, 0.0f, 0.0f);
            }
            Debug.Log(selectedBomb);

            selectedBombImage.rectTransform.Translate(-32.0f, 0.0f, 0.0f);
        }

        //Craft a bomb
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetButtonUp("Confirm"))
			{
				if (newerBomb.hasIngredient)
				{

					//Find an empty slot to add the new bomb into
					for (int i = 0; i < craftedBombs.Length; i++)
					{
						if (craftedBombs[i].count == 0)
						{
						craftedBombs[i] = newerBomb;
						craftedBombs[i].count++;


                        setInventoryText();

                        //Erase current bomb
                        makeBombDefaults(ref newerBomb);
                        Debug.Log(craftedBombs[i].count);
						break;

                    }
					}
				
				}

			}
		//------------------------------------------------------------------------------
        //Throw a bomb
        if ((Input.GetKey(KeyCode.Space) || Input.GetAxis("Throw") > 0 ) && canThrow)
        {
            if (firstThrow)
            {
                firstThrow = false;
                canThrow = false;
                throwRecharge = 10;
                throwBomb(true);
            }
            else
            {
                throwBomb();

                //Now prevent more from being thrown for a moment
                throwRecharge = throwRechargeTime;
                canThrow = false;
            }

            

        }

        //Check if able to throw again
        if (throwRecharge > 0)
        {
            throwRecharge -= 1 * Time.deltaTime;
            //Debug.Log(throwRecharge);

            if (throwRecharge <= 0) canThrow = true;
        }

        //Input needs to be fixed so its on release with the dpad...-----------------------------------------------
        //Toggle crafting material 0
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetAxis("D-PadVertical") == 1)
        {
            addMaterial(materialID[0], 0);
        }

        //Toggle crafting material 1
        if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetAxis("D-PadVertical") == -1)
        {
            addMaterial(materialID[1], 1);
        }

        //Toggle crafting material 2
        if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetAxis("D-PadHorizontal") == 1)
        {
            addMaterial(materialID[2], 2);
        }
        //Toggle crafting material 3
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetAxis("D-PadHorizontal") == -1)
        {
            addMaterial(materialID[3], 3);
        }
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

    void addMaterial(int newMaterialID, int materialSlot)
    {
        //Don't do anything if its zero
        if (materialCount[materialSlot] <= 0) return;

        //Every material's added effect happens here!
        switch (newMaterialID)
        {

            default:
                break;
            case 1:
                newerBomb.freeze += 1;

                break;
            case 2:
                newerBomb.fire += 1;
                
                break;
            case 3:
                newerBomb.explosionScaleLimit += 2.0f;

                break;
            case 4:
                newerBomb.explosionScaleLimit += 2.0f;

                break;
        }

        //Set the flag for the new bomb to now be craftable
        newerBomb.hasIngredient = true;

        //Remove material
        materialCount[materialSlot]--;


        setInventoryText();

        //Update text
        setMaterialCountText(materialSlot);

    }

    //Make a bomb
    void throwBomb(bool first = false)
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


        Bomb newBombClass = newBomb.GetComponent<Bomb>();
        newBombClass.ThrowingPlayer = gameObject;

        if (craftedBombs[selectedBomb].count > 0)
        { 

        //Transfer all bomb attributes here from the player's inventory data to the new bomb
        newBombClass.attributes = craftedBombs[selectedBomb];

        craftedBombs[selectedBomb].count--;

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
            
        }
    }

    void makeBombDefaults(ref BombAttributes.BombData bombToReset)
    {
        //Set some stuff to 0 and some specific stuff to 
        bombToReset = default(BombAttributes.BombData);
        bombToReset.explosionScaleSpeed = new Vector3(4.0f, 4.0f, 4.0f);
        bombToReset.explosionScaleLimit = 10.0f;
        bombToReset.explosionLifetime = 3.0f;
        bombToReset.fire = 0;
        bombToReset.freeze = 0;
        bombToReset.damage = 25.0f;
    }

    void setInventoryText()
    {
        for (int i =0; i < 5; i++)
        {
            if (craftedBombs[i].count > 0) textForInventory[i].text = craftedBombs[i].count.ToString();
            else textForInventory[i].text = "";


        }

    }

    public void damage(float damageAmount)
    {
        health -= damageAmount;
        healthText.text = health.ToString();
    }

    void manageStatusEffects()
    {
        if (burning > 0.0f)
        {

            damage(1.0f * Time.deltaTime);
            burning -= 1.0f * Time.deltaTime;
        }

        if (stunned > 0.0f)
        {
            stunned -= 1.0f * Time.deltaTime;
        }
    }

    //Add a status effect based on a number. Duration is based on ticks * delta time
    public void addStatusEffect(int statusID, float duration)
    {
        switch (statusID)
        {
            case 1:

                burning = duration;
                break;

            case 2:

                stunned = duration;
                break;


        }
    }
}