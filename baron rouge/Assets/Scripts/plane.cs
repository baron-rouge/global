using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    public float angleOfAttack;
    private float throttle;
    public Rigidbody rb;
    public float EnginePower;
    public float torque;
    private void FixedUpdate()
    {
        InputControl();
        rb.AddForce(transform.forward * EnginePower);
        calculateForces();
        PitchPID();
        RollPID();
        //YawPID();
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
        /*Quaternion angVel = Quaternion.identity;
        float zRot = Mathf.Sin(rb.rotation.eulerAngles.z * Mathf.Deg2Rad) * Mathf.Rad2Deg;
        float prevX = rb.rotation.eulerAngles.x;
        

        Vector3 rot = new Vector3(0, -zRot * 0.8f, -zRot * 0.5f) * Time.deltaTime;
        angVel.eulerAngles = rot;
        angVel *= rb.rotation;
        angVel.eulerAngles = new Vector3(prevX, angVel.eulerAngles.y, angVel.eulerAngles.z);
        rb.rotation = angVel;
        */
        
        Vector3 directVel = (rb.rotation * Vector3.forward).normalized * rb.velocity.magnitude;
        rb.velocity = Vector3.Lerp(rb.velocity, directVel, Time.deltaTime);
        // *flip sign(s) if necessary*
        var localVelocity = transform.InverseTransformDirection(rb.velocity);
        Vector3 directionVector = Vector3.Normalize(rb.velocity) * 360;
        //var angleOfAttack = Vector3.Angle(transform.forward, directionVector);
        Vector3 dirVel;
        if (rb.velocity != Vector3.zero)
        {
            dirVel = Quaternion.LookRotation(rb.velocity) * Vector3.up;
        }
        else
        {
            dirVel = Vector3.up;
        }
        

        Vector3 forward = transform.rotation * Vector3.forward;

        angleOfAttack = Mathf.Asin(Vector3.Dot(forward, dirVel)) * Mathf.Rad2Deg;



        
        // α * 2 * PI * (AR / AR + 2)
        var inducedLift = angleOfAttack * (aspectRatio / (aspectRatio + 2f)) * 2f * Mathf.PI;

        

        // V ^ 2 * R * 0.5 * A
        var pressure = rb.velocity.sqrMagnitude * 1.2754f * 0.5f * wingArea;
        float cl;
        if (angleOfAttack < -1f || angleOfAttack > 26f)
        {
            cl = -Mathf.Abs((angleOfAttack - 13) / 26f) + 1f;
        }
        else
        {
            cl = Mathf.Clamp(-1f / 100f * (angleOfAttack * angleOfAttack - 26f * angleOfAttack - 31f), 0f, 2f);
        }
        //var cl = 2 * Mathf.PI * angleOfAttack * Mathf.Deg2Rad;
        var cd = 0.001f * Mathf.Clamp(angleOfAttack, 0f, 22f) * Mathf.Clamp(angleOfAttack, 0f, 22f) + 0.025f;

        //var lift = inducedLift * pressure;
        var lift = 0.5f * rb.velocity.magnitude * rb.velocity.magnitude * 1.225f * wingArea * cl;
        var inducedDrag = (lift * lift) / (0.5f * 1.225f * rb.velocity.magnitude * rb.velocity.magnitude * wingArea * Mathf.PI * 0.9f * aspectRatio);
        var formDrag = 0.5f * 1.225f * rb.velocity.magnitude * rb.velocity.magnitude * wingArea * cd;
        var drag = inducedDrag + formDrag;

        // *flip sign(s) if necessary*
        var dragDirection = Quaternion.LookRotation(rb.velocity) * Vector3.back;
        //var liftDirection = Vector3.Cross(dragDirection, transform.right);
        var liftDirection = Quaternion.LookRotation(rb.velocity) * Vector3.up;
        Debug.DrawRay(transform.position, liftDirection * 100, Color.green);
        Debug.DrawRay(transform.position, transform.forward * 100, Color.red);
        //Debug.Log("lift : " + lift);
        Debug.Log("speed : " + rb.velocity.magnitude);
        //Debug.Log("AoA : " + angleOfAttack);

        // Lift + Drag = Total Force
        rb.AddForce(liftDirection * lift);
        //rb.AddForce(dragDirection * drag);
    }

    private void InputControl()
    {
        float pitch;
        float roll;

        //pitch = Input.GetAxis("PitchFr") * torque;
        //roll = Input.GetAxis("RollFr") * torque;
       
        
        pitch = Input.GetAxis("Vertical") * torque;
        roll = Input.GetAxis("Roll") * torque;

        rb.AddTorque(transform.right * pitch * rb.velocity.magnitude);
        rb.AddTorque(transform.forward * roll / 10 * rb.velocity.magnitude);
        //Debug.Log("pitch: " + pitch);
    }

    private void PitchPID()
    {
        PIDController p = new PIDController(rb.angularVelocity.x, 5f, 10f, 5f);
        if (Input.GetAxis("Vertical") != 0)
        {
            return;
        }
        float tarPitch = 0;
        float curPitch;

        if (transform.rotation.eulerAngles.x > 180)
        {
            curPitch = 360 - transform.rotation.eulerAngles.x;
        }
        else
        {
            curPitch = 0 - transform.rotation.eulerAngles.x;
        }

        rb.AddTorque(transform.right * p.Update(tarPitch, rb.angularVelocity.x, Time.deltaTime) * 1000 * rb.velocity.magnitude);
        
        Debug.Log("pitch : " + curPitch);
        
    }

    private void RollPID()
    {
        PIDController p = new PIDController(rb.angularVelocity.z, 5f, 1f, 5f);
        if (Input.GetAxis("Roll") != 0)
        {
            return;
        }
        float tarRoll = 0;
        

        rb.AddTorque(transform.forward * p.Update(tarRoll, rb.angularVelocity.z, Time.deltaTime) * 10 * rb.velocity.magnitude);
    }

    private void YawPID()
    {
        PID p = new PID(10f, 1f, 0f);
        rb.AddTorque(transform.up * p.Update(0, rb.angularVelocity.y, Time.deltaTime) * 1000 * rb.velocity.magnitude);
    }

    private float lastPError = 0;
    private void PIDLoop()
    {
        float iError = 0;
        var pError = 0 - rb.angularVelocity.x;
        iError += pError * Time.deltaTime;
        var dError = (pError - lastPError) / Time.deltaTime;
        lastPError = pError;
        var tor = 1f * pError + 0 * iError + 0 * dError;

        rb.AddTorque(transform.right * tor * 10000);
    }

}
