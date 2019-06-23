using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncJetEngine : PartFunction
{
    private string engineID;
    private Transform thrustTransform;
    private float minThrust;
    private float maxThrust;
    private float accelerationSpeed;
    private float decelerationSpeed;
    private bool useVelocityCurve;
    private Propellant propellant;
    private AnimationCurve ispCurve;

    private PID pid = new PID(0.5f, 0, 0);
    private float currentThrust;
    private float requestedThrust;
    private float t;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        UpdateThrottle();
    }
    private void UpdateThrottle()
    {
        t = Time.fixedDeltaTime;
        pid.Update(requestedThrust, currentThrust, t);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
