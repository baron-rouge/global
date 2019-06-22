using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public ResourceType type;
    public float amount;
    public float maxAmount;
}

public enum ResourceType
{
    Fuel,
    EC,
    Air,
    Ammo30,
    Ammo20
}
