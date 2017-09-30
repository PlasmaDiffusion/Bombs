using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {

    public float time;
    public GameObject newExplosion;
    private bool active;

    //Variables that change based on what materials were used
    public float explosionLifetime;
    public Vector3 explosionScaleSpeed;
    public float explosionScaleLimit;

    public BombAttributes.BombData attributes;

    // Use this for initialization
    void Start () {
        /*
        Debug.Log("start");

        time = 1.0f;
        active = true;

        explosionScaleSpeed = new Vector3(4.0f, 4.0f, 4.0f);
        explosionScaleLimit = 10.0f;
        explosionLifetime = 3.0f;*/
    }
	
	// Update is called once per frame
	void Update () {

        if (time > 0)
        {
            time -= 1.0f * Time.deltaTime;
        }
        else
        {
            //Explode! Create an explosion object and destroy self
            GameObject bombExplosion = Instantiate(newExplosion, transform.position, transform.rotation);
            

            //Modify that explosion based on explosion variables
            ExplosionScale newExplosionClass = bombExplosion.GetComponent<ExplosionScale>();

            newExplosionClass.explosionAttributes = attributes;
           

            Destroy(gameObject);
        }
    }

}
