using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public float mass;
    public string partName;
    public string title;
    public string description;
    public float crashTolerance;
    public float maxTemp;
    public int crewCap;
    public PartUpgrade[] upgrades;
    public Node[] nodes;
    public Category category;
    public Material nodeMaterial;
    public Mesh arrowMesh;

    private GameObject[] ng;
    private GameObject[] arrows;
    private MeshRenderer rend;
    private Vector3 screenPoint;
    private Vector3 offset;
    private bool move;


    // Start is called before the first frame update
    void Start()
    {
        rend = this.GetComponent<MeshRenderer>();

        ng = new GameObject[nodes.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            ng[i] = new GameObject();
            ng[i].name = "Node" + i;
            ng[i].transform.parent = this.transform;
            ng[i].transform.localPosition = nodes[i].position;
            ng[i].AddComponent<MeshRenderer>();
            ng[i].GetComponent<MeshRenderer>().material = nodeMaterial;
            ng[i].AddComponent<MeshFilter>();
            MeshUtils.CreateSphere(ng[i].GetComponent<MeshFilter>().mesh, 0.1f, 12, 12);
            ng[i].SetActive(false);
        }

        if (this.GetComponent<ProceduralCone>())
        {
            arrows = new GameObject[2];
            arrows[0] = new GameObject("arrowTR");
            arrows[0].SetActive(false);
            arrows[0].transform.parent = this.transform;
            arrows[0].transform.localScale = new Vector3(0.3f, 0.3f, 0.8f);
            Quaternion rot = new Quaternion();
            rot.eulerAngles = new Vector3(0, -90, 0);
            arrows[0].transform.localRotation = rot;
            arrows[0].AddComponent<MeshFilter>();
            arrows[0].AddComponent<MeshRenderer>();
            arrows[0].GetComponent<MeshFilter>().mesh = arrowMesh;
            arrows[0].GetComponent<MeshRenderer>().material = nodeMaterial;
            arrows[0].transform.localPosition = new Vector3(-0.8f, 1, 0);
            arrows[0].AddComponent<flecheControl>();
            arrows[0].GetComponent<flecheControl>().axis = "x";
            //arrows[0].AddComponent<MeshCollider>();
            arrows[0].GetComponent<MeshCollider>().sharedMesh = arrowMesh;

            arrows[1] = new GameObject("arrowBR");
            arrows[1].SetActive(false);
            arrows[1].transform.parent = this.transform;
            arrows[1].transform.localScale = new Vector3(0.3f, 0.3f, 0.8f);
            rot.eulerAngles = new Vector3(0, 90, 0);
            arrows[1].transform.localRotation = rot;
            arrows[1].AddComponent<MeshFilter>();
            arrows[1].AddComponent<MeshRenderer>();
            arrows[1].GetComponent<MeshFilter>().mesh = arrowMesh;
            arrows[1].GetComponent<MeshRenderer>().material = nodeMaterial;
            arrows[1].transform.localPosition = new Vector3(0.8f, -1, 0);
            arrows[1].AddComponent<flecheControl>();
            arrows[1].GetComponent<flecheControl>().axis = "x";
            //arrows[1].AddComponent<MeshCollider>();
            arrows[1].GetComponent<MeshCollider>().sharedMesh = arrowMesh;
            this.GetComponent<ProceduralCone>().flecheBr = arrows[1].transform;
            this.GetComponent<ProceduralCone>().flecheTr = arrows[0].transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (move && arrows[0].activeSelf)
        {
            foreach (GameObject arr in arrows)
            {
                arr.SetActive(false);
            }
        }
    }

    private void OnMouseDown()
    {
        foreach (GameObject go in ng)
        {
            go.SetActive(!go.activeSelf);
        }

        foreach (GameObject arr in arrows)
        {
            arr.SetActive(false);
        }

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        move = !move;
    }

    void OnMouseOver()
    {
        if (move)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = new Vector3(curPosition.x, curPosition.y, curPosition.z);
        }
        if (Input.GetMouseButtonDown(2))
        {
            foreach (GameObject arr in arrows)
            {
                arr.SetActive(!arr.activeSelf);
            }
        }
    }

    private void OnMouseEnter()
    {
        rend.sharedMaterial.SetFloat("_Highlight", 1);
    }

    private void OnMouseExit()
    {
        rend.sharedMaterial.SetFloat("_Highlight", 0);
    }
}
