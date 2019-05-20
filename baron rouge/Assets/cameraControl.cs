using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;       //Public variable to store a reference to the player game object


    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    public float camHspd = 0.1f;
    public float camVspd = 5f;

    public float minZoom = 70f;
    public float maxZoom = 200f;
    public float zoomSpeed = 20f;

    private float camyaw = 0.0f;
    private float campitch = 0.0f;
    private float MouseXOffset = 0.0f;
    private float MouseYOffset = 0.0f;

    private float distance;

    // Use this for initialization
    void Start()
    {
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        offset = transform.position - player.transform.position;
        distance = offset.magnitude;
    }

    // LateUpdate is called after Update each frame
    void Update()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player.transform.position + player.transform.forward * -20 + player.transform.up * 5;

        //zoom
        distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, minZoom, maxZoom);
        offset.z = -distance;

        // Orbit Camera around player
        if (Input.GetMouseButtonDown(1))
        {
            MouseXOffset = Input.GetAxis("Mouse X");
            MouseYOffset = Input.GetAxis("Mouse Y");
            camyaw = 0.0f;
            campitch = 0.0f;
        }
        if (Input.GetMouseButton(1))
        {
            camyaw += offset.magnitude * (Input.GetAxis("Mouse X") - MouseXOffset);
            campitch -= (Input.GetAxis("Mouse Y") - MouseYOffset);

            Orbit(camyaw, campitch);
        }
        transform.LookAt(player.GetComponent<Rigidbody>().velocity * 999, Vector3.up);
    }

    private void Orbit(float _x, float _y)
    {

        Quaternion rotation = Quaternion.Euler(_y * camVspd, _x * camHspd, 0);

        Vector3 position = rotation * offset + player.transform.position;

        transform.position = position;
    }

}
