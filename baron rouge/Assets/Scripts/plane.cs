using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{
    float angleOfAttack;
    private float throttle;
    public Rigidbody rb;
    public float EnginePower;
    public float torque;
    float stall;
    private void FixedUpdate()
    {
        InputControl();
        rb.AddForce(transform.forward * EnginePower);
        calculateForces();
        PitchPID();
        RollPID();
        YawPID();
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

        float PerpVelocity = Vector3.Dot(transform.forward, rb.velocity.normalized);
        //angleOfAttack = Mathf.Asin(Vector3.Dot(forward, dirVel)) * Mathf.Rad2Deg;
        angleOfAttack = Mathf.Asin(Mathf.Clamp(PerpVelocity, -1, 1));
        if (angleOfAttack < 1f)
            angleOfAttack = 1f;


        
        // α * 2 * PI * (AR / AR + 2)
        var inducedLift = angleOfAttack * (aspectRatio / (aspectRatio + 2f)) * 2f * Mathf.PI;



        // V ^ 2 * R * 0.5 * A
        //var pressure = rb.velocity.sqrMagnitude * 1.2754f * 0.5f * wingArea;
        float q = 1.2754f * rb.velocity.sqrMagnitude * 0.5f;
        Vector3 ParallelInPlane = Vector3.ProjectOnPlane(rb.velocity, transform.forward).normalized;
        Vector3 perp = Vector3.Cross(transform.forward, ParallelInPlane).normalized;
        Vector3 liftDirection = Vector3.Cross(perp, rb.velocity).normalized;

        
        float minStall = 0f;
        float liftSlope = 4f;
        float lastStall = stall;
        stall = 0f;
        float tmp = 0f;
        float AoAMax = 1.6f / liftSlope;
        float AbsAoA = Mathf.Abs(angleOfAttack);
        if (AbsAoA > AoAMax)
        {
            stall = Mathf.Clamp((AbsAoA - AoAMax) * 10, 0, 1);
            stall += tmp;
            stall = Mathf.Max(stall, lastStall);
        }
        else
        {
            stall = 1 - Mathf.Clamp((AoAMax - AbsAoA) * 10, 0, 1);
            stall += tmp;
            stall = Mathf.Min(stall, lastStall);
        }

        stall = Mathf.Clamp(stall, lastStall - 2 * Time.fixedDeltaTime, lastStall + 2 * Time.fixedDeltaTime);
        stall = Mathf.Clamp(stall, 0, 1);

        float piARe = 4 * Mathf.PI;
        float Cl = liftSlope * Mathf.Sin(2f * angleOfAttack) * 0.5f;
        float Cd = (0.006f + Cl * Cl / piARe);
        stall = Mathf.Clamp(stall, minStall, 1);

        Cl -= Cl * stall * 0.5f;
        Cd += Cd * stall * 1.5f;
        Debug.Log("Cl : " + Cl);

        float S = 17f * 4.27f;
        Vector3 L = liftDirection * (q * Cl * S);
        Vector3 D = rb.velocity.normalized * (-q * Cd * S);
        Vector3 force = (L + D) * 0.001f;
        rb.AddForce(force * 2f);
        Debug.Log("speed :" + rb.velocity.magnitude); 
    }

    private void InputControl()
    {
        float pitch;
        float roll;

        //pitch = Input.GetAxis("PitchFr") * torque;
        //roll = Input.GetAxis("RollFr") * torque;
       
        
        pitch = Input.GetAxis("Vertical") * torque;
        roll = Input.GetAxis("Roll") * torque;

        rb.AddTorque(transform.right * pitch / 2 * rb.velocity.magnitude);
        rb.AddTorque(transform.forward * roll / 10 * rb.velocity.magnitude);
        //Debug.Log("pitch: " + pitch);
    }

    private void PitchPID()
    {
        PIDController p = new PIDController((float)System.Math.Round(rb.angularVelocity.x, 2), 5f, 10f, 5f);
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
        

        rb.AddTorque(transform.forward * p.Update(tarRoll, rb.angularVelocity.z, Time.deltaTime) * 100 * rb.velocity.magnitude);
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
