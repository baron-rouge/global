using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    public float angleOfAttack;
    public Rigidbody rb;
    public float EnginePower;
    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * EnginePower);
        calculateForces();
    }
    
    public float wingSpan = 13.56f;
    public float wingArea = 78.04f;
    private float aspectRatio;
    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            angleOfAttack += 0.1f;
            Debug.Log("s a été préssé");
        } else if (Input.GetKeyDown("z"))
        {
            angleOfAttack -= 0.1f;
        }
    }
    private void Awake()
    {

        //rb.drag = Mathf.Epsilon;
        aspectRatio = (wingSpan * wingSpan) / wingArea;
        Debug.Log(rb.transform.GetComponentInChildren<Renderer>().bounds.size);
    }
    
    private void calculateForces()
    {
        // *flip sign(s) if necessary*
        var localVelocity = transform.InverseTransformDirection(rb.velocity);
        Vector3 directionVector = Vector3.Normalize(rb.velocity) * 360;
        //var angleOfAttack = Vector3.Angle(transform.forward, directionVector);

        
        // α * 2 * PI * (AR / AR + 2)
        var inducedLift = angleOfAttack * (aspectRatio / (aspectRatio + 2f)) * 2f * Mathf.PI;

        // CL ^ 2 / (AR * PI)
        var inducedDrag = (inducedLift * inducedLift) / (aspectRatio * Mathf.PI);

        // V ^ 2 * R * 0.5 * A
        var pressure = Mathf.Pow(rb.velocity.z, 2) * 1.2754f * 0.5f * wingArea;

        var lift = inducedLift * pressure;
        var drag = (0.021f + inducedDrag) * pressure;

        // *flip sign(s) if necessary*
        var dragDirection = rb.velocity.normalized;
        //var liftDirection = Vector3.Cross(dragDirection, transform.right);
        var liftDirection = transform.up;
        Debug.DrawRay(rb.position, liftDirection * 100, Color.green);
        Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
        Debug.Log("lift : " + lift);
        Debug.Log("speed : " + rb.velocity.z);
        Debug.Log("AoA : " + angleOfAttack);

        // Lift + Drag = Total Force
        rb.AddForce(liftDirection * lift);
    }


}
