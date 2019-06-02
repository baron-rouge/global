using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour
{

    public Rigidbody rb;
    public Text myText = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string str = Mathf.Round(rb.velocity.z + rb.velocity.x).ToString();

        myText.text = "Vitesse horizontale : " + str + "m/s";
    }
}
