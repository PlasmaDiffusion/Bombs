using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScale : MonoBehaviour
{

    public bool expanding; // Is the explosion scaling itself up?
 
    public BombAttributes.BombData explosionAttributes;

    public GameObject[] emitterObjectReferences;

    GameObject[] particleEmitters;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Explosion started");
        //scaleSpeed = new Vector3(4.0f, 4.0f, 4.0f);
        //scaleLengthLimit = 10.0f;
        expanding = true;
        //extraLifetime = 3.0f;
        

        particleEmitters = new GameObject[explosionAttributes.smoke];

        //Smoke particles
            for (int i = 0; i < explosionAttributes.smoke; i++)
        {
            particleEmitters[i] = Instantiate(emitterObjectReferences[0], transform.position, new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
        }

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
            //Already scaled up; now wait a brief moment and destroy the explosion (and any extra particle emitters)
            explosionAttributes.explosionLifetime -= 1.0f * Time.deltaTime;
            if (explosionAttributes.explosionLifetime < 0.0f)
                {
                for (int i = explosionAttributes.smoke - 1; i >= 0; i--) Destroy(particleEmitters[i]);

                Destroy(gameObject);
                }

            }
    }
    
    void OnTriggerEnter(Collider other)
    {
        //Knockback whatever is in the explosion

        Rigidbody otherRigidBody = other.gameObject.GetComponent<Rigidbody>();
        Player player = other.gameObject.GetComponent<Player>();


        if (otherRigidBody)
        {
            otherRigidBody.velocity = transform.forward * 10.0f;

            if (player)
            {

                //Deal some damage
                player.damage(explosionAttributes.damage);

            //Do extra affects here

            if (explosionAttributes.fire > 0)
                {
                player.addStatusEffect(1, 10.0f * explosionAttributes.fire);
                }
            if (explosionAttributes.freeze > 0)
                {
                player.addStatusEffect(2, 5.0f * explosionAttributes.freeze);
                }
            }
        }



        
    }


    
}
