using System;
using UnityEngine;

public class PID
{
    public float pFactor, iFactor, dFactor;

    float integral;
    float lastError;


    public PID(float pFactor, float iFactor, float dFactor)
    {
        this.pFactor = pFactor;
        this.iFactor = iFactor;
        this.dFactor = dFactor;
    }


    public float Update(float setpoint, float actual, float timeFrame)
    {
        float present = setpoint - actual;
        integral += present * timeFrame;
        float deriv = (present - lastError) / timeFrame;
        lastError = present;
        return present * pFactor + integral * iFactor + deriv * dFactor;
    }
}

public class PIDController
{
    public float Kp { get; private set; }

    public float Ki { get; private set; }

    public float Kd { get; private set; }

    public float OutputMin { get; private set; }

    public float OutputMax { get; private set; }

    float lastInput;
    float integralTerm;

   
    public PIDController(float input, float kp = 1, float ki = 0, float kd = 0, float outputMin = -1, float outputMax = 1)
    {
        Reset(input, kp, ki, kd, outputMin, outputMax);
    }

   
    public void Reset(float input, float kp = 1, float ki = 0, float kd = 0, float outputMin = -1, float outputMax = 1)
    {
        integralTerm = 0;
        lastInput = input;
        SetParameters(kp, ki, kd, outputMin, outputMax);
    }

    public void SetParameters(float kp, float ki, float kd, float outputMin = -1, float outputMax = 1)
    {
        Kp = kp;
        Ki = ki;
        Kd = kd;
        OutputMin = outputMin;
        OutputMax = outputMax;
        integralTerm = Mathf.Clamp(integralTerm, outputMin, outputMax);
    }

    public float Update(float setpoint, float input, float deltaTime)
    {
        var error = setpoint - input;
        integralTerm += Ki * error * deltaTime;
        integralTerm = Mathf.Clamp(integralTerm, OutputMin, OutputMax);
        var derivativeInput = (input - lastInput) / deltaTime;
        var output = Kp * error + integralTerm - Kd * derivativeInput;
        output = Mathf.Clamp(output, OutputMin, OutputMax);
        lastInput = input;
        return output;
    }

    public void ClearIntegralTerm()
    {
        integralTerm = 0;
    }
}
