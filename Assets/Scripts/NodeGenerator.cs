using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{

    public GameObject[] AttachedNodes;
    public GameObject NodeType;

	// Use this for initialization
	void Start () {
		spawnNode(5);
	}

    void spawnNode(int num)
    {
        

        for (int i = 0; i < num; i++)
        {
            Vector3 Pos;
            switch (Random.Range(0, 3))
            {
                case 0:
                    Pos = new Vector3(gameObject.GetComponent<Renderer>().bounds.size.x,
                        0, gameObject.GetComponent<Renderer>().bounds.size.z);
                    break;
                case 1:
                    Pos = new Vector3(-gameObject.GetComponent<Renderer>().bounds.size.x,
                        0, gameObject.GetComponent<Renderer>().bounds.size.z);
                    break;
                case 2:
                    Pos = new Vector3(gameObject.GetComponent<Renderer>().bounds.size.x,
                        0, -gameObject.GetComponent<Renderer>().bounds.size.z);
                    break;
                case 3:
                    Pos = new Vector3(-gameObject.GetComponent<Renderer>().bounds.size.x,
                        0, -gameObject.GetComponent<Renderer>().bounds.size.z);
                    break;
                default:
                    Pos = gameObject.GetComponent<Renderer>().bounds.size;
                    break;
            }
            
            Instantiate(NodeType, Pos, gameObject.transform.rotation);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
