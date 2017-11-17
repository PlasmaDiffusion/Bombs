using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SQAUSHSTRETCH : MonoBehaviour {

    public Vector3 collisionNormal;
    Vector3 originalScale;
    bool recentCollision;
    int collisionTimer;
    float startTime;
    public float journeyTime = 5.0f;

	// Use this for initialization
	void Start () {

        //find out if a rigid body already exists, and if it doesn't, make one.
		if (gameObject.GetComponent<Rigidbody>() == null)
        {
            gameObject.AddComponent<Rigidbody>();
        }
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        

        originalScale = gameObject.transform.localScale;
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            collisionNormal = Vector3.Reflect(collision.contacts[0].normal, Vector3.right) / 2;
            recentCollision = true;
            startTime = Time.time;
        }
    }

        // Update is called once per frame
    void Update () {
        if (recentCollision)
        {
            float fracComplete = (Time.time - startTime) / journeyTime;
            if (fracComplete >= 0.25f)
            {
                gameObject.transform.localScale = Vector3.Slerp(-collisionNormal, originalScale, (fracComplete - 0.25f) * 4);
            }
            else if (fracComplete >= 0.5f)
            {
                gameObject.transform.localScale = Vector3.Slerp(originalScale, collisionNormal, (fracComplete - 0.5f) * 4);
            }
            else if (fracComplete >= 0.75f)
            {
                gameObject.transform.localScale = Vector3.Slerp(collisionNormal, originalScale, (fracComplete - 0.75f) * 4);
            }
            else
            {
                gameObject.transform.localScale = Vector3.Slerp(originalScale, -collisionNormal, fracComplete * 4);
            }
            
            Debug.Log("scale set as " + collisionNormal.ToString());
            
        }

        
        
    }
}
