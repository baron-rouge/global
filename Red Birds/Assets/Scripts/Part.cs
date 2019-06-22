using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    [SerializeField]
    private float mass;
    [SerializeField]
    private string partName;
    [SerializeField]
    private string title;
    [SerializeField]
    private Category category;
    [SerializeField]
    private string description;

    public string Name => partName;
    public string Title => title;
    public float Mass => mass;
    public Category Category => category;
    public string Description => description;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
