using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeGenerator : MonoBehaviour
{

    public List<GameObject> AttachedNodes;
    public bool[] exists = {false, false, false, false};
    public static int numNodes = 0;
    public const int maxDepth = 6;
    public const int maxNodes = 2;
    public GameObject nodeToInitalize;
    
    public bool primaryNode = false;
    public int life = 0;
    public GameManager TimeManager;

    private bool dead = false;
    public int NodeType;


    // Use this for initialization
    void Start()
    {
        TimeManager = GameObject.FindObjectOfType<GameManager>();
        if (TimeManager.NumDepthReached <= maxDepth)
        {
            if (primaryNode)
                spawnNode(8);
            else
                spawnNode(4);
        }
        
    }

    void spawnNode(int num)
    {
        if (TimeManager.NumDepthReached <= maxDepth)
        {
            for (int i = 0; i < num; i++)
            {
                Vector3 Pos;

                switch (Random.Range(0, 4))
                {
                    case 0:
                        if (exists[0] == false)
                        {
                            int nodeType = ChooseNodeType();
                            Collider chosenRenderer;
                            chosenRenderer = gameObject.GetComponentInChildren<Collider>();
                            Pos = new Vector3(
                                chosenRenderer.bounds.size.x + gameObject.transform.position.x,
                                chosenRenderer.bounds.size.y / 2,
                                gameObject.transform.position.z);
                            GameObject newnode =
                                Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[1] = true;
                            newnode.GetComponent<NodeGenerator>().life = this.life + 10;
                            if (newnode.GetComponent<NodeGenerator>().life == (10 * maxDepth))
                            {
                                TimeManager.NumDepthReached++;
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
                            int nodeType = ChooseNodeType();
                            Collider chosenRenderer;
                            chosenRenderer = gameObject.GetComponentInChildren<Collider>();
                            Pos = new Vector3(
                            -chosenRenderer.bounds.size.x + gameObject.transform.position.x,
                            chosenRenderer.bounds.size.y / 2,
                            gameObject.transform.position.z);
                            
                            
                            GameObject newnode =
                                Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            
                            newnode.GetComponent<NodeGenerator>().exists[0] = true;
                            newnode.GetComponent<NodeGenerator>().life = this.life + 10;
                            if (newnode.GetComponent<NodeGenerator>().life == (10 * maxDepth))
                            {
                                TimeManager.NumDepthReached++;
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
                            int nodeType = ChooseNodeType();
                            Collider chosenRenderer;
                            chosenRenderer = gameObject.GetComponentInChildren<Collider>();
                            Pos = new Vector3(gameObject.transform.position.x,
                                chosenRenderer.bounds.size.y / 2,
                                chosenRenderer.bounds.size.z + gameObject.transform.position.z);
                            GameObject newnode =
                                Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[3] = true;
                            newnode.GetComponent<NodeGenerator>().life = this.life + 10;
                            if (newnode.GetComponent<NodeGenerator>().life == (10 * maxDepth))
                            {
                                TimeManager.NumDepthReached++;
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
                            int nodeType = ChooseNodeType();
                            Collider chosenRenderer;
                            chosenRenderer = gameObject.GetComponentInChildren<Collider>();
                            Pos = new Vector3(gameObject.transform.position.x,
                                chosenRenderer.bounds.size.y / 2,
                                -chosenRenderer.bounds.size.z + gameObject.transform.position.z);
                            GameObject newnode =
                                Instantiate(nodeToInitalize, Pos, gameObject.transform.rotation) as GameObject;
                            newnode.GetComponent<NodeGenerator>().exists[2] = true;
                            newnode.GetComponent<NodeGenerator>().life = this.life + 10;
                            if (newnode.GetComponent<NodeGenerator>().life == (10 * maxDepth))
                            {
                                TimeManager.NumDepthReached++;
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

    }

    int ChooseNodeType(bool overrideAll = false)
    {
        if (!overrideAll)
        {
            int selectedMod = Random.Range(0, TimeManager.nodes.Count);
            nodeToInitalize = TimeManager.nodes[selectedMod];
            Debug.Log(selectedMod + " was the biome selected");
            return selectedMod;
        }
        else
        {
            nodeToInitalize = TimeManager.nodes[2];
            return 0;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //Warning feedback
        if (TimeManager.time == life + 5 && !dead)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", new Color(0.8f, 0.0f, 0.0f));

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
