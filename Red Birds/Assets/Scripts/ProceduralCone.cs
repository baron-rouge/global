using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCone : MonoBehaviour
{
    public Transform flecheBr;
    public Transform flecheTr;
    Vector3 lastArrowPosBr;
    Vector3 lastArrowPosTr;
    MeshFilter mf;
    Mesh mesh;
    MeshCollider mc;
    Texture mainTex;
    Material mat;
    // Start is called before the first frame update
    void Start()
    {
        mf = gameObject.GetComponent<MeshFilter>();
        mesh = mf.mesh;
        lastArrowPosBr = flecheBr.localPosition;
        lastArrowPosTr = flecheTr.localPosition;
        mc = gameObject.GetComponent<MeshCollider>();
        mat = gameObject.GetComponent<Renderer>().material;
    }
    
    // Update is called once per frame
    void Update()
    {
        Vector3 arrowPosBr = flecheBr.localPosition;
        Vector3 arrowPosTr = flecheTr.localPosition;
        if (arrowPosBr.x == lastArrowPosBr.x && arrowPosTr.x == lastArrowPosTr.x)
            return;
        
        MeshUtils.CreateCone(2f, flecheBr.GetComponent<MeshRenderer>().bounds.center.x - flecheBr.GetComponent<MeshRenderer>().bounds.extents.x - transform.position.x, transform.position.x - flecheTr.GetComponent<MeshRenderer>().bounds.center.x - flecheTr.GetComponent<MeshRenderer>().bounds.extents.x, 18, mesh);
        mc.sharedMesh = mesh;
        mat.mainTexture = mainTex;
        lastArrowPosTr = arrowPosTr;
        lastArrowPosBr = arrowPosBr;
    }

}
