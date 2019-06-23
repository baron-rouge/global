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

    private GameObject[] ng;

    // Start is called before the first frame update
    void Start()
    {
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
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        foreach (Node n in nodes)
        {

        }
    }
}
