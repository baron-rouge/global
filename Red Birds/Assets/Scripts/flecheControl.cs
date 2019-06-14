using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]

public class flecheControl : MonoBehaviour
{

    private Vector3 screenPoint;
    private Vector3 offset;
    public Transform cone;
    public string axis;

    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }

    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        if (axis == "x")
            transform.position = new Vector3((float)System.Math.Round(curPosition.x, 1), transform.position.y, transform.position.z);
        else if (axis == "xy")
            transform.position = new Vector3((float)System.Math.Round(curPosition.x, 1), transform.position.y, (float)System.Math.Round(curPosition.y, 1));

    }

}