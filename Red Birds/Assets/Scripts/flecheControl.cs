using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]

public class flecheControl : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;
    public Transform cone;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = new Vector3((float)System.Math.Round(curPosition.x, 1), transform.position.y, transform.position.z);

    }

}