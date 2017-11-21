using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //This object will be what the camera follows



    public GameObject playerFollowing;

    //Player's varialbes that will be used
    private string playerInputString;
    bool player1;
    float m_RotationSpeed;


	// Use this for initialization
	void Start () {
        playerInputString = playerFollowing.GetComponent<Player>().playerInputString;
        if (playerInputString == "") player1 = true;
        else player1 = false;
        m_RotationSpeed = playerFollowing.GetComponent<Player>().m_RotationSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        //Position also is player's position. Rotation and scale is NOT.
        if (playerFollowing == null) return;
        transform.position = playerFollowing.transform.position;



        //Rotation/camera input
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
        }
    }
}
