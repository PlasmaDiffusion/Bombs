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

    private Transform camTransform;

    private float camVerticalMax;
    private float camVerticalMin;
    float currentPitch;
    private float camVerticalSpeed;


    // Use this for initialization
    void Start () {
        playerInputString = playerFollowing.GetComponent<Player>().playerInputString;
        if (playerInputString == "") player1 = true;
        else player1 = false;
        m_RotationSpeed = playerFollowing.GetComponent<Player>().m_RotationSpeed;

        camTransform = transform.GetChild(1);

        camVerticalMax = 30.0f;
        camVerticalMin = 0.0f;

        currentPitch = camTransform.localEulerAngles.x;
        Debug.Log(currentPitch);

        camVerticalSpeed = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
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


        //Rotate camera up or down
        float camVertical = (Input.GetAxis("RightStickV" + playerInputString));
        //Debug.Log(camRotate);
        /*
                //Debug.Log(camTransform.rotation.eulerAngles.x);
                if (camTransform.rotation.eulerAngles.x < 40.0f && camRotate > 0.0f)
                    {
                        camTransform.Rotate(camRotate, 0, 0);
                        //Debug.Log(camRotate);
                        Debug.Log("Rotating down?");
                    }


                    if(camTransform.rotation.eulerAngles.x > 0.0f && camRotate < 0.0f)
                    {
                        camTransform.Rotate(camRotate, 0, 0);
                        Debug.Log("Rotating up?");
                    }

            */
        if (camVertical > 0.2f || (player1 && Input.GetKey(KeyCode.I)))
        {
            if (camVertical == 0.0f) camVertical = -1.0f;

            currentPitch += camVertical * camVerticalSpeed * Time.deltaTime;
            currentPitch = Mathf.Clamp(currentPitch, camVerticalMin, camVerticalMax);
            camTransform.localEulerAngles = new Vector3(currentPitch, 0.0f, 0.0f);
            Debug.Log(currentPitch);
        }
        if (camVertical < -0.2f || (player1 && Input.GetKey(KeyCode.K)))
        {
            if (camVertical == 0.0f) camVertical = 1.0f;

            currentPitch += camVertical * camVerticalSpeed * Time.deltaTime;
            currentPitch = Mathf.Clamp(currentPitch, camVerticalMin, camVerticalMax);
            camTransform.localEulerAngles = new Vector3(currentPitch, 0.0f, 0.0f);
            Debug.Log(currentPitch);
        }


        /*  if (camVertical > 0.2f)
              camTransform.rotation = new Quaternion(40.0f, camTransform.rotation.eulerAngles.y, camTransform.eulerAngles.z, camTransform.rotation.w);

          else if (camVertical < -0.2f)
              ccamTransform.rotation = new Quaternion(40.0f, camTransform.rotation.eulerAngles.y, camTransform.eulerAngles.z, camTransform.rotation.w);
              */
    }
}
