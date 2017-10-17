using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{

    private readonly bool[] AttachedNodes = { false, false, false, false };
    public GameObject NodeType;
    public static int numNodes = 0;
    public const int maxNodes = 10;

	// Use this for initialization
	void Start () {
		spawnNode(2);
	}

    void spawnNode(int num)
    {

        if (numNodes <= maxNodes)
        {
            for (int i = 0; i < num; i++)
        {
            
            
                Vector3 Pos;
                
                switch (Random.Range(0, 3))
                {
                    case 0:
                        if (AttachedNodes[0] == false)
                        {
                            AttachedNodes[0] = true;
                            Pos = new Vector3(gameObject.GetComponent<Renderer>().bounds.size.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y / 2, gameObject.GetComponent<Renderer>().bounds.size.z);
                            Instantiate(NodeType, Pos, gameObject.transform.rotation);
                            Debug.Log("generate node");
                        }
                        else
                        {
                            Debug.Log("Fuck");
                        }
                        break;
                    case 1:
                        if (AttachedNodes[1] == false)
                        {
                            AttachedNodes[1] = true;
                            Pos = new Vector3(0,
                                -gameObject.GetComponent<Renderer>().bounds.size.y / 2,
                                gameObject.GetComponent<Renderer>().bounds.size.z);
                            Instantiate(NodeType, Pos, gameObject.transform.rotation);
                            Debug.Log("generate node");
                        }
                        else
                        {
                            Debug.Log("Fuck");
                        }
                        break;
                    case 2:
                        if (AttachedNodes[2] == false)
                        {
                            AttachedNodes[2] = true;
                            Pos = new Vector3(gameObject.GetComponent<Renderer>().bounds.size.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y, 0);
                            Instantiate(NodeType, Pos, gameObject.transform.rotation);
                            Debug.Log("generate node");
                        }
                        else
                        {
                            Debug.Log("Fuck");
                        }
                        break;
                    case 3:
                        if (AttachedNodes[3] == false)
                        {
                            AttachedNodes[3] = true;
                            Pos = new Vector3(0,
                                -gameObject.GetComponent<Renderer>().bounds.size.y, 0);
                            Instantiate(NodeType, Pos, gameObject.transform.rotation);
                            Debug.Log("generate node");
                        }
                        else
                        {
                            Debug.Log("Fuck");
                        }
                        break;
                    default:
                        Pos = gameObject.GetComponent<Renderer>().bounds.size;
                        break;
                }
                numNodes++;

            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
