using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{

    public List<GameObject> AttachedNodes;
    public bool[] exists = {false, false, false, false};
    public static int numNodes = 0;
    public const int maxNodes = 10;
    public GameObject nodeToInitalize;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start Called");
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
                        if (exists[0] == false)
                        {
                            Pos = new Vector3(gameObject.GetComponent<Renderer>().bounds.size.x + gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y / 2, gameObject.GetComponent<Renderer>().bounds.size.z + gameObject.transform.position.z);
                            GameObject newnode = Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            AttachedNodes.Add(newnode);
                            Debug.Log("generate node");
                            exists[0] = true;
                        }
                        else
                        {
                            Debug.Log("Fuck");
                        }
                        break;
                    case 1:
                        if (exists[1] == false)
                        {
                            Pos = new Vector3(0 + gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y / 2,
                                gameObject.GetComponent<Renderer>().bounds.size.z + gameObject.transform.position.z);
                            GameObject newnode = Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            AttachedNodes.Add(newnode);
                            Debug.Log("generate node");
                            exists[1] = true;
                        }
                        else
                        {
                            Debug.Log("Fuck");
                        }
                        break;
                    case 2:
                        if (exists[2] == false)
                        {
                            Pos = new Vector3(gameObject.GetComponent<Renderer>().bounds.size.x + gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y, 0 + gameObject.transform.position.z);
                            GameObject newnode = Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            AttachedNodes.Add(newnode);
                            Debug.Log("generate node");
                            exists[2] = true;
                        }
                        else
                        {
                            Debug.Log("Fuck");
                        }
                        break;
                    case 3:
                        if (exists[3] == false)
                        {
                            Pos = new Vector3(0 + gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y, gameObject.transform.position.z);
                            GameObject newnode = Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            AttachedNodes.Add(newnode);
                            Debug.Log("generate node");
                            exists[3] = true;
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
    void Update()
    {

    }
}
