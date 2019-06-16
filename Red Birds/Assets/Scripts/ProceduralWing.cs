using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralWing : MonoBehaviour
{
    MeshFilter mf;
    Mesh mesh;
    MeshCollider mc;
    public Transform arrowLength;
    Vector3 arrowLengthPos;
    Vector3 lastArrowLengthPos;
    MeshRenderer arrowLengthRenderer;
    // Start is called before the first frame update
    void Start()
    {
        mf = gameObject.GetComponent<MeshFilter>();
        mesh = mf.mesh;
        mc = gameObject.GetComponent<MeshCollider>();
        lastArrowLengthPos = arrowLength.localPosition;
        arrowLengthRenderer = arrowLength.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        arrowLengthPos = arrowLength.localPosition;
        if (lastArrowLengthPos == arrowLengthPos)
            return;
        MeshUtils.CreateWing(mesh, transform.position.x - arrowLengthRenderer.bounds.center.x - arrowLengthRenderer.bounds.extents.x, 1f, 0.2f, arrowLengthPos.z, 0.1f, 0.02f);
        lastArrowLengthPos = arrowLengthPos;
    }
}
