using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunOrbit : MonoBehaviour
{
    public static float centerx;
    public static float centerz;
    Vector3 center;
    void Start()
    {
        
        centerx = Terrain.activeTerrain.terrainData.size.x / 2f;
        centerz = Terrain.activeTerrain.terrainData.size.y / 2f;
        center = new Vector3(centerx, 0, centerz);
    }

    public float theta = 0.1f;


    void Update()
    {
        this.transform.RotateAround(center, Vector3.right, theta);
    }
}
