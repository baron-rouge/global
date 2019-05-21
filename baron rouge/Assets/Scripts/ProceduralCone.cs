using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCone : MonoBehaviour
{
    public Transform fleche;
    Vector3 lastArrowPos;
    MeshFilter mf;
    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        mf = gameObject.GetComponent<MeshFilter>();
        mesh = mf.mesh;
        lastArrowPos = fleche.position;
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 arrowPos = fleche.position;
        if (arrowPos == lastArrowPos)
            return;
        
        MeshUtils.CreateCone(2f, fleche.GetComponent<MeshRenderer>().bounds.center.x - fleche.GetComponent<MeshRenderer>().bounds.extents.x, 0.5f, 18, mesh);
    
    }
}
