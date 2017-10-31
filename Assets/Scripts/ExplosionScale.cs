using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScale : MonoBehaviour
{

    public bool expanding; // Is the explosion scaling itself up?
 
    public BombAttributes.BombData explosionAttributes;

    public GameObject[] emitterObjectReferences;

    GameObject[] particleEmitters;

    public bool firstBomb = false;

    private bool pullTowards = false;

    public float radius = 10.0F;
    public float power = 3000.0F;


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

            //Recolour explosion if bigger radius
        if (explosionAttributes.explosionScaleLimit > 15.0f)
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material.SetColor("_Color", new Color(0.6f, 0.0f, 0.0f, 0.5f));
        }

        if (explosionAttributes.blackhole > 0)
        {
            pullTowards = true;

            //Blackhole will deal less damage but also last around longer. (Good for combining with other things like fire!)
            explosionAttributes.damage = explosionAttributes.damage / 2.0f;
            explosionAttributes.explosionLifetime += (1.0f * explosionAttributes.blackhole);
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
    
    void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision enter");

        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddExplosionForce(power, explosionPos, radius, 3.0F);
        }

    }

    void OnTriggerEnter(Collider other)
    {
        //Do nothing as a first bomb
        if (firstBomb) return;

        //Knockback whatever is in the explosion

        Rigidbody otherRigidBody = other.gameObject.GetComponent<Rigidbody>();
        Player player = other.gameObject.GetComponent<Player>();


        if (otherRigidBody)
        {

            //Blast player in opposite direction of them relative to the explosion. (But not if a blackhole bomb)
            if (!pullTowards)
            { 
            Vector3 blastImpact;

            otherRigidBody.velocity = other.transform.up * 10.0f;

            blastImpact = Vector3.Normalize(other.transform.position - transform.position) * 10.0f;

             otherRigidBody.velocity += blastImpact;

            }

            if (player) damagePlayer(player);

        }

        


    }
    void OnTriggerStay(Collider other)
    {
        //Blackhole
       if (pullTowards)
        {
        Rigidbody otherRigidBody = other.gameObject.GetComponent<Rigidbody>();

        if (otherRigidBody)
            {

                Vector3 pullForce = Vector3.Normalize(other.transform.position - transform.position) * 0.5f;

                otherRigidBody.velocity -= pullForce;

                
            }
        }

    }

    void damagePlayer(Player player)
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

        //And finally check if the player died.
        player.checkIfDead();

    }



}
