using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{

    public List<GameObject> AttachedNodes;
    public bool[] exists = {false, false, false, false};
    public static int numNodes = 0;
    public const int maxNodes = 200;
    public GameObject nodeToInitalize;
    public bool primaryNode = false;

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start Called");
        if (primaryNode)
            spawnNode(8);
        else
            spawnNode(2);
    }

    void spawnNode(int num)
    {

        if (numNodes <= maxNodes)
        {
            for (int i = 0; i < num; i++)
            {


                Vector3 Pos;

                switch (Random.Range(0, 4))
                {
                    case 0:
                        if (exists[0] == false)
                        {
                            Pos = new Vector3(gameObject.GetComponent<Renderer>().bounds.size.x + gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y / 2, gameObject.transform.position.z);
                            GameObject newnode = Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[1] = true;
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
                            Pos = new Vector3(- gameObject.GetComponent<Renderer>().bounds.size.x + gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y / 2,
                                gameObject.transform.position.z);
                            GameObject newnode = Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[0] = true;
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
                            Pos = new Vector3(gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y, gameObject.GetComponent<Renderer>().bounds.size.z + gameObject.transform.position.z);
                            GameObject newnode = Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[3] = true;
                            AttachedNodes.Add(newnode);
                            Debug.Log("case 2 fire");
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
                            Pos = new Vector3(gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y, -gameObject.GetComponent<Renderer>().bounds.size.z + gameObject.transform.position.z);
                            GameObject newnode = Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[2] = true;
                            AttachedNodes.Add(newnode);
                            Debug.Log("case 3 fire");
                            exists[3] = true;
                        }
                        else
                        {
                            Debug.Log("Fuck");
                        }
                        break;
                    default:
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
