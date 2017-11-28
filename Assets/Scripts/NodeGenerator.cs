using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{

    public List<GameObject> AttachedNodes;
    public bool[] exists = {false, false, false, false};
    public static int numNodes = 0;
    public const int maxDepth = 4;
    public static int numDepthReached = 0;
    public GameObject nodeToInitalize;
    public bool primaryNode = false;
    public int life = 0;
    public GameManager TimeManager;

    private bool dead = false;
    public bool terminalNode = false;

    // Use this for initialization
    void Start()
    {
        TimeManager = GameObject.FindObjectOfType<GameManager>();
        if (!terminalNode)
        {
            if (primaryNode)
                spawnNode(8);
            else
                spawnNode(4);
        }
        
    }

    void spawnNode(int num)
    {
            for (int i = 0; i < num; i++)
            {


                Vector3 Pos;

                switch (Random.Range(0, 4))
                {
                    case 0:
                        if (exists[0] == false)
                        {
                            if (life == (10 * maxDepth))
                            {
                                spawnFringe();
                                numDepthReached++;
                            }

                            Pos = new Vector3(
                                gameObject.GetComponent<Renderer>().bounds.size.x + gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y / 2,
                                gameObject.transform.position.z);
                            GameObject newnode =
                                Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[1] = true;
                            newnode.GetComponent<NodeGenerator>().life = this.life + 10;
                            if (life == (10 * maxDepth))
                            {
                                newnode.GetComponent<NodeGenerator>().terminalNode = true;
                            }
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
                            if (life == (10 * maxDepth))
                            {
                                spawnFringe();
                                numDepthReached++;
                            }
                            Pos = new Vector3(
                                -gameObject.GetComponent<Renderer>().bounds.size.x + gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y / 2,
                                gameObject.transform.position.z);
                            GameObject newnode =
                                Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[0] = true;
                            newnode.GetComponent<NodeGenerator>().life = this.life + 10;
                            if (life == (10 * maxDepth))
                            {
                                newnode.GetComponent<NodeGenerator>().terminalNode = true;
                            }
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
                            if (life == (10 * maxDepth))
                            {
                                spawnFringe();
                                numDepthReached++;
                            }
                            Pos = new Vector3(gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y,
                                gameObject.GetComponent<Renderer>().bounds.size.z + gameObject.transform.position.z);
                            GameObject newnode =
                                Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[3] = true;
                            newnode.GetComponent<NodeGenerator>().life = this.life + 10;
                            if (life == (10 * maxDepth))
                            {
                                newnode.GetComponent<NodeGenerator>().terminalNode = true;
                            }
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
                            if (life == (10 * maxDepth))
                            {
                                spawnFringe();
                                numDepthReached++;
                            }
                            Pos = new Vector3(gameObject.transform.position.x,
                                -gameObject.GetComponent<Renderer>().bounds.size.y,
                                -gameObject.GetComponent<Renderer>().bounds.size.z + gameObject.transform.position.z);
                            GameObject newnode =
                                Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[2] = true;
                            newnode.GetComponent<NodeGenerator>().life = this.life + 10;
                            if (life == (10 * maxDepth))
                            {
                                newnode.GetComponent<NodeGenerator>().terminalNode = true;
                            }
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

    void spawnFringe()
    {
        int toSpawn = Random.Range(0, TimeManager.TerminalNodes.Count);
        nodeToInitalize = TimeManager.TerminalNodes[toSpawn];
    }

    // Update is called once per frame
    void Update()
    {
        //Warning feedback
        if (TimeManager.time == life + 5 && !dead)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", gameObject.GetComponent<ParticleControl>().TargetColor);

        }

        //Destroy platform
        if (TimeManager.time <= life && !dead)
        {
        //Remove pickups on the platform too
        Instantiate(GameObject.Find("PickupDestroyer"), transform);

        gameObject.AddComponent<Rigidbody>();
        gameObject.GetComponent<Rigidbody>().useGravity = true;
        dead = true;
        }

        
        
    }
}
