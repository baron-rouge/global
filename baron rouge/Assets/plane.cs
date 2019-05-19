using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    public float angleOfAttack;
    public Rigidbody rb;
    public float EnginePower;
    public float torque;
    private void FixedUpdate()
    {
        InputControl();
        rb.AddForce(transform.forward * EnginePower);
        calculateForces();
    }
    
    public float wingSpan = 10f;
    public float wingArea = 43f;
    private float aspectRatio;
    /*void Update()
    {
        if (Input.GetKey("s"))
        {
            angleOfAttack += 0.05f;
            //Debug.Log("s a été préssé");
        } else if (Input.GetKey("z"))
        {
            angleOfAttack -= 0.05f;
        }
    }*/
    private void Awake()
    {

        //rb.drag = Mathf.Epsilon;
        aspectRatio = (wingSpan * wingSpan) / wingArea;
    }
    
    private void calculateForces()
    {
        // *flip sign(s) if necessary*
        var localVelocity = transform.InverseTransformDirection(rb.velocity);
        Vector3 directionVector = Vector3.Normalize(rb.velocity) * 360;
        //var angleOfAttack = Vector3.Angle(transform.forward, directionVector);

        var dirVel = Quaternion.LookRotation(rb.velocity) * Vector3.up;

        Vector3 forward = rb.rotation * Vector3.forward;

        angleOfAttack = Mathf.Asin(Vector3.Dot(forward, dirVel)) * Mathf.Rad2Deg;



        
        // α * 2 * PI * (AR / AR + 2)
        var inducedLift = angleOfAttack * (aspectRatio / (aspectRatio + 2f)) * 2f * Mathf.PI;

        // CL ^ 2 / (AR * PI)
        var inducedDrag = (inducedLift * inducedLift) / (aspectRatio * Mathf.PI);

        // V ^ 2 * R * 0.5 * A
        var pressure = rb.velocity.sqrMagnitude * 1.2754f * 0.5f * wingArea;

        var cl = 2 * Mathf.PI * angleOfAttack * Mathf.Deg2Rad;

        //var lift = inducedLift * pressure;
        var lift = rb.velocity.magnitude * rb.velocity.magnitude * 1.225f * wingArea * cl;
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

    private void InputControl()
    {
        float pitch = Input.GetAxis("Vertical") * torque;
        float roll = Input.GetAxis("Roll") * torque;

        rb.AddTorque(transform.right * pitch);
        rb.AddTorque(transform.forward * roll);
        Debug.Log("pitch: " + pitch);
    }

}
