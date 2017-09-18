using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScale : MonoBehaviour
{

    public Vector3 scaleSpeed;
    public float scaleLengthLimit; //How big the explosion can get
    public bool expanding; // Is the explosion scaling itself up?
    public float extraLifetime; //How long it takes for the explosion to disppear once it has reached its scale limit

    // Use this for initialization
    void Start()
    {
        Debug.Log("Explosion started");
        //scaleSpeed = new Vector3(4.0f, 4.0f, 4.0f);
        //scaleLengthLimit = 10.0f;
        expanding = true;
        //extraLifetime = 3.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (expanding)
        {
            //Increase explosion scale
            Vector3 currentScale = transform.localScale;
            currentScale += scaleSpeed * Time.deltaTime;
            transform.localScale = currentScale;

            //...Until it's limit is reached, then destroy it
            if (currentScale.magnitude > scaleLengthLimit) expanding = false;


        }
        else
        {
            //Already scaled up; now wait a brief moment and destroy the explosion

            extraLifetime -= 1.0f * Time.deltaTime;
            if (extraLifetime < 0.0f) Destroy(gameObject);

        }
    }

    public void setExplosionVariables(Vector3 newScaleSpeed, float newScaleLenghtLimit, float newExtraLifetime)
    {
        Debug.Log("Setting stuff");
        scaleSpeed = newScaleSpeed;
        scaleLengthLimit = newScaleLenghtLimit;
        extraLifetime = newExtraLifetime;


    }
    void OnTriggerEnter(Collider other)
    {
        //Knockback whatever is in the explosion

        Rigidbody otherRigidBody = other.gameObject.GetComponent<Rigidbody>();


        if (otherRigidBody)
        {
            otherRigidBody.velocity = transform.forward * 10.0f;
        }
        
    }
}
