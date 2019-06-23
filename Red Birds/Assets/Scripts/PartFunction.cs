using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartFunction : MonoBehaviour
{
    private GameObject part;
    // Start is called before the first frame update
    void Start()
    {
        part = this.transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
