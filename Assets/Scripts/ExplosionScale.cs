using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScale : MonoBehaviour
{

    public bool expanding; // Is the explosion scaling itself up?
 
    public BombAttributes.BombData explosionAttributes;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Explosion started");
        //scaleSpeed = new Vector3(4.0f, 4.0f, 4.0f);
        //scaleLengthLimit = 10.0f;
        expanding = true;
        //extraLifetime = 3.0f;

        /*if (explosionAttributes.smoke)
        {

        }*/
    }

    // Update is called once per frame
    void Update()
    {

        if (expanding)
        {
            //Increase explosion scale
            Vector3 currentScale = transform.localScale;
            currentScale += explosionAttributes.explosionScaleSpeed * Time.deltaTime;
            transform.localScale = currentScale;

            //...Until it's limit is reached, then destroy it
            if (currentScale.magnitude > explosionAttributes.explosionScaleLimit) expanding = false;


        }
        else
        {
            //Already scaled up; now wait a brief moment and destroy the explosion

            explosionAttributes.explosionLifetime -= 1.0f * Time.deltaTime;
            if (explosionAttributes.explosionLifetime < 0.0f) Destroy(gameObject);

        }
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
